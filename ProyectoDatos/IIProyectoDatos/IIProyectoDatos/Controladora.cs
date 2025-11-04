using IIProyectoDatos;
using System.IO;

public class Controladora
{
    public void EjecucionPrograma(string rutaCSV, string tipoNormalizador, string tipoDistancia)
    {
        Console.WriteLine("1. Leyendo CSV");
        var (datos, nombresFilas) = LectorCSV.Leer(rutaCSV); 

        Console.WriteLine("2. Normalizando datos");
        var normalizador = FactoryNormalizador.Crear(tipoNormalizador);
        datos = normalizador.Normalizar(datos);

        Console.WriteLine("3. Calculando matriz de distancias");
        var distancia = FactoryDistancia.Crear(tipoDistancia);
        var matriz = GeneradorMatrizDistancia.Calcular(datos, distancia);

        Console.WriteLine("4. Ejecutando clustering");
        var dendrograma = Clustering.Algoritmo(matriz, nombresFilas); 

        Console.WriteLine("5. Exportando a JSON");
        string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string ruta = Path.Combine(escritorio, "resultado.json");
        ExportadorJson.Exportar(dendrograma, ruta);

        Console.WriteLine("Proceso completo.");
    }
}