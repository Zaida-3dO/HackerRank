using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProblemSolving {
    class Program {
        static int problemSolving(int k, int[] v) {
            /*
             * Write your code here.
             */
            List<int> probRate = v.ToList();
            int best = int.MaxValue;
            int noOfDays = 0;

            while (probRate.Count > 0) {
                int lastSolved = -10 - k;
                for (int problem = 0; problem < probRate.Count; problem++) {
                    if (Math.Abs(probRate[problem] - lastSolved) >= k){
                        lastSolved = probRate[problem];
                        probRate.RemoveAt(problem);
                        problem--;
                    }
                }
                noOfDays++;
                if (noOfDays > best) {
                    continue;
                }
            }
            return noOfDays;

        }

        static void Main(string[] args) {
            TextWriter textWriter = new StreamWriter("C:\\ProgramData\\file.txt");

            int t = Convert.ToInt32(Console.ReadLine());
            while (t > 0) {
                string[] nk = Console.ReadLine().Split(' ');

                int n = Convert.ToInt32(nk[0]);


                int k = Convert.ToInt32(nk[1]);

                int[] v = Array.ConvertAll(Console.ReadLine().Split(' '), vTemp => Convert.ToInt32(vTemp))
                ;
                int result = problemSolving(k, v);
                textWriter.WriteLine(result);
                t--;
            }

           

            textWriter.Flush();
            textWriter.Close();
        }
    }
}
