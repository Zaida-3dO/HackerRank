using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareConfig {
    class Program {
        public static void Main(string[] args) {
            long p = Convert.ToInt64(Console.ReadLine().Trim());

            Result.solve(p);
        }
    }

    class Result {

        /*
         * Complete the 'solve' function below.
         *
         * The function accepts LONG_INTEGER foo as parameter.
         */

        public static void solve(long foo) {
            // Implementation
            //List<Tuple<long, long>> answer = new List<Tuple<long, long>>();
            int count = 0;
            Dictionary<long, HashSet<long>> unsorted = new Dictionary<long, HashSet<long>>();
            for (long i = 1; i <= foo; i++) {

                long minj = i;
                long maxj = (long)Math.Ceiling((double)(foo / i));
                bool shouldCont = true;
                while (shouldCont) {
                    if (minj > maxj) {
                        break;
                    }
                    long j;
                    if (maxj - minj == 1) {
                        j = minj;
                    } else if (maxj - minj == 0) {
                        j = minj;
                    } else {
                        j = (maxj + minj) / 2;
                    }
                    //for (long j = i; j * i <= foo; j++) {
                    Tuple<bool, bool> squareRes = noOfSquares(i, j, foo);

                    if (squareRes.Item1) {
                        if (!unsorted.ContainsKey(i)) {
                            unsorted[i] = new HashSet<long>();
                        }
                        unsorted[i].Add(j);
                        //answer.Add(new Tuple<long, long>(i, j));
                        if (i != j) {
                            if (!unsorted.ContainsKey(j)) {
                                unsorted[j] = new HashSet<long>();
                            }
                            unsorted[j].Add(i);

                            //answer.Add(new Tuple<long, long>(j, i));
                        }
                        break;
                    }
                    if (maxj - minj == 1) {
                        minj = maxj;
                    } else if (maxj - minj == 0) {
                        shouldCont = false;
                    } else {
                        if (squareRes.Item2) {
                            maxj = j;
                        } else {
                            minj = j;
                        }
                    }
                }

            }
            String ans = "";


            foreach (long first in unsorted.Keys.OrderBy(x => x)) {
                foreach (long sec in unsorted[first].OrderBy(x => x)) {
                    count++;
                    ans += (first + " " + sec + "\n");
                }
            }
            Console.WriteLine(count);
            Console.WriteLine(ans);

        }
        //Equal?, Larger?
        static Tuple<bool, bool> noOfSquares(long i, long j, long total) {
            long smaller = (i < j) ? i : j;
            long count = 0;
            for (long size = 1; size <= smaller; size++) {
                long width = (i - size) + 1;
                long height = (j - size) + 1;
                count += (width * height);
                if (count > total) {
                    return new Tuple<bool, bool>(false, true);
                }
            }
            if (count == total) {
                return new Tuple<bool, bool>(true, true);
            } else {
                if (count > total) {
                    return new Tuple<bool, bool>(false, true);
                } else {
                    return new Tuple<bool, bool>(false, false);
                }
            }
        }

    }
}
