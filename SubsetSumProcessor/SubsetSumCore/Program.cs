using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SubsetSumCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] inputs = { -5, -2, -1, 3, 7 };
            //decimal[] inputs = { -5.10m, -2.01m, -1.00m, 3.00m, 7.11m };
            decimal[] inputs = { 994.04m, -55400.81m, -6543.6m, -1228.06m, 4833.2m, -8613.36m, -3189.17m, 5697.23m, -8962.98m, -1670.83m, 6763.97m, -3941.09m, 5214.52m, -1206.61m, -8979.49m, 6127.02m, -5797.81m, 9774.32m, 929.25m, -9431.31m, 2189.93m, -4977.58m, -278.64m };

            Stopwatch timer = new Stopwatch();
            timer.Start();

            var subset = FindDynamic(inputs);
            var isSubset = subset != null;

            timer.Stop();
            var runTime = timer.Elapsed;         

            Console.WriteLine($"Match: {isSubset} in {runTime.Seconds.ToString()}secs {runTime.Milliseconds}ms");
            Console.Write("[");
            for(int i = 0; i < subset.Length; i ++)
            {
                Console.Write(subset[i].ToString());
                if (i + 1 != subset.Length)
                    Console.Write(",");
            }
            Console.Write("]");
            Console.ReadKey();
        }

        private static decimal[] FindDynamic(decimal[] doubles)
        {
            int[] inputs = new int[doubles.Length];
            try
            {
                for (int i = 0; i < doubles.Length; i++)
                {
                    if (Math.Round(doubles[i], 2) != doubles[i])
                        throw new InvalidCastException("Inputs must be 2 decimal places");
                    inputs[i] = (int)(doubles[i] * 100);
                }

                int[] subset = FindDynamic(inputs);
                decimal[] subsetDecimal = null;
                
                if(subset != null && subset.Length > 0)
                {
                    subsetDecimal = new decimal[subset.Length];

                    for (int i = 0; i < subset.Length; i++)
                    {
                        subsetDecimal[i] = (decimal)subset[i] / 100;
                    }
                }

                return subsetDecimal;
            }
            catch(InvalidCastException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int[] FindDynamic(int[] inputs)
        {
            int a = 0;
            int b = 0;

            for(int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] > 0)
                    b += inputs[i];
                else
                    a += inputs[i];
            }

            int s = (b - a) + 1;

            bool[,] matrix = new bool[inputs.Length, s];

            int findIndex = FillMatrix(inputs, matrix, s, a);
            //int findIndex = FillMatrixParallel(inputs, matrix, s, a);

            bool isSubset = matrix[findIndex, -a];
            List<int> subset;
            int[] subsetArray = null;
            if (isSubset)
            {
                subset = GetSubset(inputs, matrix, a, findIndex);
                if(subset != null && subset.Count > 0)
                {
                    subsetArray = subset.ToArray();
                }
            }
            return subsetArray;
        }

        private static int FillMatrix(int[] inputs, bool[,] matrix, int s, int a)
        {
            int findIndex = inputs.Length - 1;
            matrix[0, inputs[0] - a] = true;

            for (int i = 1; i < inputs.Length; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    int check = j - inputs[i];
                    if (s - 1 >= check && check >= 0)
                    {
                        if (matrix[i - 1, j] || (j + a) == inputs[i] || matrix[i - 1, check])
                            matrix[i, j] = true;
                    }
                    else
                    {
                        if (matrix[i - 1, j] || (j + a) == inputs[i])
                            matrix[i, j] = true;
                    }
                }
                if (matrix[i, -a])
                {
                    findIndex = i;
                    break;
                }
            }
            return findIndex;
        }

        private static int FillMatrixParallel(int[] inputs, bool[,] matrix, int s, int a)
        {
            int findIndex = inputs.Length - 1;
            matrix[0, inputs[0] - a] = true;
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 4 };

            for (int i = 1; i < inputs.Length; i++)
            {
                Parallel.For(0, s, parallelOptions, j =>
                {
                    int check = j - inputs[i];
                    if (s - 1 >= check && check >= 0)
                    {
                        if (matrix[i - 1, j] || (j + a) == inputs[i] || matrix[i - 1, check])
                            matrix[i, j] = true;
                    }
                    else
                    {
                        if (matrix[i - 1, j] || (j + a) == inputs[i])
                            matrix[i, j] = true;
                    }
                });
               
                if (matrix[i, -a])
                {
                    findIndex = i;
                    break;
                }
            }
            return findIndex;
        }

        private static List<int> GetSubset(int[] inputs, bool[,] matrix, int a, int findIndex)
        {
            List<int> subset = new List<int>();
            subset.Add(inputs[findIndex]);
            int col = -a - inputs[findIndex];

            for (int i = findIndex - 1; i > 0; i--)
            {
                if (!matrix[i - 1, col])
                {
                    subset.Add(inputs[i]);
                    col = col - inputs[i];
                }
            }
            if (matrix[0, col])
            {
                subset.Add(inputs[0]);
            }
            return subset;
        }
    }
}