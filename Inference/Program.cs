using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Inference
{
    class Program
    {
        static string tell, ask = null;

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
                    Regex regex = new Regex("\\s+");

                    //LOOK FOR TELL
                    string input;

                    bool tellAsk = false;
                    while (!sr.EndOfStream)
                    {
                        input = sr.ReadLine();
                        if(regex.Replace(input, " ") == "TELL") {
                            tell = "";
                            tellAsk = true;
                        }
                        else if(regex.Replace(input, " ") == "ASK")
                        {
                            ask = "";
                            tellAsk = false;
                        }
                        else if(tellAsk)
                        {
                            if(tellAsk && )
                            { //TRUE TO READ INFRENCE;
                                tell += sr.ReadLine();
                            } else
                            { //FALSE TO PROCESS;
                                ask += sr.ReadLine();
                            }
                        }
                    }
                    if (tell != null && ask != null)
                    {
                        //Trim Whitespace
                        tell = regex.Replace(tell, "");
                        ask = regex.Replace(ask, "");
                    } else
                    {
                        Console.Write("Missing TELL or ASK");
                        return;
                    }
                }
                else
                {
                    Console.Write("File does not exist.");
                    return;
                }

                switch(args[1].ToUpper())
                {
                    case "TT":
                        TT truth = new TT(tell, ask);
                        break;
                    case "FC":
                        FC forward = new FC(tell, ask);
                        break;
                    case "BC":
                        BC backward = new BC(tell, ask);
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
