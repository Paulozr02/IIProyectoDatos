namespace IIProyectoDatos;

public class NormalizadorZScore : interfaceNormalizador
{
    public List<Vector> Normalizar(List<Vector> datos)
    {
        if (datos.Count == 0) return datos;
        int n = datos[0].longitud; //Lo largo de la fila
        double[] media = new double[n]; //aca se guardara la media de cada componente, que vienen siendo las filas
        double[] desv = new double[n]; //aca se guardara la desviacion, al igual
        
        for (int j = 0; j < n; j++) //Este recorrido a la lista es para conocer la media
        {
            double suma = 0;
            for (int i = 0; i < datos.Count; i++)
            {
                suma += datos[i].Obtener(j);
            }
            media[j] = suma / datos.Count; 
            //por cada posicion, se suman todos los valores datos[i][j] y divide entre el numero de vectores
        }

        for (int j = 0; j < n; j++) //Este para la desviacion
        //Se debe entender que filas(i) son los vectores y que columnas(j) son las caraceteristicas de cada vector
        {
            double suma = 0;
            for (int i = 0; i < datos.Count; i++)
                
            {
                suma += Math.Pow(datos[i].Obtener(j) - media[j], 2);
                
                /*
            
                 creamos la variable suma, ya que hay que obtener el valor del vector i en la posicion j
                 y restarlo a la media de esa fila para luego elevarlo al cuadrado y asi obtener el de cada vector.
                 
                 */
                
            }   
            desv[j] = Math.Sqrt(suma / datos.Count); 
            //Ya aca hacemos uso de la formula donde la desviacion sera
            //esa suma de los datos entre el promedio, que serian cuantos hay
            
            if (desv[j] == 0) desv[j] = 1;
            }
            for (int i = 0; i < datos.Count; i++) //Recorremos lista
                for (int j = 0; j < n; j++)
            {
                datos[i].Asignar(j, (datos[i].Obtener(j) - media[j]) / desv[j]); //asignamos a cada dato
            }

        return datos;
    }

}