using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitty_s_Tree
{
    class Program
    {
        static Dictionary<long, Dictionary<long, long>> distanceOfNodes;
        static void Main(String[] args) {
            long MOD_VALUE = (long)(Math.Pow(10, 9) + 7);
            int bufSize = 1024000;
            Stream inStream = Console.OpenStandardInput(bufSize);
            Console.SetIn(new StreamReader(inStream, Console.InputEncoding, false, bufSize));
            var input = Console.ReadLine().Split();
            var noOfNodes = int.Parse(input[0]);
            var noOfSets = int.Parse(input[1]);
            var treeMap = new Dictionary<long, Node>();
            distanceOfNodes = new Dictionary<long, Dictionary<long, long>>();
            for (int i = 0; i < noOfNodes - 1; i++) {
                var kk = Console.ReadLine().Split();
                NewPair(int.Parse(kk[0]), int.Parse(kk[1]), treeMap);
            }
            // foreach(int left in remaining){
            //     Console.WriteLine("Root:"+left);
            // }
            // Console.WriteLine("Step Into");
            Node root = treeMap[1];
            root.SetParent(null);
            root.SetLevel(0);


            var mySet = new List<List<int>>();
            for (int l = 0; l < noOfSets; l++) {
                int total = int.Parse(Console.ReadLine());
                var sts = Console.ReadLine().Trim().Split().ToList();
                while (total != sts.Count) {
                    sts.AddRange(Console.ReadLine().Trim().Split().ToList());
                }
                mySet.Add(sts.ConvertAll<int>(Convert.ToInt32));
            }
            //Print tree
            // foreach(Node node in treeMap.Values){
            //     Console.WriteLine(node.printIt());
            // }

            foreach (List<int> st in mySet) {
                if (st.Count < 2) {
                    Console.WriteLine(0);
                    continue;
                }
                long ans = 0;
                //List<Tuple<int, int>> s = Combination(st);

                foreach (Tuple<int, int> link in Combination(st)) {
                    long smaller = (link.Item1 < link.Item2) ? link.Item1 : link.Item2;
                    long bigger = (link.Item1 < link.Item2) ? link.Item2 : link.Item1;
                    //if (!distanceOfNodes.ContainsKey(smaller)) {
                    //    distanceOfNodes[smaller] = new Dictionary<long, long> {
                    //        [bigger] = GetDistanceOfNodes(treeMap[smaller], treeMap[bigger])
                    //    };
                    //} else {
                    //    if (!distanceOfNodes[smaller].ContainsKey(bigger)) {
                    //        distanceOfNodes[smaller][bigger] = GetDistanceOfNodes();
                    //    }
                    //}
                    if (!KnowDistance(smaller, bigger)) {
                        SetDistance(smaller, bigger, GetDistanceOfNodes(treeMap[smaller], treeMap[bigger]));
                    }
                    ans += smaller*bigger* GetDistanceOfNodes(treeMap[smaller], treeMap[bigger]);
                    Console.WriteLine("Distance btw " + smaller + " and " + bigger + " = " + GetDistanceOfNodes(treeMap[smaller], treeMap[bigger]));
                    //ICOE
                    if (ans >= MOD_VALUE) {
                        ans %= MOD_VALUE;
                    }
                }
                // ans %= MOD_VALUE;
                Console.WriteLine(ans);
            }
            Console.ReadLine();
        }

        static IEnumerable<Tuple<int, int>> Combination(List<int> nodeSet) {
            //List<Tuple<int, int>> allCombs = new List<Tuple<int, int>>();
            for (int i = 0; i < nodeSet.Count - 1; i++) {
                for (int j = i + 1; j < nodeSet.Count; j++) {
                    yield return new Tuple<int, int>(nodeSet[i], nodeSet[j]);
                }
            }
            //return allCombs;
        }

        static long GetDistanceOfNodes(Node from, Node to) {
            if (KnowDistance(from.value, to.value)) {
                return GetDistance(from.value, to.value);
            }

            Dictionary<long, Dictionary<long, long>> distanceToSubtract = new Dictionary<long, Dictionary<long, long>>();
            Node first = (from.GetLevel() < to.GetLevel()) ? to : from;//First has a higheer level, so first is lower than second
            Node second = (from.GetLevel() < to.GetLevel()) ? from : to;
            int edgeCount = 0;


            while (first.GetLevel() > second.GetLevel()) {
                first = first.GetParent();
                edgeCount++;

                if (KnowDistance(first.value, second.value)) {
                    long ans = GetDistance(first.value, second.value) + edgeCount;
                    ResolveDistances(distanceToSubtract, ans);
                    
                    return ans;
                }
                SetDistanceToSubtract(first.value, second.value, edgeCount, distanceToSubtract);

            }


            while (first != second) {
                Node temp = first;
                first = first.GetParent();
                edgeCount += 1;
                //First second
                //second parent
                //Temp second
                //First second
                if (KnowDistance(first.value, second.value)) {
                    long ans = GetDistance(first.value, second.value) + edgeCount;
                    ResolveDistances(distanceToSubtract, ans);
                    return ans;
                }
                SetDistanceToSubtract(first.value, second.value, edgeCount, distanceToSubtract);

                second = second.GetParent();

                if (KnowDistance(temp.value, second.value)) {
                    long ans = GetDistance(temp.value, second.value) + edgeCount;
                    ResolveDistances(distanceToSubtract, ans);
                    return ans;
                }
                SetDistanceToSubtract(temp.value, second.value, edgeCount, distanceToSubtract);
                edgeCount += 1;

                if (KnowDistance(first.value, second.value)) {
                    long ans = GetDistance(first.value, second.value) + edgeCount;
                    ResolveDistances(distanceToSubtract, ans);
                    return ans;
                }
                SetDistanceToSubtract(first.value, second.value, edgeCount, distanceToSubtract);

            }
            return edgeCount;
        }

        static void SetDistanceToSubtract(long a, long b, int amountToSubtract, Dictionary<long, Dictionary<long, long>> distanceToSubtract) {
            long smaller = (a < b) ? a : b;
            long bigger = (a < b) ? b : a;
            if (!distanceToSubtract.ContainsKey(smaller)) {
                distanceToSubtract[smaller] = new Dictionary<long, long> {
                    [bigger] = amountToSubtract
                };
            } else {
                distanceToSubtract[smaller][bigger] = amountToSubtract;
            }
        }

        static void ResolveDistances(Dictionary<long, Dictionary<long, long>> distanceToSubtract, long value) {
            foreach(KeyValuePair<long,Dictionary<long,long>> pairing in distanceToSubtract){
                foreach(KeyValuePair<long,long> second in pairing.Value) {
                    SetDistance(pairing.Key, second.Key, value - second.Value);
                }
            }
        }
        static bool KnowDistance(long a, long b) {
            if (a == b) {
                return true;
            }
            long smaller = (a < b) ? a : b;
            long bigger = (a < b) ? b : a;
            if (distanceOfNodes.ContainsKey(smaller)) {
                if (distanceOfNodes[smaller].ContainsKey(bigger)) {
                    return true;
                }
            }
            return false;
        }
        static long GetDistance(long a, long b) {
            if (a == b) {
                return 0;
            }
            long smaller = (a < b) ? a : b;
            long bigger = (a < b) ? b : a;
            return distanceOfNodes[smaller][bigger];
        }
        static void SetDistance(long a, long b, long distance) {
            long smaller = (a < b) ? a : b;
            long bigger = (a < b) ? b : a;
            if (!distanceOfNodes.ContainsKey(smaller)) {
                distanceOfNodes[smaller] = new Dictionary<long, long> {
                    [bigger] = distance
                };
            } else {
                distanceOfNodes[smaller][bigger] = distance;
            }
        }

        static void NewPair(int u, int v, Dictionary<long, Node> treeMap) {
            Node you;
            Node vee;
            if (!treeMap.ContainsKey(u)) {
                treeMap[u] = new Node(u);
            }
            you = treeMap[u];
            if (treeMap.ContainsKey(v)) {
                vee = treeMap[v];
                you.AddNeighbour(vee);
            } else {
                treeMap[v] = you.AddNeighbour(v);
            }
        }

        class Node
        {
            public int value;
            int level;
            List<Node> neighbours;
            Node parent;
            public Node(int value) {
                this.value = value;
                neighbours = new List<Node>();
            }

            public Node AddNeighbour(int value) {
                Node neighbour = new Node(value);
                neighbours.Add(neighbour);
                neighbour.neighbours.Add(this);
                return neighbour;
            }

            public void AddNeighbour(Node neighbour) {
                neighbours.Add(neighbour);
                neighbour.neighbours.Add(this);
            }

            public void SetParent(Node parent) {
                if (parent != null) {
                    // Console.WriteLine(value+"hasParent:"+parent.value);
                    this.parent = parent;
                    neighbours.Remove(parent);
                }
                foreach (Node neighbour in neighbours) {
                    // Console.WriteLine(value+"hasSideCock:"+neighbour.value);
                    neighbour.SetParent(this);
                }
            }

            public Node GetParent() {
                return parent;
            }

            public int GetLevel() {
                return level;
            }

            public void SetLevel(int level) {
                this.level = level;
                foreach (Node neighbour in neighbours) {
                    neighbour.SetLevel(level + 1);
                }
            }
            public String printIt() {
                return value + ": Level " + level + " noOfNeighbors:" + neighbours.Count;
            }
        }

    }
}
