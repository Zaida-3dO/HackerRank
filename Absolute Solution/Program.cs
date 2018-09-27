using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace Absolute_Solution {
    class Program {
        static int[] absolutePermutation(int n, int k) {
            HashSet<int> numbers = new HashSet<int>();
            for (int i = 1; i <= n; i++) {
                numbers.Add(i);
            }
            bool result = inputNum(new HashSet<int>(), numbers, 1, k);
            if (result) {
                return ans;
            } else {
                int[] ns = new int[1];
                ns[0] = -1;
                return ns;
            }
        }
        static int[] ans;
        static bool inputNum(HashSet<int> upToNow, HashSet<int> numbersLeft, int pos, int k) {
            bool found = false;
            bool skipFirst = false;
            bool skipSecond = false;
            if(numbersLeft.Contains(pos - k)) {
                if(numbersLeft.Contains(pos + k)) {
                    int first = pos -k;
                    int otherOption = first - k;
                    if (!numbersLeft.Contains(otherOption)) {
                        skipSecond = true;
                    }
                    int second = pos + k;
                    otherOption = second + k;
                    if (!numbersLeft.Contains(otherOption)) {
                        skipFirst = true;
                    }
                }
            }
            if (!skipFirst) {
                if (numbersLeft.Contains(pos - k)) {
                    upToNow.Add(pos - k);
                    numbersLeft.Remove(pos - k);
                    if (numbersLeft.Count == 0) {
                        ans = upToNow.ToArray();
                        return true;
                    }
                    found = inputNum(upToNow, numbersLeft, pos + 1, k);
                    if (found) {
                        return true;
                    }
                    upToNow.Remove(pos - k);
                    numbersLeft.Add(pos - k);
                }
            }
            if (!skipSecond) {
                if (numbersLeft.Contains(k + pos)) {
                    upToNow.Add(k + pos);
                    numbersLeft.Remove(k + pos);
                    if (numbersLeft.Count == 0) {
                        ans = upToNow.ToArray();
                        return true;
                    }
                    found = inputNum(upToNow, numbersLeft, pos + 1, k);
                    if (found) {
                        return true;
                    }
                    upToNow.Remove(k + pos);
                    numbersLeft.Add(k + pos);
                }
            }
            return false;
        }

        static void Main(string[] args) {
            TextWriter textWriter = new StreamWriter("C:\\ProgramData\\file.txt");

            int t = Convert.ToInt32(Console.ReadLine());

            for (int tItr = 0; tItr < t; tItr++) {
                string[] nk = Console.ReadLine().Split(' ');

                int n = Convert.ToInt32(nk[0]);

                int k = Convert.ToInt32(nk[1]);

                int[] result = absolutePermutation(n, k);

                textWriter.WriteLine(string.Join(" ", result));
            }

            textWriter.Flush();
            textWriter.Close();
        }
    }
}
