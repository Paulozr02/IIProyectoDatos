using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        private List<string> encabezados;
        private Dictionary<int, string> normalizacionPorColumna; // Guarda la normalización por columna

        public MainWindow()
        {
            InitializeComponent();
            datos = new ObservableCollection<FilaDatos>();
            encabezados = new List<string>();
            normalizacionPorColumna = new Dictionary<int, string>();
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
            encabezados = lineas[0].Split(',').Select(h => h.Trim().Trim('"')).ToList();
            
            // Limpiar datos existentes
            datos.Clear();
            DataGridPeliculas.Columns.Clear();
            normalizacionPorColumna.Clear();

            // Inicializar normalización por defecto
            for (int i = 1; i < encabezados.Count; i++)
            {
                normalizacionPorColumna[i] = "Sin Normalizar";
            }

            // Crear columnas dinámicamente
            CrearColumnas(encabezados);

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

        private void CrearColumnas(List<string> encabezados)
        {
            // Primera columna: Nombre (solo texto)
            DataGridPeliculas.Columns.Add(new DataGridTextColumn
            {
                Header = encabezados[0],
                Binding = new System.Windows.Data.Binding($"Valores[0]"),
                Width = new DataGridLength(150)
            });

            // Resto de columnas con header personalizado (valor + combobox)
            for (int i = 1; i < encabezados.Count; i++)
            {
                int columnaIndex = i;
                
                // Crear el header con ComboBox
                var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                
                var txtHeader = new TextBlock 
                { 
                    Text = encabezados[i], 
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5),
                    TextAlignment = TextAlignment.Center
                };
                
                var comboNorm = new ComboBox
                {
                    ItemsSource = new List<string> { "Sin Normalizar", "MinMax", "ZScore", "Log" },
                    SelectedIndex = 0,
                    Width = 120,
                    Tag = columnaIndex
                };
                
                comboNorm.SelectionChanged += (s, e) =>
                {
                    if (s is ComboBox cb && cb.Tag is int idx)
                    {
                        normalizacionPorColumna[idx] = cb.SelectedItem.ToString();
                    }
                };
                
                stackPanel.Children.Add(txtHeader);
                stackPanel.Children.Add(comboNorm);

                // Columna con el header personalizado
                DataGridPeliculas.Columns.Add(new DataGridTextColumn
                {
                    Header = stackPanel,
                    Binding = new System.Windows.Data.Binding($"Valores[{columnaIndex}]"),
                    Width = new DataGridLength(140)
                });
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
                // Preparar datos
                var nombres = new List<string>();
                int numColumnas = datos[0].Valores.Count - 1;
                var vectores = new List<Vector>();

                // Extraer nombres y crear vectores
                foreach (var fila in datos)
                {
                    nombres.Add(fila.Valores[0]);
                    var vector = new Vector(numColumnas);
                    
                    for (int i = 1; i < fila.Valores.Count; i++)
                    {
                        if (double.TryParse(fila.Valores[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double valor))
                        {
                            vector.Asignar(i - 1, valor);
                        }
                        else
                        {
                            vector.Asignar(i - 1, 0);
                        }
                    }
                    vectores.Add(vector);
                }

                // Normalizar columna por columna según la selección del ComboBox
                for (int col = 1; col < encabezados.Count; col++)
                {
                    if (!normalizacionPorColumna.ContainsKey(col)) continue;
                    
                    var tipoNormalizacion = normalizacionPorColumna[col];
                    
                    if (tipoNormalizacion != "Sin Normalizar")
                    {
                        // Extraer todos los valores de esta columna
                        var vectoresColumna = new List<Vector>();
                        for (int i = 0; i < vectores.Count; i++)
                        {
                            var v = new Vector(1);
                            v.Asignar(0, vectores[i].Obtener(col - 1));
                            vectoresColumna.Add(v);
                        }

                        // Normalizar la columna completa
                        var normalizador = FactoryNormalizador.Crear(tipoNormalizacion);
                        var normalizados = normalizador.Normalizar(vectoresColumna);

                        // Aplicar valores normalizados de vuelta
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
                MessageBox.Show($"Error al generar JSON:\n{ex.Message}\n\n{ex.StackTrace}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            datos.Clear();
            DataGridPeliculas.Columns.Clear();
            encabezados.Clear();
            normalizacionPorColumna.Clear();
        }

        private void BtnAgregarFila_Click(object sender, RoutedEventArgs e)
        {
            if (encabezados.Count > 0)
            {
                datos.Add(new FilaDatos(encabezados.Count));
            }
            else
            {
                MessageBox.Show("Primero carga un CSV para definir la estructura", 
                    "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnEliminarFila_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridPeliculas.SelectedItem is FilaDatos filaSeleccionada)
            {
                datos.Remove(filaSeleccionada);
            }
            else
            {
                MessageBox.Show("Selecciona una fila para eliminar", 
                    "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    }

    // Clase simplificada para representar una fila de datos
    public class FilaDatos : INotifyPropertyChanged
    {
        public ObservableCollection<string> Valores { get; set; }

        public FilaDatos(int numColumnas)
        {
            Valores = new ObservableCollection<string>();
            
            for (int i = 0; i < numColumnas; i++)
            {
                Valores.Add("");
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