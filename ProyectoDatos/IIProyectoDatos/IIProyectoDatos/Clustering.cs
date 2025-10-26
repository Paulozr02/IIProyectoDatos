using System;
using System.Collections.Generic;

namespace IIProyectoDatos
{
    public class Clustering
    {
        public static NodoDendrograma Algoritmo(Matriz matriz, List<string> nombres)
        {
            // 🔹 Mapa nombre -> índice original (posición en la matriz)
            var indiceOriginal = new Dictionary<string, int>();
            for (int i = 0; i < nombres.Count; i++)
                indiceOriginal[nombres[i]] = i;

            // 🔹 Crear un nodo por cada elemento (película)
            var grupos = new List<NodoDendrograma>();
            for (int i = 0; i < nombres.Count; i++)
                grupos.Add(new NodoDendrograma(nombres[i], 0));

            // 🔹 Bucle principal: combinar hasta que quede un grupo
            while (grupos.Count > 1)
            {
                double menor = double.MaxValue;
                int a = 0, b = 1;

                // Buscar el par más cercano usando SIEMPRE la matriz original
                for (int i = 0; i < grupos.Count; i++)
                {
                    for (int j = i + 1; j < grupos.Count; j++)
                    {
                        int ia = indiceOriginal[ObtenerNombreBase(grupos[i])];
                        int ib = indiceOriginal[ObtenerNombreBase(grupos[j])];
                        double d = matriz.Obtener(ia, ib);
                        if (d < menor)
                        {
                            menor = d;
                            a = i;
                            b = j;
                        }
                    }
                }

                // 🔹 Mostrar pares unidos (debug opcional)
                Console.WriteLine($"Uniendo: {grupos[a].nombre} + {grupos[b].nombre} (d = {menor})");

                // Crear un nuevo nodo combinando los dos más cercanos
                string nombreNuevo = "(" + grupos[a].nombre + ", " + grupos[b].nombre + ")";
                var nuevo = new NodoDendrograma(nombreNuevo, menor);
                nuevo.hijos.Add(grupos[a]);
                nuevo.hijos.Add(grupos[b]);

                if (a < b)
                {
                    grupos.RemoveAt(b);
                    grupos.RemoveAt(a);
                }
                else
                {
                    grupos.RemoveAt(a);
                    grupos.RemoveAt(b);
                }
                grupos.Add(nuevo);
            }

            return grupos[0];
        }

    
        private static string ObtenerNombreBase(NodoDendrograma n)
        {
            while (n.hijos.Count > 0)
                n = n.hijos[0];
            return n.nombre;
        }
    }
}
