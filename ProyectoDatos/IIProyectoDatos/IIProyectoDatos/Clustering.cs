using System;
using System.Collections.Generic;

namespace IIProyectoDatos
{
    public class Clustering
    {
        public static NodoDendrograma Algoritmo(Matriz matriz, List<string> nombres)
        {
            var indiceOriginal = new Dictionary<string, int>();
            for (int i = 0; i < nombres.Count; i++)
                indiceOriginal[nombres[i]] = i;

            var grupos = new List<NodoDendrograma>();
            for (int i = 0; i < nombres.Count; i++)
                grupos.Add(new NodoDendrograma(nombres[i], 0));

            while (grupos.Count > 1)
            {
                double menor = double.MaxValue;
                int a = 0, b = 1;

                for (int i = 0; i < grupos.Count; i++)
                {
                    for (int j = i + 1; j < grupos.Count; j++)
                    {
                        var nodoI = grupos[i];
                        while (nodoI.hijos.Count > 0) nodoI = nodoI.hijos[0];
                        
                        var nodoJ = grupos[j];
                        while (nodoJ.hijos.Count > 0) nodoJ = nodoJ.hijos[0];
                        
                        double d = matriz.Obtener(indiceOriginal[nodoI.nombre], indiceOriginal[nodoJ.nombre]);
                        
                        if (d < menor)
                        {
                            menor = d;
                            a = i;
                            b = j;
                        }
                    }
                }

                Console.WriteLine($"Uniendo: {grupos[a].nombre} + {grupos[b].nombre} (d = {menor})");

                var nuevo = new NodoDendrograma($"({grupos[a].nombre}, {grupos[b].nombre})", menor);
                nuevo.hijos.Add(grupos[a]);
                nuevo.hijos.Add(grupos[b]);

                grupos.RemoveAt(b);
                grupos.RemoveAt(a);
                grupos.Add(nuevo);
            }

            return grupos[0];
        }
    }
}