using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Inference
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 3)
            {
                if(File.Exists(args[2]))
                {
                    /*
                     * Handle following in seperate class.
                     */
                    StreamReader sr = File.OpenText(args[2]);

                    //LOOK FOR TELL
                    string input;
                    bool tellAsk = false;
                    while (!sr.EndOfStream)
                    {
                        input = sr.ReadLine();
                        if (input.Trim(' ') == "TELL") {
                            tellAsk = true;
                        }
                        else if(input.Trim(' ') == "ASK")
                        {
                            tellAsk = false;
                        }
                        else
                        {
                            if(tellAsk)
                            { //TRUE TO READ INFRENCE;
                                string[] infrence = input.Split(';');
                                foreach (string equation in infrence)
                                {
                                    //Look for inference '=>';
                                    //Look for Brackjets
                                    //Look for Logic '&';
                                    //Look for variables;
                                    //Save equation;
                                }
                            } else
                            { //FALSE TO PROCESS;
                                
                            }
                        }
                    }

                    //LOOK FOR ASK
                }
                else
                {
                    Console.Write("File does not exist.");
                    return;
                }

                switch(args[1].ToUpper())
                {
                    case "TT":
                        break;
                    case "FC":
                        break;
                    case "BC":
                        break;
                    default:
                        Console.Write("Invalid Method");
                        return;
                }
            } else
            {
                Console.WriteLine("INCOMPLETE ARGUMENTS");
                Console.Write("Please include the Method and A Valid File.");
            }
            return;
        }
    }
}
