using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road {
    class Program {
        class Node {
            int value;
            Dictionary<Node, int> edges;
            public Node(int value) {
                this.edges = new Dictionary<Node, int>();
                this.value = value;
            }
            public void AddEdge(Node node, int weight) {
                edges[node] = weight;
            }
            public List<Node> GetNeighbors() {
                return edges.Keys.ToList();
            }
            public int GetWeightOfEdge(Node edge) {
                return edges[edge];
            }
            public override string ToString() {
                return value + "";
            }
        }
            static Dictionary<int, int> indexPathCount = new Dictionary<int, int>();
        static Dictionary<Node, Dictionary<Node, List<int>>> youCantUnderstand = new Dictionary<Node, Dictionary<Node, List<int>>>();
        static void Main(string[] args) {
            string[] roadNodesEdges = Console.ReadLine().Split(' ');
            int roadNodes = Convert.ToInt32(roadNodesEdges[0]);
            int roadEdges = Convert.ToInt32(roadNodesEdges[1]);

            int[] roadFrom = new int[roadEdges];
            int[] roadTo = new int[roadEdges];
            int[] roadWeight = new int[roadEdges];
            Dictionary<int,Node> nodes = new Dictionary<int,Node>();
            for (int i = 0; i < roadEdges; i++) {
                string[] roadFromToWeight = Console.ReadLine().Split(' ');
                roadFrom[i] = Convert.ToInt32(roadFromToWeight[0]);
                roadTo[i] = Convert.ToInt32(roadFromToWeight[1]);
                roadWeight[i] = Convert.ToInt32(roadFromToWeight[2]);
                if (!nodes.ContainsKey(roadFrom[i])) {
                    nodes[roadFrom[i]] =  new Node(roadFrom[i]);
                }
                Node from = nodes[roadFrom[i]];
                if (!nodes.ContainsKey(roadTo[i])) {
                    nodes[roadTo[i]] = new Node(roadTo[i]);
                }
                Node to = nodes[roadTo[i]];
                from.AddEdge(to, roadWeight[i]);
                to.AddEdge(from, 1000 - roadWeight[i]);
            }

            
            
            foreach(Node n in nodes.Values) {
                GetDistance(n, n,0,n+"");
            }






            for (int d = 0; d <= 9; d++) {
                    Console.WriteLine((indexPathCount.ContainsKey(d))?indexPathCount[d]:0);
            }

        }

        static void GetDistance(Node root, Node present, int presentCost,String link) {
            foreach(Node neighbor in present.GetNeighbors()) {
                int cost = presentCost + present.GetWeightOfEdge(neighbor);
                if (HasRecorded(root, neighbor,cost)) {
                    continue;
                } else {
                    Record(root, neighbor, cost);
                        Console.WriteLine("Value: " + (cost % 10) + " Path: " + link + "->" + neighbor);
                    GetDistance(root, neighbor, cost,link+"->"+neighbor);
                }
            }
        }

        static bool HasRecorded(Node root, Node dest, int cost) {
            int lastDigit = cost%10;//TODO

            if (!youCantUnderstand.ContainsKey(root)) {
                return false;
            }
            if (!youCantUnderstand[root].ContainsKey(dest)) {
                return false;
            }
            if (!youCantUnderstand[root][dest].Contains(lastDigit)) {
                return false;
            }
            
            return true;
        }

        static void Record(Node root, Node dest, int cost) {
            int lastDigit = cost%10;//TODO
            if (!youCantUnderstand.ContainsKey(root)) {
                youCantUnderstand[root] = new Dictionary<Node, List<int>>();
            }
            if (!youCantUnderstand[root].ContainsKey(dest)) {
                youCantUnderstand[root][dest] = new List<int>();
            }
            if (youCantUnderstand[root][dest].Contains(lastDigit)) {
                Console.Write("PRRRRRROOOOOBLEEM");
            } else {
                if (root != dest) {
                    if (!indexPathCount.ContainsKey(lastDigit)) {
                        indexPathCount[lastDigit] = 0;
                    }
                    indexPathCount[lastDigit]++;
                }
            }
            youCantUnderstand[root][dest].Add(lastDigit);
        }
       
        
    }
}

