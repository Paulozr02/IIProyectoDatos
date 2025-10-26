using IIProyectoDatos;

public class Controladora
{
    public void EjecucionPrograma(string rutaCSV, string tipoNormalizador, string tipoDistancia)
    {
        Console.WriteLine("Leyendo CSV");
        var (datos, nombresFilas) = LectorCSV.Leer(rutaCSV); 

        Console.WriteLine("Normalizando datos");
        var normalizador = FactoryNormalizador.Crear(tipoNormalizador);
        datos = normalizador.Normalizar(datos);

        Console.WriteLine("Calculando matriz de distancias");
        var distancia = FactoryDistancia.Crear(tipoDistancia);
        var matriz = GeneradorMatrizDistancia.Calcular(datos, distancia);

        Console.WriteLine("Ejecutando clustering");
        var dendrograma = Clustering.Algoritmo(matriz, nombresFilas); 

        Console.WriteLine("Exportando a JSON");
        string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string ruta = Path.Combine(escritorio, "resultado.json");
        ExportadorJson.Exportar(dendrograma, ruta);

        Console.WriteLine("Proceso completo.");
    }
}