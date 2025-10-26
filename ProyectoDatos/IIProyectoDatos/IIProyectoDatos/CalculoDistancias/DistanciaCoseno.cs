using System.Diagnostics.CodeAnalysis;

namespace IIProyectoDatos;

public class DistanciaCoseno : InterfaceDistancia
{
    public double CalcularDistancia(Vector vector1, Vector vector2)
    {
        double resultado = 0, magVec1 = 0, magVec2 = 0;
        for (int i = 0; i < vector1.longitud; i++)
        {
            
            double powVec1 = vector1.Obtener(i) *vector1.Obtener(i);
            double powVec2 = vector2.Obtener(i) * vector2.Obtener(i);
            
            magVec1 += powVec1;
            magVec2 += powVec2;
            
            resultado += vector1.Obtener(i) *  vector2.Obtener(i);
           
        }
        magVec1 = Math.Sqrt(magVec1);
        magVec2 = Math.Sqrt(magVec2);
        resultado = resultado / (magVec1 * magVec2);

        return 1 - resultado;
    }
}