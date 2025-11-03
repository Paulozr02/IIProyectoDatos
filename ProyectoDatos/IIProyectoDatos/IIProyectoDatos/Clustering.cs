using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IIProyectoDatos
{
    public class Clustering
    {
        public static NodoDendrograma Algoritmo(Matriz matriz, List<string> nombres)
        {
            var indiceOriginal = new Dictionary<string, int>();
            //Este diccionario es porque la matriz almacena distancias segun indices numericos (index)
            //haciendo que nos permita saber para cada nombre, que indice buscar dentro de la matriz
            for (int i = 0; i < nombres.Count; i++)
                indiceOriginal[nombres[i]] = i; //tambien sirve como copia de la matriz y no perder referencia a la original

            var grupos = new List<NodoDendrograma>();
            for (int i = 0; i < nombres.Count; i++)
                grupos.Add(new NodoDendrograma(nombres[i], 0));

            while (grupos.Count > 1) //Mientras haya mas de un grupo, seguir uniendo
            {
                double menor = double.MaxValue; //menor guarda la menor distancia encontrada hasta ahora
                int a = 0, b = 1; //a y b guardaran los indices en la lista

                for (int i = 0; i < grupos.Count; i++)
                {
                    for (int j = i + 1; j < grupos.Count; j++) //doble bucle para probar todos los pares
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

                //Console.WriteLine($"Uniendo: {grupos[a].nombre} + {grupos[b].nombre} (d = {menor})");

                var nuevo = new NodoDendrograma($"({grupos[a].nombre}, {grupos[b].nombre})", menor);
                nuevo.hijos.Add(grupos[a]);
                nuevo.hijos.Add(grupos[b]);

                grupos.RemoveAt(b);
                grupos.RemoveAt(a);
                //Eliminamos ambos porque como se unio en un mismo nodo, se debe romper ese enlace
                grupos.Add(nuevo); //vamos agregando los clusters
            }

            return grupos[0]; //como al final solo queda una raiz, se retorna
        }
    }
}