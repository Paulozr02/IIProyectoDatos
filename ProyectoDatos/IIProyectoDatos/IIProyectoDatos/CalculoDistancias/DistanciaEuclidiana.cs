namespace IIProyectoDatos;

public class DistanciaEuclidiana : InterfaceDistancia
{
    public double CalcularDistancia(Vector vector1, Vector vector2)
    {
        double suma = 0;
        for (int i = 0; i < vector1.longitud; i++)
        {
            double d = vector1.Obtener(i) - vector2.Obtener(i);
            suma += d * d;
        }
        return Math.Sqrt(suma);
    }
    
}