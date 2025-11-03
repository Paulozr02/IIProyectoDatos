using System.Runtime.Intrinsics;

namespace IIProyectoDatos;

public class  DistanciaManhattan : InterfaceDistancia
{
    public double CalcularDistancia(Vector vector1, Vector vector2)
    {
        double suma = 0;
        for (int i = 0; i < vector1.longitud; i++) //Recorremos los componentes 
        {
            suma += Math.Abs(vector1.Obtener(i) - vector2.Obtener(i));
            
            /*
             
             Se usa abs porque se encarga segun enunciado de proyecto, de asegurar que devuelva un valor
             que no sea negativo, asi que devuelve un valor absoluto.
             Ademas, suma acumula el valor absoluto de la resta entre las componentes correspondientes 
             de ambos vectores. La idea es que tras recorrer toda la longitud, devolvamos suma,
            que es la distancia entre los dos vectores
            
            */
            
        }

        return suma;
    }
}