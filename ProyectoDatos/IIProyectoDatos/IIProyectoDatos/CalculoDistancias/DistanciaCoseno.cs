using System.Diagnostics.CodeAnalysis;

namespace IIProyectoDatos;

public class DistanciaCoseno : InterfaceDistancia
{
    public double CalcularDistancia(Vector vector1, Vector vector2)
    {
        //Creamos 3 variables que van a ser el resultado, magnitud del primer vector y la del segundo
        double resultado = 0, magVec1 = 0, magVec2 = 0; 
        for (int i = 0; i < vector1.longitud; i++) //Recorremos el vector
        {
            
            double powVec1 = vector1.Obtener(i) * vector1.Obtener(i);
            double powVec2 = vector2.Obtener(i) * vector2.Obtener(i);
            
            //Ambos contienen el numero por 2, ya que es mas rapido que hacer el math.pow y elevarlo
            magVec1 += powVec1;
            magVec2 += powVec2;
            
            resultado += vector1.Obtener(i) *  vector2.Obtener(i);
            //aca acumulamos en resultado el producto punto
           
        }
        magVec1 = Math.Sqrt(magVec1);
        magVec2 = Math.Sqrt(magVec2);
        resultado = resultado / (magVec1 * magVec2);
        //por formula, el resultado debe ser el producto punto que es resultado, entre la
        //magnitud del dato en el vector 1 por la magnitud del dato del vector 2

        return 1 - resultado;
    }
}