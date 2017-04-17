using System;

namespace SubsetSumCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] inputs = { -5, -2, -1, 3, 7 };
            int[] inputs = { -5, -2, -1, 4, 20 };
            bool success = FindDynamic(inputs);
            Console.WriteLine($"Match: {success}");
            Console.ReadKey();
        }

        private static bool FindDynamic(int[] inputs)
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

            int findIndex = inputs.Length - 1;
            int check = 0;
            matrix[0,inputs[0] - a] = true;

            for (int i = 1; i < inputs.Length; i++)
            {
                for(int j = 0; j < s; j++)
                {
                    check = j - inputs[i];
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
                if (matrix[i,-a])
                {
                    findIndex = i;
                    break;
                }
            }

            return matrix[findIndex, -a];
        }
    }
}