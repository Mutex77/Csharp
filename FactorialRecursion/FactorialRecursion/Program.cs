using System;

namespace FactorialRecursion
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            int num, factorial;
            bool done = false;

            while (!done)
            {
                Console.WriteLine("Enter an positive integer to calculate the factorial. Enter 0 to exit.");
                input = Console.ReadLine();
                factorial = 1;

                if (input == "0")
                    done = true;
                else if (int.TryParse(input, out num) && num > 0)
                {
                    factorial = FindFactorial(num);
                    Console.WriteLine(num + " factorial = " + factorial + "\n");
                }
                else
                    Console.WriteLine("You did not enter a positive integer.\n");
            }
        }

        public static int FindFactorial(int n)
        {
            if(n == 1)
            {
                return 1;
            } else
            {
                return n * FindFactorial(n - 1);
            }
        }
    }
}
