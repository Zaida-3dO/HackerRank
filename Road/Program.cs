using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Road {
    class Program {
        class Node {
            Dictionary<Node, int> edges;
            Dictionary<Node, HashSet<int>> distances;
            public Node() {
                edges = new Dictionary<Node, int>();
                distances = new Dictionary<Node, HashSet<int>>();
            }
            public void AddEdge(Node node, int weight) {
                edges[node] = weight;
            }
            public IEnumerable<Node> GetNeighbors() {
                return edges.Keys;
            }
            public int GetWeightOfEdge(Node edge) {
                return edges[edge];
            }
            public bool HasDistance(Node node) {
                return distances.ContainsKey(node);
            }
            public IEnumerable<Node> GetDistances() {
                return distances.Keys;
            }
            public HashSet<int> GetDistanceToNode(Node node) {
                return distances[node];
            }
            public void AddDistance(Node dest, int weight) {
                if (!distances.ContainsKey(dest)) {
                    distances[dest] = new HashSet<int>();
                }
                distances[dest].Add(weight);
            }
        }
        static Dictionary<int, int> indexPathCount = new Dictionary<int, int>();
        static Dictionary<Node, Dictionary<Node, HashSet<int>>> youCantUnderstand = new Dictionary<Node, Dictionary<Node, HashSet<int>>>();
        static List<Tuple<Node, long>> queue = new List<Tuple<Node, long>>();
        static void Main(string[] args) {
            string[] roadNodesEdges = Console.ReadLine().Split(' ');
            int roadNodes = Convert.ToInt32(roadNodesEdges[0]);
            int roadEdges = Convert.ToInt32(roadNodesEdges[1]);

            int[] roadFrom = new int[roadEdges];
            int[] roadTo = new int[roadEdges];
            int[] roadWeight = new int[roadEdges];
            Dictionary<int, Node> nodes = new Dictionary<int, Node>();
            for (int i = 0; i < roadEdges; i++) {
                string[] roadFromToWeight = Console.ReadLine().Split(' ');
                roadFrom[i] = Convert.ToInt32(roadFromToWeight[0]);
                roadTo[i] = Convert.ToInt32(roadFromToWeight[1]);
                roadWeight[i] = Convert.ToInt32(roadFromToWeight[2]);
                if (!nodes.ContainsKey(roadFrom[i])) {
                    nodes[roadFrom[i]] = new Node();
                }
                Node from = nodes[roadFrom[i]];
                if (!nodes.ContainsKey(roadTo[i])) {
                    nodes[roadTo[i]] = new Node();
                }
                Node to = nodes[roadTo[i]];
                from.AddEdge(to, roadWeight[i]);
                to.AddEdge(from, 1000 - roadWeight[i]);
            }



            foreach (Node n in nodes.Values) {
                GetDistance(n, n, 0);
            }






            for (int d = 0; d <= 9; d++) {
                Console.WriteLine((indexPathCount.ContainsKey(d)) ? indexPathCount[d] : 0);
            }

        }

        static void GetDistance(Node root, Node present, long presentCost) {
            foreach (Node neighbor in present.GetNeighbors()) {
                long cost = presentCost + present.GetWeightOfEdge(neighbor);
              
                if (HasRecorded(root, neighbor, cost)) {
                    continue;
                } else {
                    bool recurse = root.HasDistance(neighbor);
                    Record(root, neighbor, cost);
                    queue.Add(new Tuple<Node, long>(neighbor, cost));
                    if (recurse) {
                        UpdateDistances(root, neighbor, cost);
                    }
                    
                }
            }
            if (queue.Count != 0) {
                Tuple<Node, long> next = queue[0];
                queue.RemoveAt(0);
                GetDistance(root, next.Item1, next.Item2);
            }

        }

        static void UpdateDistances(Node root, Node present, long presentCost) {
          
            foreach(Node node in present.GetDistances()) {
                foreach (int dist in present.GetDistanceToNode(node)) {
                    if (HasRecorded(root, node, presentCost + dist)) {
                        continue;
                    } else {
                        bool recurse = root.HasDistance(node);
                        Record(root, node, presentCost + dist);
                        if (recurse) {
                            UpdateDistances(root, node, presentCost + dist);
                        }
                       
                    }
                }
            }
        }

        static bool HasRecorded(Node root, Node dest, long cost) {
            int lastDigit = (int)cost % 10;
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

        static void Record(Node root, Node dest, long cost) {
            int lastDigit = (int)cost % 10;
            if (!youCantUnderstand.ContainsKey(root)) {
                youCantUnderstand[root] = new Dictionary<Node, HashSet<int>>();
            }
            if (!youCantUnderstand[root].ContainsKey(dest)) {
                youCantUnderstand[root][dest] = new HashSet<int>();
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
            root.AddDistance(dest, lastDigit);
        }
    }
}

