using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountVowels
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            bool done = false;
            int cntVowels = 0, cntA = 0, cntE = 0, cntI = 0, cntO = 0, cntU = 0;

            while(!done)
            {
                Console.WriteLine("Type in a string to receive a count of each vowel. Enter 0 to exit.");
                input = Console.ReadLine();
                cntVowels = 0;
                cntA = 0;
                cntE = 0;
                cntI = 0;
                cntO = 0;
                cntU = 0;

                if (input == "0")
                    done = true;
                else
                {
                    foreach(char c in input.ToCharArray())
                    {
                        switch (char.ToLower(c))
                        {
                            case 'a':
                                cntVowels++;
                                cntA++;
                                break;
                            case 'e':
                                cntVowels++;
                                cntE++;
                                break;
                            case 'i':
                                cntVowels++;
                                cntI++;
                                break;
                            case 'o':
                                cntVowels++;
                                cntO++;
                                break;
                            case 'u':
                                cntVowels++;
                                cntU++;
                                break;
                            default:
                                break;
                        }
                    }
                }
                Console.WriteLine("The vowel counts are as follows:\nTotal vowels: " + cntVowels + "\nA: " + cntA + "\nE: " + cntE + "\nI: " + cntI + "\nO: " + cntO + "\nU: " + cntU + "\n");
            }
        }
    }
}
