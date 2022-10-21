using System;
using System.Collections.Generic;

namespace TI4.Raw
{
    public class Edge
    {
        private int origem;
        private int destino;
        private int peso;

        public Edge()
        {
            this.origem = -1;
            destino = -1;
            peso = 0;
        }
        public Edge(int orig, int dest, int peso)
        {
            this.origem = orig;
            this.destino = dest;
            this.peso = peso;
        }
        public int getOrig()
        {
            return this.origem;
        }
        public void setOrig(int orig)
        {
            this.origem = orig;
        }
        public int getDest()
        {
            return this.destino;
        }
        public void setDest(int dest)
        {
            this.destino = dest;
        }
        public int getPeso()
        {
            return this.peso;
        }
        public void setPeso(int peso)
        {
            this.peso = peso;
        }
        public void printClass()
        {
            UnityEngine.Debug.Log("\nOrigem: " + origem + "\tDestino: " + destino + "\tPeso: " + peso);
        }
    }
    public class Relations
    {
        private int n_vertices;
        private int n_relacoes;

        private List<Edge> lista_arestas;

        public Relations()
        {
            n_vertices = 0;
            n_relacoes = 0;
            lista_arestas = new List<Edge>();
        }
        public Relations(int numVert, int numArest)
        {
            n_vertices = numVert;
            n_relacoes = numArest;
            lista_arestas = new List<Edge>();
        }
        public int get_n_relacoes()
        {
            return this.n_relacoes;
        }
        public int get_n_vertices()
        {
            return this.n_vertices;
        }
        public List<Edge> get_lista_arestas()
        {
            return this.lista_arestas;
        }
        public void showRelations()
        {
            foreach (Edge ares in lista_arestas)
            {
                ares.printClass();
            }
        }
        public void newRelation(int orig, int dest, int peso)
        {
            Edge aresta = new Edge(orig, dest, peso);
            aresta.printClass();
            lista_arestas.Add(new Edge(orig, dest, peso));
        }
    }

    // Class that creates a lista of relations (graph), based on the info on grafoMapa.txt
    public class GraphInterpreter
    {
        public Relations mapa;
        public GraphInterpreter(string rawData)
        {
            mapa = readClass(rawData);
        }
        public void showGraph()
        {
            mapa.showRelations();
        }

        // Method that reads the txt file and generates a graph - implemented through a list
        public Relations readClass(string rawData)
        {
            string infoGrafoCompleta;
            int[] infos_grafo = new int[2];
            int[] orig_dest = new int[3];

            string[] rawDataLines = rawData.Split("\n");
            infoGrafoCompleta = rawDataLines[0];
            string content = infoGrafoCompleta;
            infos_grafo = cleanLine(infoGrafoCompleta, 2);
            Relations mapa = new Relations(infos_grafo[0], infos_grafo[1]);

            for (int i = 1; content != null && i <= mapa.get_n_relacoes(); i++)
            {
                content = rawDataLines[i];
                orig_dest = cleanLine(content, 3);
                mapa.newRelation(orig_dest[0], orig_dest[1], orig_dest[2]);
            }
            return mapa;
        }

        // Class that cleans the line, read from the file
        public static int[] cleanLine(String linha, int num_infos)
        {
            string[] orig_destino_peso;
            string[] dest_peso;
            int[] orig_destino_peso_int = new int[3];

            if (num_infos == 3)
            {
                linha = linha.Trim();
                orig_destino_peso = linha.Split(";");
                dest_peso = orig_destino_peso[1].Split(" ");

                orig_destino_peso_int[0] = int.Parse(orig_destino_peso[0]);
                orig_destino_peso_int[1] = int.Parse(dest_peso[0]);

                orig_destino_peso_int[2] = int.Parse(dest_peso[dest_peso.Length - 1]);
            }

            else if (num_infos == 2)
            {
                orig_destino_peso = linha.Split(" ");
                int.TryParse(orig_destino_peso[0], out orig_destino_peso_int[0]);
                orig_destino_peso_int[1] = int.Parse(orig_destino_peso[orig_destino_peso.Length - 1]);
            }

            return orig_destino_peso_int;
        }
    }
}

