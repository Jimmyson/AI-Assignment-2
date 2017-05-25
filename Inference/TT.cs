using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace Inference
{
    /*
     * Truth Table return the number of statements from Root to Goal. 
     */
    class TT : Search
    {
        //1 = True
        //0 = False

        //AGENDA contains all unique Variables (a, b, p1, etc.)

        private string Ask;
        private bool[,] Model;
        private List<String> AllSymbolAndStatment = new List<string>();

        public TT(string tell, string ask)
        {
            this.Ask = ask.Trim().ToLower();
            BuildKB(tell);
            //PrintModel();
            Process();   
        }

        /**
         * Count and print number of instances where scentence is true.
         */
        public override void Process()
        {
            int count = 0;

            for (int row = 0; row < (int) Math.Pow(2, Agenda.Count); row++)
            {
                //Identify rows where ASK is true;
                if (Model[row, Agenda.IndexOf(Ask.ToLower())])
                {
                    bool valid = true;
                    foreach (String sentence in Clauses)
                    {
                        if (!ProcessSentence(row, sentence))
                        {
                            //A sentence is FALSE, thus KB is FALSE
                            valid = false;
                            break;
                        }
                    }
                    // find if the KB is TRUE and the ask is TRUE in the same model
                    if (valid) count++;
                }
            }

            if (count > 0)
                Console.WriteLine("YES: " + count);
            else
                Console.WriteLine("NO");
        }

        public override bool Algorithm()
        {
            //DETERMING THE KB FROM THE SECENTESE (ProcessSentence(row, sentence)
                //USE EACH OF THE CLAUSES

            return false;
        }

        public override void BuildKB(string tell)
        {
            // Split tell into list of strings
            List<string> sentences = tell.Split(';').ToList();
            for (int i = 0; i < sentences.Count; i++)
            {
                //Look for Inference
                if (sentences[i].Contains("=>"))
                {
                    //If true, add to clause and count the conjuctions (A&B or Z)
                    Clauses.Add(sentences[i].ToLower());
                    Count.Add(sentences[i].Split('&').Length); 
                }
                else if(!sentences[i].Trim().Equals(""))  //If false and not empty, add to agenda
                    Agenda.Add(sentences[i].ToLower());
            }

            //add every symbol into the agenda including the deuplicated
            //the deuplication will be remove in the next step
            for (int i = 0; i < Clauses.Count; i++) {
                string statement = Clauses[i];
                statement = statement.Replace("=>", " ").Replace("&", " ");
                string[] temp = statement.Split(' ');
                for (int j = 0; j < temp.Length; j++)
                {
                    if (!Agenda.Contains(temp[j])) //Check for Duplicates
                        Agenda.Add(temp[j]);
                }
            }

            BuildTheModels();
        }

        public override bool Contains(string clause, string p)
        {
            // Get the premise by spliting the clause and get the first element.
            string premise = Regex.Split(clause, "=>")[0];

            // Get the conjucts by spliting premise
            List<string> conjucts = premise.Split('&').ToList();

            // Check the conjunctions in the clause.
            // If only 1, check the conjuct against p
            // Else, check each of them against p
            return (conjucts.Count == 1) ? premise.Equals(p) : conjucts.Contains(p);
        }

        // key section to add the result of statement
        public bool CheckStatement(string statement,int thelinenumber)
        {
            // currently can handle a&b=>c ; a=>c;
            string premise = Regex.Split(statement, "=>")[0];
            string aftermise = Regex.Split(statement, "=>")[1];

            if (!statement.Contains("&"))
                return (FindPosition(premise) == 0 && FindPosition(aftermise) == 1) ? false : true;
            else {
                string[] temp = premise.Split('&');
                
                if (FindPosition(temp[0]) == 0 && FindPosition(temp[1]) == 1)
                    return (FindPosition(aftermise) == 1) ? false : true;
                else
                    return true;
            }
        }

        //DISABLE
        public bool Fetch(string symbol) {
            if(Agenda.Contains(symbol.Trim().ToLower()))
            for (int i = 0; i < AllSymbolAndStatment.Count; i++) {
                if (AllSymbolAndStatment[i] == symbol)
                    return true;
            }

            return false;
        }

        //DISABLE
        public int FindPosition(string symbol) {
            if (Fetch(symbol))
            {
                for (int i = 0; i < AllSymbolAndStatment.Count; i++)
                {
                    if (AllSymbolAndStatment[i] == symbol)
                        return i;
                }
            }

            return -1;
        }

        public void BuildTheModels()
        {
            //Determine rows from variation in ALL unique variables.
            int rows = (int)Math.Pow(2, Agenda.Count);

            //Build the array.
            this.Model = new bool[rows, Agenda.Count];

            for (int row = 0; row < rows; row++)
            {
                for (int agd = 0; agd < Agenda.Count; agd++)
                {
                    int mask = (int)Math.Pow(2, agd);
                    Model[row,agd] = (row & mask) == mask;
                }
            }            
        }

        public bool ProcessSentence(int row, string sentence)
        {
            //@TODO: Seperate the Clauses. =>, &
            //Reads left to Right, thus seperate from right to left.

            string[] segments;

            //FIX IF LOGIC. LOOK FOR END LOGIC SYMBOL
            int pos;
            if ((pos = sentence.LastIndexOf("=>")) > 0)
            {
                segments = new string[2];
                segments[0] = sentence.Substring(0, pos);
                segments[1] = sentence.Substring(pos + 2, sentence.Length-(pos+2));

                bool[] logic = new bool[segments.Count()];
                for (int i = 0; i < segments.Count(); i++)
                {
                    logic[i] = ProcessSentence(row, segments[i]);
                }
                return Inference(logic[0], logic[1]);
            }
            else if ((pos = sentence.LastIndexOf("&")) > 0)
            {
                segments = new string[2];
                segments[0] = sentence.Substring(0, pos);
                segments[1] = sentence.Substring(pos+1, sentence.Length-(pos+1));

                bool[] logic = new bool[segments.Count()];
                for (int i = 0; i < segments.Count(); i++)
                {
                    logic[i] = ProcessSentence(row, segments[i]);
                }
                return And(logic[0], logic[1]);
            }
            else
                return Model[row, Agenda.IndexOf(sentence)];
        }

        public bool Inference(bool p, bool q)
        {
            return (!p || q);
        }

        public bool And(bool p, bool q)
        {
            return (p && q);
        }

        public bool Not(bool p)
        {
            return !p;
        }

        public void PrintModel()
        {
            for (int j = 0; j < (int) Math.Pow(2, Agenda.Count); j++)
            {
                Console.Write(j + ": ");
                for (int i = 0; i < Agenda.Count; i++)
                {
                    if (Model[j,i])
                    {

                        Console.Write("1 ");
                    } else
                    {

                        Console.Write("0 ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
