namespace IIProyectoDatos;

public class GeneradorMatrizDistancia
{
    public static Matriz Calcular(List<Vector> datos, InterfaceDistancia distancia)
    //Aca creamos la matriz ya actualizada con su tipo de distancia y normalizacion
    {
        int n = datos.Count; //tamano de la matriz
        var matriz = new Matriz(n, n); //creamos la matriz nueva para no trabajar en la original

        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                double dis = (i == j) ? 0 : distancia.CalcularDistancia(datos[i], datos[j]);
                //Si el dato es igual a 0 entre las filas, su distancia sera 0,
                //mientras que si es distinto, calculamos segun el tipo, su distancia
                matriz.Asignar(i, j, dis); //asignamos valores en ambos
                matriz.Asignar(j, i, dis);
            }
        }

        return matriz;

    }
}