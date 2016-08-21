using System;

namespace FibonacciToN
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string output = "";
            int n;
            int fibA;
            int fibB;
            bool done = false;

            while(!done)
            {
                Console.WriteLine("Enter an integer between 1 and 20 to generate the Fibonacci sequence to the Nth number. Enter 0 to exit.");
                input = Console.ReadLine();

                if(int.TryParse(input, out n) && 0 <= n && n <= 20)
                {
                    if (n == 0)
                    {
                        done = true;
                    } else
                    {
                        fibA = 0;
                        fibB = 1;

                        for(int i = 0; i < n; i++)
                        {
                            if(i == 0)
                            {
                                output = fibA.ToString();
                            } else if (i == 1)
                            {
                                output += ", " + fibB;
                            } else if (i % 2 == 0)
                            {
                                fibA = fibA + fibB;
                                output += ", " + fibA;
                            } else
                            {
                                fibB = fibA + fibB;
                                output += ", " + fibB;
                            }
                        }

                        Console.WriteLine(output + "\n");
                    }
                } else
                {
                    Console.WriteLine("You did not enter an integer between 0 and 20.\n");
                }
            }
        }
    }
}
