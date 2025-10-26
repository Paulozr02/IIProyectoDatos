namespace IIProyectoDatos;

public class NormalizadorLogaritmica : interfaceNormalizador
{
    public List<Vector> Normalizar(List<Vector> datos)
    {
        if (datos.Count == 0) return datos;
        double n = datos[0].longitud;

        for (int i = 0; i < datos.Count; i++)
        for (int j = 0; j < n; j++)
        {
            double v = datos[i].Obtener(j);
            if (v < 0) v = 0;
            datos[i].Asignar(j, Math.Log(v + 1));
        }

        return datos;
    }
}