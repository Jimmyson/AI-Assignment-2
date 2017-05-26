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
        private List<String> AllSymbolBeenIndicated = new List<string>();  // this used to store the aymbol that been indicated by others,
                                                                           // as a example "a&b =>c", c will be stored inside, so it helps check and search
        private List<String> SingleStatement = new List<string>();         
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
            if (CheckReferences(Ask))
            {   //check if Ask exist and been controlled by the condition

                string statement = ReturnReferences(Ask);
                Console.WriteLine(Model[2001, FindPosition(Ask)]);
                Console.WriteLine(FindPosition(Ask));

                for (int row = 0; row < (int)Math.Pow(2, Agenda.Count); row++)
                {
                    if (SearchUntilEnd(row, statement) && Model[row, FindPosition(Ask)]) {

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

                            bool valid2 = true;
                            
                            foreach (String sentence in SingleStatement)
                            {
                                if (Model[row, FindPosition(sentence)] == false)
                                {
                                    valid2 = false;
                                    break;

                                }
                            }

                            if (valid && valid2)
                            {
                               
                                count++;
                            }
                        }
                    }
                    /*
                    if (SearchUntilEnd(row, statement) && Model[row, FindPosition(Ask)])
                    {

                        count += 1;
                    }*/

                        //Identify rows where ASK is true;
                       
                }
            }

            // see if KB out of ask
            
            for (int row = 0; row < (int)Math.Pow(2, Agenda.Count); row++)
            {
                

                    if (Model[row, Agenda.IndexOf(Ask.ToLower())])
                    {
                        bool checkKB = false;
                        foreach (String sentence in Clauses)
                        {
                            if (!ProcessSentence(row, sentence))
                            {
                            //A sentence is FALSE, thus KB is FALSE
                            checkKB = false;
                            break;
                            }
                        checkKB = true;


                        }
                    // if KB is out of the ask, then it can not entail it

                    if (Model[row, FindPosition(Ask)] == false && checkKB == true)
                    {
                        count = 0;

                    };
                }
                

            }



            if (count > 0)
            {
                Console.WriteLine("YES: " + count);
                foreach (String sentence in AllSymbolBeenIndicated)
                { Console.WriteLine(sentence); }
            }
            else
            {
                Console.WriteLine("NO" );
             //   foreach (String sentence in AllSymbolBeenIndicated)
             //   { Console.WriteLine(sentence); }

                
            }
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
                {    Agenda.Add(sentences[i].ToLower());
                    SingleStatement.Add(sentences[i].ToLower());
                }
            }

            //add every symbol into the agenda including the deuplicated
            //the deuplication will be remove in the next step
            
            for (int i = 0; i < Clauses.Count; i++) {
                string statement = Clauses[i];
                string aftermise = Regex.Split(statement, "=>")[1];
                AllSymbolBeenIndicated.Add(aftermise);
                Console.WriteLine(aftermise);
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
            AllSymbolBeenIndicated.Add(aftermise);
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


        public bool Fetch(string symbol) {
            if(Agenda.Contains(symbol.Trim().ToLower()))
            for (int i = 0; i < agenda.Count; i++) {
                if (agenda[i] == symbol)
                    return true;
            }

            return false;
        }


        public int FindPosition(string symbol) {
            if (Fetch(symbol))
            {
                for (int i = 0; i < agenda.Count; i++)
                {
                    if (agenda[i] == symbol)
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

        public bool OR(bool p, bool q)
        {
            return (p || q);
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


        // new features 
        //1.CheckReferences
        //2.ReturnReferences
        //3.SearchUntilEnd    <- this one is the core method to search if all the relationship fits the needs

        public bool CheckReferences(string statement)
        {
            if (AllSymbolBeenIndicated.Contains(statement)) {

            return true;
            }
            return false;
        }

        public string ReturnReferences(string statement)
        {
            if (AllSymbolBeenIndicated.Contains(statement))
            {
                for (int i = 0; i < Clauses.Count; i++)
                {
                    string statementlocal = Clauses[i];
                    string premise = Regex.Split(statementlocal, "=>")[0];
                    string aftermise = Regex.Split(statementlocal, "=>")[1];
                    if (statement == aftermise) {
                        return statementlocal;
                    }
                    
                }
            }
            return null;
        }

        // currecntly can handle "a&b => c" and "a||b => c"
        public bool SearchUntilEnd(int row, string statement) {
            
            string premise = Regex.Split(statement, "=>")[0];
            string aftermise = Regex.Split(statement, "=>")[1];
            int pos;
            if ((pos = premise.LastIndexOf("&")) > 0)
            {
                string first = Regex.Split(statement, "&")[0];
                string second = Regex.Split(statement, "&")[1];

                bool result1;
                bool result2;

                if (And(Model[row, FindPosition(first)], Model[row, FindPosition(second)]) && ProcessSentence(row,statement))
                {
                    if (ReturnReferences(first) != null)
                    {
                        string newstatement = ReturnReferences(first);
                        result1 = SearchUntilEnd(row, newstatement);
                    }
                    else { result1 = true; }
                    if (ReturnReferences(second) != null)
                    {
                        string newstatement = ReturnReferences(second);
                        result2 = SearchUntilEnd(row, newstatement);
                    }
                    else { result2 = true; }


                    if (result1 && result2) { return true; }
                    else { return false; }
                }
                else { return false; }


            }
            else if ((pos = premise.LastIndexOf("||")) > 0)
            {
                string first = Regex.Split(statement, "||")[0];
                string second = Regex.Split(statement, "||")[1];

                bool result1;
                bool result2;

                if (OR(Model[row, FindPosition(first)], Model[row, FindPosition(second)]) && ProcessSentence(row, statement))
                {
                    if (ReturnReferences(first) != null)
                    {
                        string newstatement = ReturnReferences(first);
                        result1 = SearchUntilEnd(row, newstatement);
                    }
                    else { result1 = true; }
                    if (ReturnReferences(second) != null)
                    {
                        string newstatement = ReturnReferences(second);
                        result2 = SearchUntilEnd(row, newstatement);
                    }
                    else { result2 = true; }

                    if (result1 && result2) { return true; }
                    else { return false; }
                }
                else { return false; }
            }
            else
            {
                bool result = false; 
                if (Model[row, FindPosition(premise)]==true && ProcessSentence(row, statement)==true)
                {
                    if (ReturnReferences(premise) != null)
                    {
                        string newstatement = ReturnReferences(premise);
                        result = SearchUntilEnd(row, newstatement);
                    }
                    else { result = true; }
                    

                }
                else { result = false; }
                return result;

            }

        }

    }
}
