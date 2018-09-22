using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitty_s_Tree
{
    class Program
    {
        static void Main(String[] args)
        {
            int bufSize = 1024000;
            Stream inStream = Console.OpenStandardInput(bufSize);
            Console.SetIn(new StreamReader(inStream, Console.InputEncoding, false, bufSize));

            var input = Console.ReadLine().Split();
            var noOfNodes = int.Parse(input[0]);
            var noOfSets = int.Parse(input[1]);
            var treeMap = new Dictionary<int, Node>();
            //         var remaining = new List<int>();

            //             for (int i= 1; i <= noOfNodes;i++){
            //                 remaining.Add(i);
            //             }

            for (int i = 0; i < noOfNodes - 1; i++)
            {
                var kk = Console.ReadLine().Split();
                // remaining.Remove(int.Parse(kk[1]));
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
            for (int l = 0; l < noOfSets; l++)
            {
                int total = int.Parse(Console.ReadLine());
                var sts = Console.ReadLine().Trim().Split().ToList();
                while (total != sts.Count)
                {
                    sts.AddRange(Console.ReadLine().Trim().Split().ToList());
                }
                mySet.Add(sts.ConvertAll<int>(Convert.ToInt32));
            }

            //Print tree
            // foreach(Node node in treeMap.Values){
            //     Console.WriteLine(node.printIt());
            // }

            foreach (List<int> st in mySet)
            {
                if (st.Count < 2)
                {
                    Console.WriteLine(0);
                    continue;
                }
                double ans = 0;
                //List<Tuple<int, int>> s = Combination(st);

                foreach (Tuple<int, int> link in Combination(st))
                {
                    ans += link.Item1 * link.Item2 * GetDistance(treeMap[link.Item1], treeMap[link.Item2]);
                }
                double mod = ans % (Math.Pow(10, 9) + 7);
                Console.WriteLine(mod);
            }

        }

        static IEnumerable<Tuple<int, int>> Combination(List<int> nodeSet)
        {
            //List<Tuple<int, int>> allCombs = new List<Tuple<int, int>>();
            for (int i = 0; i < nodeSet.Count - 1; i++)
            {
                for (int j = i + 1; j < nodeSet.Count; j++)
                {
                    yield return new Tuple<int, int>(nodeSet[i], nodeSet[j]);
                }
            }
            //return allCombs;
        }

        static int GetDistance(Node from, Node to)
        {
            Node first = (from.GetLevel() < to.GetLevel()) ? to : from;
            Node second = (from.GetLevel() < to.GetLevel()) ? from : to;
            int edgeCount = 0;
            // Console.WriteLine("JJ"+first.printIt()+" "+second.printIt());
            while (first.GetLevel() > second.GetLevel())
            {

                first = first.GetParent();
                // Console.WriteLine("KK"+first.printIt()+" "+second.printIt());
                edgeCount++;
            }
            // Console.WriteLine("''''''''''''''''");
            while (first != second)
            {
                // Console.WriteLine(first.printIt()+" "+second.printIt());
                first = first.GetParent();
                second = second.GetParent();
                edgeCount += 2;
            }
            return edgeCount;
        }

        static void NewPair(int u, int v, Dictionary<int, Node> treeMap)
        {
            Node you;
            Node vee;
            if (!treeMap.ContainsKey(u))
            {
                treeMap[u] = new Node(u);
            }
            you = treeMap[u];
            if (treeMap.ContainsKey(v))
            {
                vee = treeMap[v];
                you.AddNeighbour(vee);
            }
            else
            {
                treeMap[v] = you.AddNeighbour(v);
            }
        }

        class Node
        {
            public int value;
            int level;
            List<Node> neighbours;
            Node parent;
            public Node(int value)
            {
                this.value = value;
                neighbours = new List<Node>();
            }

            public Node AddNeighbour(int value)
            {
                Node neighbour = new Node(value);
                neighbours.Add(neighbour);
                neighbour.neighbours.Add(this);
                return neighbour;
            }

            public void AddNeighbour(Node neighbour)
            {
                neighbours.Add(neighbour);
                neighbour.neighbours.Add(this);
            }

            public void SetParent(Node parent)
            {
                if (parent != null)
                {
                    // Console.WriteLine(value+"hasParent:"+parent.value);
                    this.parent = parent;
                    neighbours.Remove(parent);
                }
                foreach (Node neighbour in neighbours)
                {
                    // Console.WriteLine(value+"hasSideCock:"+neighbour.value);
                    neighbour.SetParent(this);
                }
            }

            public Node GetParent()
            {
                return parent;
            }

            public int GetLevel()
            {
                return level;
            }

            public void SetLevel(int level)
            {
                this.level = level;
                foreach (Node neighbour in neighbours)
                {
                    neighbour.SetLevel(level + 1);
                }
            }
            public String printIt()
            {
                return value + ": Level " + level + " noOfNeighbors:" + neighbours.Count;
            }
        }

    }
}
