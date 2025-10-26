using System.Runtime.Intrinsics;

namespace IIProyectoDatos;

public class DistanciaManhattan : InterfaceDistancia
{
    public double CalcularDistancia(Vector vector1, Vector vector2)
    {
        double suma = 0;
        for (int i = 0; i < vector1.longitud; i++)
        {
            suma += Math.Abs(vector1.Obtener(i) - vector2.Obtener(i));
        }

        return suma;
    }
}