namespace IIProyectoDatos;

public class NormalizadorZScore : interfaceNormalizador
{
    public List<Vector> Normalizar(List<Vector> datos)
    {
        if (datos.Count == 0) return datos;
        int n = datos[0].longitud;
        double[] media = new double[n];
        double[] desv = new double[n];
        
        for (int j = 0; j < n; j++)
        {
            double suma = 0;
            for (int i = 0; i < datos.Count; i++)
            {
                suma += datos[i].Obtener(j);
            }
            media[j] = suma / datos.Count;
        }

        for (int j = 0; j < n; j++)
        {
            double suma = 0;
            for (int i = 0; i < datos.Count; i++)
            {
                suma += Math.Pow(datos[i].Obtener(j) - media[j], 2);
            }
            desv[j] = Math.Sqrt(suma / datos.Count);
            if (desv[j] == 0) desv[j] = 1;
            }

            for (int i = 0; i < datos.Count; i++)
                for (int j = 0; j < n; j++)
            {
                datos[i].Asignar(j, (datos[i].Obtener(j) - media[j]) / desv[j]);
            }

        return datos;
    }

}