namespace IIProyectoDatos;

public class NormalizadorLogaritmica : interfaceNormalizador
{
    public List<Vector> Normalizar(List<Vector> datos)
    {
        if (datos.Count == 0) return datos;
        double n = datos[0].longitud; 

        for (int i = 0; i < datos.Count; i++) //Recorremos la lista con el .count, cuantas veces sean necesarias
        for (int j = 0; j < n; j++) //Aca el j < n porque vamos a ir a lo largo, normalizando todos los datos de esa fila
        {
            double v = datos[i].Obtener(j); //obtenemos valor en esa posicion
            if (v < 0) v = 0; 
            datos[i].Asignar(j, Math.Log(v + 1)); //asignamos la respectiva formula
        }

        return datos;
    }
}