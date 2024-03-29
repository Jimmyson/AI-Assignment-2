﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Inference
{
    class Program
    {
        static string tell, ask = null;

        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if(File.Exists(args[1]))
                {
                    /*
                     * Handle following in seperate class.
                     */
                    StreamReader sr = File.OpenText(args[1]);
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
                        else
                        {
                            if(tellAsk)
                            { //TRUE TO READ INFRENCE;
                                tell += input;
                            } else
                            { //FALSE TO PROCESS;
                                ask += input;
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

                switch(args[0].ToUpper())
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
