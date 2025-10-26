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
            string[] lineas = File.ReadAllLines(ruta);

            if (lineas.Length <= 1)
                throw new Exception("El archivo CSV está vacío o solo contiene encabezado.");

            List<Vector> datos = new List<Vector>();
            List<string> nombresFilas = new List<string>();

            int maxColumnas = 0;
            for (int i = 1; i < lineas.Length; i++)
            {
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

            for (int i = 1; i < lineas.Length; i++)
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
            string[] resultado = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                resultado[i] = matches[i].Groups[1].Success
                    ? matches[i].Groups[1].Value
                    : matches[i].Groups[2].Value;
            }

            return resultado;
        }
    }
}
