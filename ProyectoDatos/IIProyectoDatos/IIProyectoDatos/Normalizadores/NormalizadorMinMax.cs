namespace IIProyectoDatos
{
    public class NormalizadorMinMax : interfaceNormalizador
    {
        public List<Vector> Normalizar(List<Vector> datos)
        {
            if (datos.Count == 0) return datos;

            int n = datos[0].longitud;
            double[] min = new double[n];
            double[] max = new double[n];

       
            for (int i = 0; i < n; i++)
            {
                min[i] = double.MaxValue;
                max[i] = double.MinValue; 
            }
            
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < datos.Count; i++)
                {
                    double v = datos[i].Obtener(j);
                    if (v < min[j]) min[j] = v;
                    if (v > max[j]) max[j] = v;
                }
            }
            
            for (int i = 0; i < datos.Count; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double v = datos[i].Obtener(j);
                    double rango = max[j] - min[j];
                    double normalizado = (rango == 0) ? 0 : (v - min[j]) / rango;
                    datos[i].Asignar(j, normalizado);
                }
            }

            return datos;
        }
    }
}