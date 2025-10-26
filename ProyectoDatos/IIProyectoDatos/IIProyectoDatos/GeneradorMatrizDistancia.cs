namespace IIProyectoDatos;

public class GeneradorMatrizDistancia
{
    public static Matriz Calcular(List<Vector> datos, InterfaceDistancia distancia)
    {
        int n = datos.Count;
        var matriz = new Matriz(n, n);

        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                double dis = (i == j) ? 0 : distancia.CalcularDistancia(datos[i], datos[j]);
                matriz.Asignar(i, j, dis);
                matriz.Asignar(j, i, dis);
            }
        }

        return matriz;

    }
}