using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace IIProyectoDatos
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<FilaDatos> datos;
        private string distanciaSeleccionada = "Euclidiana";

        public MainWindow()
        {
            InitializeComponent();
            datos = new ObservableCollection<FilaDatos>();
            ConfigurarDataGrid();
        }

        private void ConfigurarDataGrid()
        {
            // Columna de Nombre (obligatoria)
            DataGridPeliculas.Columns.Add(new DataGridTextColumn
            {
                Header = "Nombre",
                Binding = new System.Windows.Data.Binding("Nombre"),
                Width = new DataGridLength(200)
            });

            // Las demás columnas serán agregadas dinámicamente cuando se cargue el CSV
            DataGridPeliculas.ItemsSource = datos;
        }

        private void BtnDistancia_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag != null)
            {
                distanciaSeleccionada = btn.Tag.ToString();
                MessageBox.Show($"Métrica de distancia seleccionada: {distanciaSeleccionada}", 
                    "Distancia", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCargarCSV_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Seleccionar archivo CSV"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    CargarCSV(openFileDialog.FileName);
                    MessageBox.Show("CSV cargado exitosamente", "Éxito", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar CSV: {ex.Message}", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CargarCSV(string ruta)
        {
            var lineas = File.ReadAllLines(ruta);
            if (lineas.Length < 2) return;

            // Leer encabezados
            var encabezados = lineas[0].Split(',').Select(h => h.Trim().Trim('"')).ToList();
            
            // Limpiar datos existentes
            datos.Clear();
            DataGridPeliculas.Columns.Clear();

            // Reconfigurar columnas según el CSV
            ConfigurarColumnasDesdeCSV(encabezados);

            // Cargar datos
            for (int i = 1; i < lineas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lineas[i])) continue;

                var valores = SepararLineaCSV(lineas[i]);
                var fila = new FilaDatos(encabezados.Count);
                
                for (int j = 0; j < Math.Min(valores.Length, encabezados.Count); j++)
                {
                    fila.SetValor(j, valores[j]);
                }

                datos.Add(fila);
            }
        }

        private void ConfigurarColumnasDesdeCSV(List<string> encabezados)
        {
            // Primera columna: Nombre (siempre texto)
            DataGridPeliculas.Columns.Add(new DataGridTextColumn
            {
                Header = encabezados[0],
                Binding = new System.Windows.Data.Binding($"Valores[0]"),
                Width = new DataGridLength(180)
            });

            // Resto de columnas con ComboBox para normalización
            for (int i = 1; i < encabezados.Count; i++)
            {
                int columnaIndex = i; // Captura para el binding
                
                var col = new DataGridTemplateColumn
                {
                    Header = encabezados[i],
                    Width = new DataGridLength(150)
                };

                // Template para mostrar el valor y el combo de normalización
                var cellTemplate = new DataTemplate();
                var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
                
                var textBox = new FrameworkElementFactory(typeof(TextBox));
                textBox.SetBinding(TextBox.TextProperty, 
                    new System.Windows.Data.Binding($"Valores[{columnaIndex}]"));
                textBox.SetValue(TextBox.MarginProperty, new Thickness(2));
                
                var comboBox = new FrameworkElementFactory(typeof(ComboBox));
                comboBox.SetBinding(ComboBox.SelectedItemProperty, 
                    new System.Windows.Data.Binding($"Normalizaciones[{columnaIndex}]"));
                comboBox.SetValue(ComboBox.MarginProperty, new Thickness(2));
                comboBox.SetValue(ComboBox.FontSizeProperty, 10.0);
                
                var items = new List<string> { "Sin Normalizar", "MinMax", "ZScore", "Log" };
                foreach (var item in items)
                {
                    var comboItem = new FrameworkElementFactory(typeof(ComboBoxItem));
                    comboItem.SetValue(ComboBoxItem.ContentProperty, item);
                    // Este approach no funciona directamente, necesitamos usar ItemsSource en el code-behind
                }

                stackPanel.AppendChild(textBox);
                stackPanel.AppendChild(comboBox);
                cellTemplate.VisualTree = stackPanel;
                col.CellTemplate = cellTemplate;

                DataGridPeliculas.Columns.Add(col);
            }

            // Configurar ItemsSource para los ComboBox después de crear las columnas
            DataGridPeliculas.LoadingRow += (s, e) =>
            {
                if (e.Row.Item is FilaDatos fila)
                {
                    ConfigurarComboBoxesEnFila(e.Row, fila);
                }
            };
        }

        private void ConfigurarComboBoxesEnFila(DataGridRow row, FilaDatos fila)
        {
            for (int i = 1; i < DataGridPeliculas.Columns.Count; i++)
            {
                var presenter = GetVisualChild<DataGridCellsPresenter>(row);
                if (presenter != null)
                {
                    var cell = presenter.ItemContainerGenerator.ContainerFromIndex(i) as DataGridCell;
                    if (cell != null)
                    {
                        var combo = FindVisualChild<ComboBox>(cell);
                        if (combo != null)
                        {
                            combo.ItemsSource = new List<string> { "Sin Normalizar", "MinMax", "ZScore", "Log" };
                            combo.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void BtnGenerarJSON_Click(object sender, RoutedEventArgs e)
        {
            if (datos.Count == 0)
            {
                MessageBox.Show("No hay datos para procesar", "Advertencia", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Preparar datos para clustering
                var nombres = new List<string>();
                var vectores = new List<Vector>();

                // Determinar número de columnas numéricas
                int numColumnas = datos[0].Valores.Count - 1; // Excluyendo la columna de nombre

                foreach (var fila in datos)
                {
                    // Guardar nombre (primera columna)
                    nombres.Add(fila.Valores[0]);

                    // Crear vector con valores numéricos
                    var vector = new Vector(numColumnas);
                    for (int i = 1; i < fila.Valores.Count; i++)
                    {
                        if (double.TryParse(fila.Valores[i], out double valor))
                        {
                            vector.Asignar(i - 1, valor);
                        }
                        else
                        {
                            vector.Asignar(i - 1, 0); // Valor por defecto si no es numérico
                        }
                    }
                    vectores.Add(vector);
                }

                // Normalizar según las selecciones
                for (int col = 1; col < datos[0].Valores.Count; col++)
                {
                    var normalizacion = datos[0].Normalizaciones[col];
                    if (normalizacion != "Sin Normalizar")
                    {
                        var normalizador = FactoryNormalizador.Crear(normalizacion);
                        var vectoresColumna = new List<Vector>();
                        foreach (var v in vectores)
                        {
                            var temp = new Vector(1);
                            temp.Asignar(0, v.Obtener(col - 1));
                            vectoresColumna.Add(temp);
                        }
                        var normalizados = normalizador.Normalizar(vectoresColumna);
                        for (int i = 0; i < vectores.Count; i++)
                        {
                            vectores[i].Asignar(col - 1, normalizados[i].Obtener(0));
                        }
                    }
                }

                // Calcular matriz de distancias
                var distancia = FactoryDistancia.Crear(distanciaSeleccionada);
                var matriz = GeneradorMatrizDistancia.Calcular(vectores, distancia);

                // Ejecutar clustering
                var dendrograma = Clustering.Algoritmo(matriz, nombres);

                // Exportar JSON
                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string ruta = Path.Combine(escritorio, "resultado.json");
                ExportadorJson.Exportar(dendrograma, ruta);

                MessageBox.Show($"JSON generado exitosamente en:\n{ruta}", "Éxito", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar JSON: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            datos.Clear();
            DataGridPeliculas.Columns.Clear();
            ConfigurarDataGrid();
        }

        private void BtnAgregarFila_Click(object sender, RoutedEventArgs e)
        {
            int numColumnas = DataGridPeliculas.Columns.Count;
            if (numColumnas > 0)
            {
                datos.Add(new FilaDatos(numColumnas));
            }
        }

        private void BtnEliminarFila_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridPeliculas.SelectedItem is FilaDatos filaSeleccionada)
            {
                datos.Remove(filaSeleccionada);
            }
        }

        private string[] SepararLineaCSV(string linea)
        {
            var resultado = new List<string>();
            bool dentroComillas = false;
            string valorActual = "";

            for (int i = 0; i < linea.Length; i++)
            {
                char c = linea[i];

                if (c == '"')
                {
                    dentroComillas = !dentroComillas;
                }
                else if (c == ',' && !dentroComillas)
                {
                    resultado.Add(valorActual.Trim());
                    valorActual = "";
                }
                else
                {
                    valorActual += c;
                }
            }
            resultado.Add(valorActual.Trim());

            return resultado.ToArray();
        }

        // Métodos helper para encontrar controles visuales
        private static T GetVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            T child = default(T);
            int numVisuals = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                var v = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                child = v as T ?? GetVisualChild<T>(v);
                if (child != null) break;
            }
            return child;
        }

        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
                if (child is T t) return t;
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null;
        }
    }

    // Clase para representar una fila de datos
    public class FilaDatos : INotifyPropertyChanged
    {
        public ObservableCollection<string> Valores { get; set; }
        public ObservableCollection<string> Normalizaciones { get; set; }

        public string Nombre
        {
            get => Valores.Count > 0 ? Valores[0] : "";
            set
            {
                if (Valores.Count > 0)
                {
                    Valores[0] = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        public FilaDatos(int numColumnas)
        {
            Valores = new ObservableCollection<string>();
            Normalizaciones = new ObservableCollection<string>();
            
            for (int i = 0; i < numColumnas; i++)
            {
                Valores.Add("");
                Normalizaciones.Add("Sin Normalizar");
            }
        }

        public void SetValor(int index, string valor)
        {
            if (index < Valores.Count)
            {
                Valores[index] = valor;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}