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
        //Este metodo lo que hace es recorrer componente a componente los datos de ambos vectores y si
        //algun dato no coincide, se acumula en diferente.

        return diferente; //Se devuelven cuantos componentes luego de analizar ambos vectores, son disntintos
    }
}