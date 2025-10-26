using System.Text;

namespace IIProyectoDatos;

public class ExportadorJson
{
    public static void Exportar(NodoDendrograma raiz, string rutaArchivo)
    {
        string json = ConvertirNodo(raiz);
        File.WriteAllText(rutaArchivo, json);
    }

    private static string ConvertirNodo(NodoDendrograma nodo)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{"); 
        sb.Append($"\"n\": \"{nodo.nombre}\", ");
        sb.Append($"\"d\": {nodo.distancia.ToString(System.Globalization.CultureInfo.InvariantCulture)}, ");
        sb.Append("\"c\": [");

        for (int i = 0; i < nodo.hijos.Count; i++)
        {
            sb.Append(ConvertirNodo(nodo.hijos[i]));
            if (i < nodo.hijos.Count - 1)
            {
                sb.Append(", ");
            }
            
        }
        sb.Append("]}");
        return sb.ToString();
        
    }
}