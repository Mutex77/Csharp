using System;

namespace PiToNthDigit
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            int numDigits;
            bool done;

            done = false;
            
            while (!done)
            {
                Console.WriteLine("Enter an integer between 1 and 10 to display that many decimal places of PI. Enter 0 to exit.");
                input = Console.ReadLine();

                if (int.TryParse(input, out numDigits))
                {
                    if (numDigits == 0)
                    {
                        done = true;
                    }
                    else if (1 <= numDigits && numDigits <= 10)
                    {
                        Console.WriteLine(Math.Round(Math.PI, numDigits) + "\n");
                    } else
                    {
                        Console.WriteLine("You did not enter an integer between 0 and 10.\n");
                    }
                } else
                {
                    Console.WriteLine("You did not enter an integer between 0 and 10.\n");
                }
            }
        }
    }
}
