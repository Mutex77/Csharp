using System;

namespace PrimeFactorization
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "";
            string primeFactors = "";
            int n = 0;
            int prime = 2;
            bool done = false;
            bool factorComplete = false;
            

            while(!done)
            {
                prime = 2;
                primeFactors = "";

                Console.WriteLine("Enter an integer between 0 and 2,147,483,646 for prime factorization. Enter 0 to exit.");
                input = Console.ReadLine();

                if(int.TryParse(input, out n) && n >= 0 && n < 2147483647)
                {
                    if (n == 0)
                        done = true;
                    else if (!IsPrime(n))
                    {
                        while (!factorComplete)
                        {
                            if (n == 1)
                                factorComplete = true;
                            else if (IsPrime(n))
                            {
                                primeFactors += ", " + n;
                                factorComplete = true;
                            }
                            else if (n % prime == 0)
                            {
                                if (primeFactors.Length == 0)
                                    primeFactors += prime;
                                else
                                    primeFactors += ", " + prime;
                                
                                n = n / prime;
                                prime = 2;
                            } else
                            {
                                if (prime == 2)
                                    prime += 1;
                                else
                                {
                                    prime += 2;
                                    while (!IsPrime(prime))
                                        prime += 2;
                                }
                            }
                        }

                        factorComplete = false;
                        Console.WriteLine("Prime factors: " + primeFactors + "\n");

                    } else
                        Console.WriteLine("The integer that you entered is prime.\n");
                } else
                    Console.WriteLine("You did not enter an integer between 0 and 2,147,483,646.\n");
            }

        }

        private static bool IsPrime(int n)
        {
            int i;

            if (n <= 1)
                return false;
            else if (n <= 3)
                return true;
            else if (n % 2 == 0 || n % 3 == 0)
                return false;

            i = 5;
            while(i * i < n)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
                i += 6;
            }
            return true;
        }
    }
}
