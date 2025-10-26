namespace IIProyectoDatos;

public class DistanciaHamming : InterfaceDistancia
{
    public double CalcularDistancia(Vector vector1, Vector vector2)
    {
        double diferente = 0;
        for (int i = 0; i < vector1.longitud; i++)
        {
            if (vector1.Obtener(i) != vector2.Obtener(i))
                diferente++;
        }

        return diferente;
    }
}