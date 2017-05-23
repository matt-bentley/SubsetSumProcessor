using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections;
using ZeroFormatter;

namespace SubsetSumCore
{
    class Program
    {
        // Need to remove items from inputs that are greater than a or b

        static void Main(string[] args)
        {
            //int[] inputs = { -5, -2, -1, 3, 7 };
            //decimal[] inputs = { -5.10m, -2.01m, -1.00m, 3.00m, 7.11m };
            decimal[] inputs = { 994.04m, -50000.81m, -6543.6m, -1228.06m, 4833.2m, -8613.36m, -3189.17m, 5697.23m, -8962.98m, -1670.83m, 6763.97m, -3941.09m, 5214.52m, -1206.61m, -8979.49m, 6127.02m, -5797.81m, 9774.32m, 929.25m, -9431.31m, 2189.93m, -4977.58m, -278.64m };

            Stopwatch timer = new Stopwatch();
            timer.Start();

            var subset = FindSubsetRecursive(inputs);
            var isSubset = subset != null;

            timer.Stop();
            var runTime = timer.Elapsed;

            Console.WriteLine($"Match recursive: {isSubset} in {runTime.Seconds.ToString()}secs {runTime.Milliseconds}ms");
            if (isSubset)
            {
                Console.Write("[");
                for (int i = 0; i < subset.Length; i++)
                {
                    Console.Write(subset[i].ToString());
                    if (i + 1 != subset.Length)
                        Console.Write(",");
                }
                Console.Write("]");
            }
            Console.WriteLine("");
            
            timer.Reset();
            timer.Start();

            //subset = FindSubsetDynamicBits(inputs);
            subset = FindSubsetDynamic(inputs);
            isSubset = subset != null;

            timer.Stop();
            runTime = timer.Elapsed;         

            Console.WriteLine($"Match dynamic: {isSubset} in {runTime.Seconds.ToString()}secs {runTime.Milliseconds}ms");
            if (isSubset)
            {
                Console.Write("[");
                for (int i = subset.Length - 1; i >= 0; i--)
                {
                    Console.Write(subset[i].ToString());
                    if (i + 1 != subset.Length)
                        Console.Write(",");
                }
                Console.Write("]");
            }
            Console.ReadKey();
        }

        private static decimal[] FindSubsetDynamic(decimal[] doubles)
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

                //int[] subset = FindSubsetDynamic(inputs);
                int[] subset = FindSubsetDynamicSparse(inputs);
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

        private static decimal[] FindSubsetDynamicBits(decimal[] doubles)
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

                int[] subset = FindSubsetDynamicBits(inputs);

                decimal[] subsetDecimal = null;

                if (subset != null && subset.Length > 0)
                {
                    subsetDecimal = new decimal[subset.Length];

                    for (int i = 0; i < subset.Length; i++)
                    {
                        subsetDecimal[i] = (decimal)subset[i] / 100;
                    }
                }

                return subsetDecimal;
            }
            catch (InvalidCastException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int[] FindSubsetDynamic(int[] inputs)
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

        private static int[] FindSubsetDynamicBits(int[] inputs)
        {
            int a = 0;
            int b = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] > 0)
                    b += inputs[i];
                else
                    a += inputs[i];
            }

            int s = (b - a) + 1;

            BitArray[] matrix = new BitArray[inputs.Length];
            //for(int i = 0; i < inputs.Length; i++)
            //{
            //    matrix[i] = new BitArray(s);
            //}

            int findIndex = FillMatrixBits(inputs, matrix, s, a);

            bool isSubset = matrix[findIndex][-a];
            List<int> subset;
            int[] subsetArray = null;
            if (isSubset)
            {
                subset = GetSubsetBits(inputs, matrix, a, findIndex);
                if (subset != null && subset.Count > 0)
                {
                    subsetArray = subset.ToArray();
                }
            }
            return subsetArray;
        }

        private static int[] FindSubsetDynamicSparse(int[] inputs)
        {
            int a = 0;
            int b = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i] > 0)
                    b += inputs[i];
                else
                    a += inputs[i];
            }

            int s = (b - a) + 1;

            Dictionary<int, bool>[] matrix = new Dictionary<int, bool>[inputs.Length];

            int findIndex = FillSparseMatrix(inputs, matrix, s, a);

            bool isSubset = matrix[findIndex].ContainsKey(-a);
            List<int> subset;
            int[] subsetArray = null;
            if (isSubset)
            {
                subset = GetSubsetSparse(inputs, matrix, a, findIndex);
                if (subset != null && subset.Count > 0)
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
                matrix[i, inputs[i] - a] = true;

                for (int j = 0; j < s; j++)
                {
                    int check = j - inputs[i];
                    if (s - 1 >= check && check >= 0)
                    {
                        if (matrix[i - 1, j] || matrix[i - 1, check])
                            matrix[i, j] = true;
                    }
                    else
                    {
                        if (matrix[i - 1, j])
                            matrix[i, j] = true;
                    }
                }
                if (matrix[i, -a])
                {
                    findIndex = i;
                    break;
                }
            }
            int length = s * matrix.Length * 4;
            Console.WriteLine($"Standard Bool Array Byte length: {length}");
            return findIndex;
        }

        private static int FillMatrixBits(int[] inputs, BitArray[] matrix, int s, int a)
        {
            matrix[0] = new BitArray(s);
            int findIndex = inputs.Length - 1;
            matrix[0][ inputs[0] - a] = true;

            for (int i = 1; i < inputs.Length; i++)
            {
                matrix[i] = new BitArray(matrix[i - 1]);
                matrix[i][inputs[i] - a] = true;

                for (int j = 0; j < s; j++)
                {
                    if (!matrix[i][j])
                    {
                        int check = j - inputs[i];
                        if (s - 1 >= check && check >= 0)
                        {
                            if (matrix[i - 1][check])
                                matrix[i][j] = true;
                        }
                    }
                }
                if (matrix[i][-a])
                {
                    findIndex = i;
                    break;
                }
            }
            int length = (s * matrix.Length) / 8;
            Console.WriteLine($"Bit Array Byte length: {length/1000000000}");
            return findIndex;
        }

        private static int FillSparseMatrix(int[] inputs, Dictionary<int, bool>[] matrix, int s, int a)
        {
            int findIndex = inputs.Length - 1;
            matrix[0] = new Dictionary<int, bool>();
            matrix[0].Add(inputs[0] - a, true);

            for (int i = 1; i < inputs.Length; i++)
            {
                matrix[i] = new Dictionary<int, bool>(matrix[i-1]);
                matrix[i].Add(inputs[i] - a, true);

                foreach (var key in matrix[i - 1])
                {
                    if (!matrix[i - 1].ContainsKey(inputs[i] + key.Key))
                        matrix[i].Add(inputs[i] + key.Key, true);
                }

                if (matrix[i].ContainsKey(-a))
                {
                    findIndex = i;
                    break;
                }
            }
            GetSparseArrayDetails<Dictionary<int, bool>[]>(matrix, inputs.Length);
            PrintByteLength<Dictionary<int, bool>[]>(matrix);
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

        private static List<int> GetSubsetBits(int[] inputs, BitArray[] matrix, int a, int findIndex)
        {
            List<int> subset = new List<int>();
            subset.Add(inputs[findIndex]);
            int col = -a - inputs[findIndex];

            for (int i = findIndex - 1; i > 0; i--)
            {
                if (!matrix[i - 1][col])
                {
                    subset.Add(inputs[i]);
                    col = col - inputs[i];
                }
            }
            if (matrix[0][col])
            {
                subset.Add(inputs[0]);
            }
            return subset;
        }

        private static List<int> GetSubsetSparse(int[] inputs, Dictionary<int, bool>[] matrix, int a, int findIndex)
        {
            List<int> subset = new List<int>();
            subset.Add(inputs[findIndex]);
            int col = -a - inputs[findIndex];

            for (int i = findIndex - 1; i > 0; i--)
            {
                if (!matrix[i - 1].ContainsKey(col))
                {
                    subset.Add(inputs[i]);
                    col = col - inputs[i];
                }
            }
            if (matrix[0].ContainsKey(col))
            {
                subset.Add(inputs[0]);
            }
            return subset;
        }

        private static decimal[] FindSubsetRecursive(decimal[] inputs)
        {
            bool isComplete = false;
            string result = "";
            result = FindRecursive(inputs, 0, 0, result, ref isComplete);
            if(result != "")
            {
                string[] values = result.Split(',');
                decimal[] subset = new decimal[values.Length];
                for(int i = 0; i < values.Length; i++)
                {
                    subset[i] = inputs[Convert.ToInt32(values[i])];
                }
                return subset;
            }
            return null;
        }

        private static string FindRecursive(decimal[] inputs, int currIndex, decimal currTotal, string currResult, ref bool isComplete)
        {
            for(int i = currIndex; i < inputs.Length; i++)
            {
                if(TestSubset(currTotal, inputs[i]))
                {
                    string result = ExtendResult(currResult, i.ToString());
                    isComplete = true;
                    return result;
                }
                else if (currIndex < inputs.Length)
                {
                    string exResult = ExtendResult(currResult, i.ToString());
                    exResult = FindRecursive(inputs, i + 1, currTotal + inputs[i], exResult, ref isComplete);
                    if (isComplete)
                    {
                        return exResult;
                    }
                }
            }
            return "";
        }

        private static bool TestSubset(decimal currTotal, decimal input)
        {
            // Can add a tolerance here if needed
            return currTotal + input == 0;
        }

        private static string ExtendResult(string currResult, string newVal)
        {
            return currResult == "" ? newVal : $"{currResult},{newVal}";
        }

        private static void GetSparseArrayDetails<T>(T obj, int rowCount)
        {
            var bytes = ZeroFormatterSerializer.Serialize(obj);
            Console.WriteLine($"Byte length: {bytes.Length}");
            long bitCount = bytes.Length * 8;
            Console.WriteLine($"Bit count: {bitCount}");
            long indexCount = (long)Math.Pow((double)2,(double)(rowCount + 1))  - 2 - rowCount;
            Console.WriteLine($"Index count: {indexCount}");
            Console.WriteLine($"Bits per index: {bitCount / indexCount}");
        }

        private static void PrintByteLength<T>(T obj)
        {
            var bytes = ZeroFormatterSerializer.Serialize(obj);
            Console.WriteLine($"Byte length: {bytes.Length}");
        }
    }
}
