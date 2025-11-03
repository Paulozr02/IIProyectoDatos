namespace IIProyectoDatos;

public class DistanciaEuclidiana : InterfaceDistancia
{
    public double CalcularDistancia(Vector vector1, Vector vector2)
    {
        double suma = 0;
        for (int i = 0; i < vector1.longitud; i++) //Recorremos lo largo de la fila
        {
            double d = vector1.Obtener(i) - vector2.Obtener(i); //Restamos el mismo componente (caracteristica)
            suma += d * d; //suma tendra d que es la resta multiplicado por si mismo, porque es al cuadrado
        }
        return Math.Sqrt(suma); //devolvemos la raiz de esa suma
    }
    
}