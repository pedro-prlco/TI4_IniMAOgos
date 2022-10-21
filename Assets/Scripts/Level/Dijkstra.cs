using System;
using System.Collections.Generic;

namespace TI4.Raw
{
    public class GFG
    {
        static int V = 40;
        int minDistance(int[] dist, bool[] foundShortestPath)
        {
            // Initialize min value
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < V; v++)
                if (foundShortestPath[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }
        void printSolution(int[] dist, int n, int origem, int destino)
        {
            Console.Write("Vertex	 Distance " + "from Source\n");
            for (int i = 0; i < V; i++)
            {
                if (i == destino)
                {
                    Console.Write(i + " \t\t " + dist[i] + "\n");
                }
            }

        }

        List<int> dijkstra(int[,] graph, int origem, int destino)
        {
            origem--;
            destino--;

            int[] dist = new int[V]; // The output array. dist[i] will hold the shortest distance from origem to i

            bool[] foundShortestPath = new bool[V];

            Edge[] verticesUntilOrigin = new Edge[V];

            // Initialize all distances as INFINITE and stpSet[] as false
            for (int i = 0; i < V; i++)
            {
                dist[i] = int.MaxValue;
                foundShortestPath[i] = false;
            }

            dist[origem] = 0;

            // Find shortest path for all vertices
            for (int count = 0; count < V - 1; count++)
            {
                int u = minDistance(dist, foundShortestPath);

                foundShortestPath[u] = true;

                // Update dist value of the adjacent vertices of the picked vertex.
                for (int v = 0; v < V; v++)
                {
                    if (!foundShortestPath[v] && graph[u, v] != 0 && dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
                    {
                        dist[v] = dist[u] + graph[u, v];
                    }
                }

            }

            return getMinVertPath(dist, origem, destino, graph);
        }


        List<int> getMinVertPath(int[] dist, int origem, int destino, int[,] graph)
        {
            int ultimoVert = destino;
            int proxVert = destino;
            int min = int.MaxValue;
            List<int> vetPath = new List<int>();
            vetPath.Add(destino);

            while (dist[ultimoVert] != graph[origem, ultimoVert])
            {
                for (int i = 0; i < V; i++)
                {
                    if (graph[ultimoVert, i] != 0 && (graph[ultimoVert, i] + dist[i] < min))
                    {
                        min = graph[ultimoVert, i] + dist[i];
                        proxVert = i;
                    }
                }
                vetPath.Add(proxVert);
                ultimoVert = proxVert;
            }

            vetPath.Add(origem);

            // for (int i = 0; i < vetPath.Count; i++)
            // {
            //     UnityEngine.Debug.Log(vetPath[i]);
            // }

            return vetPath;
        }

		static GFG data;
		static GraphInterpreter grafo;
		static int[,] grafoMatrix;

        // Driver Code
        public static void Setup(string rawData)
        {
            List<Edge> lista_arestas = new List<Edge>();
            grafo = new GraphInterpreter(rawData);

            V = int.Parse(rawData.Split("\n")[0].Split(" ")[0]);

            grafoMatrix = new int[V, V];

            int n_relacoes = grafo.mapa.get_n_relacoes();
            int peso = 0, destino = 0, origem = 0;
            lista_arestas = grafo.mapa.get_lista_arestas();

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    grafoMatrix[i, j] = 0;
                }
            }

            foreach (Edge ares in lista_arestas)
            {
                origem = ares.getOrig() - 1;
                destino = ares.getDest() - 1;
                peso = ares.getPeso();

                grafoMatrix[origem, destino] = peso;
                grafoMatrix[destino, origem] = peso;
            }

            /* for(int i=1; i<V; i++) {
                for(int j=1; j<V; j++) {
                    Console.Write(graph[i,j]+" ");
                }
                Console.Write("\n");
            } */

            data = new GFG();
            // data.dijkstra(graph, 1, 11);
        }

		public static List<int> GetShortestPath(int from, int to)
		{
			if(data == null)
			{
				UnityEngine.Debug.Log("Graph Data is not initialized yet");
				return new List<int> { from };
			}
			return data.dijkstra(grafoMatrix, from, to);
		}
    }
}
