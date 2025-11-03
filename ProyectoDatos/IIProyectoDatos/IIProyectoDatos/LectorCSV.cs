using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace IIProyectoDatos
{
    public class LectorCSV
    {
        public static (List<Vector> datos, List<string> nombresFilas) Leer(string ruta)
        {
            string[] lineas = File.ReadAllLines(ruta); //Lee el archivo y arma las lineas

            if (lineas.Length <= 1)
                throw new Exception("El archivo CSV está vacío o solo contiene encabezado.");

            List<Vector> datos = new List<Vector>(); //Creamos la lista vacia porque no sabemos el tamano todavia
            List<string> nombresFilas = new List<string>(); //Creamos lista con los nombre de las filas (en este caso
                                                            //es el index para luego en el cluster juntar los grupos)

            int maxColumnas = 0;
            for (int i = 1; i < lineas.Length; i++) //Este primer metodo lo que hace es detectar el numero maximo de columnas
            {                                       //Comenzamos en i = 1 para saltarnos los headers
                if (string.IsNullOrWhiteSpace(lineas[i])) continue;
                string[] celdas = SepararLinea(lineas[i]);
                int numCols = 0;

                for (int j = 1; j < celdas.Length; j++)
                {
                    if (double.TryParse(celdas[j], NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        numCols++;
                }

                if (numCols > maxColumnas)
                    maxColumnas = numCols;
            }

            for (int i = 1; i < lineas.Length; i++) //Este segundo metodo es la segunda pasada al csv, creando los vectores
                                                    //con el tamano correcto
            {
                if (string.IsNullOrWhiteSpace(lineas[i])) continue;

                string[] celdas = SepararLinea(lineas[i]);
                if (celdas.Length == 0) continue;

                nombresFilas.Add(celdas[0]); 

                Vector v = new Vector(maxColumnas);
                int pos = 0;

                for (int j = 1; j < celdas.Length && pos < maxColumnas; j++)
                {
                    if (double.TryParse(celdas[j], NumberStyles.Any, CultureInfo.InvariantCulture, out double valor))
                        v.Asignar(pos++, valor);
                }

                datos.Add(v);
            }

            return (datos, nombresFilas);
        }

        private static string[] SepararLinea(string linea)
        {
            MatchCollection matches = Regex.Matches(linea, @"(?<=^|,)(?:""([^""]*)""|([^,]*))");
            
            //MatchCollection es una coleccion de todas las coincidencias encontradas
            //Usamos regex porque ayuda a separar de mejor forma los datos en caso de tener muchos separadores
            
            string[] resultado = new string[matches.Count];
            
            /*
             * 
             * Creamos un string del tamano de cuantas celdas encontro, por ejemplo:
             * "Juan, 25, 1.75, hombre"
             * El MatchCollecion lo que hace es matches[0] = "Juan", matches[1] = "25", matches[2] = "1.75", matches[3] = "hombre"
             * Entonces creamos un vector de count 4, pues son 4 datos.
             * 
             */

            for (int i = 0; i < matches.Count; i++)
            {
                resultado[i] = matches[i].Groups[1].Success
                    ? matches[i].Groups[1].Value
                    : matches[i].Groups[2].Value;
            }

            
            
            /*
             * 
             * Esto lo que hace es recorrer el matches que son las celdas y decir que en resultado, el nuevo vector que creamos,
             * va a contener el dato en esa posicion del match, sin separador, solo el dato ya parseado, por eso se usa
             * groups y succes, porque al ingresar en esa posicion, el groups quiere decir que pueden haber datos entre comillas
             * como por ejemplo groups[0] = ""Juan"", luego hacemos succes y dara true pues hay un valor dentro, asi que con
             * value, tomamos el valor que seria "Juan", no el entrecomillado, y asi copn todos hasta recorrer todo el match del vector.
             * 
             */

            return resultado;
        }
    }
}
