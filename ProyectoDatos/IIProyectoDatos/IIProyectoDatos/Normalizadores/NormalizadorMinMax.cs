namespace IIProyectoDatos
{
    public class NormalizadorMinMax : interfaceNormalizador
    {
        //Este tipo de normalizacion es para mantener datos en un rango de 0 a 1,
        //haciendo que cada caracteristica tenga una escala comparable
        public List<Vector> Normalizar(List<Vector> datos)
        {
            if (datos.Count == 0) return datos;

            int n = datos[0].longitud;
            //Arreglos para guardar el min y max de cada columna
            double[] min = new double[n];
            double[] max = new double[n];

       
            for (int i = 0; i < n; i++)
            {
                //Inicializamos extremos
                min[i] = double.MaxValue; //Se empieza con el mas grande posible
                max[i] = double.MinValue; //y el mas pequeno posible para hacer extremos
                //Esto para que el primer dato real los mueva correctamente
            }
            
            for (int j = 0; j < n; j++) //Recorremos las filas y columnas
            {
                for (int i = 0; i < datos.Count; i++)
                {
                    double v = datos[i].Obtener(j);
                    if (v < min[j]) min[j] = v; //actualizamos el min y max ya con sus datos reales
                    if (v > max[j]) max[j] = v;
                }
            }
            
            for (int i = 0; i < datos.Count; i++) //Aca se hace la escala del 0 - 1
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