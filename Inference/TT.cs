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
        private int[][] theModels;
        private List<String> AllSymbolAndStatment;
        private int TheNumberOfSymbol;

        public TT(string tell, string ask)
        {
            BuildKB(tell);
            Process(ask);   
        }



        public override void Process(string ask)
        {
            string output = "";
            if (Algorithm())
            {
                output = "YES: ";
                for (int i = 0; i < Entailed.Count; i++)
                {
                    output += Entailed[i] + ", ";
                }
                output += ask;
            }
            else
            {
                output = "NO";
            }
            
        }

        public override bool Algorithm()
        {
            while (Agenda.Count != 0)
            {
                // Pop the first element at agenda and temporarily store it at p
                string p = Agenda[0];
                Entailed.Add(p);
                Agenda.RemoveAt(0);
                // add the True and False into the whole TT map
                int rows = (int)Math.Pow(2, AllSymbolAndStatment.Count);
                for (int i = TheNumberOfSymbol; i < AllSymbolAndStatment.Count; i++)    // this step is to add the T/F into the position for statement 
                {
                    for (int j = 0; j < (Math.Pow(2, AllSymbolAndStatment.Count)); j++) // this step is pass the line number for fetch and find position
                    {

                        if (CheckStatement(AllSymbolAndStatment[i],j))
                        {
                            theModels[j][i] = 1;
                        }
                        else
                        {
                            theModels[j][i] = 0;
                        }
                    }
                }


            }
            // Cannot be entailed
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
                    Clauses.Add(sentences[i]);
                    Count.Add(sentences[i].Split('&').Length); 

                }
                else
                {
                    //If false, add to agenda
                    Agenda.Add(sentences[i]);
                }
            }

            //add every symbol into the agenda including the deuplicated
            //the deuplication will be remove in the next step
            for (int i = 0; i < Clauses.Count; i++) {
                Clauses[0].Replace("=>", " ");
                Clauses[0].Replace("&", " ");
                string[] temp = Clauses[0].Split(' ');
                for (int j = 0; j < temp.Length; j++) { Agenda.Add(temp[j]); }
            }

            BuildTheModels(Agenda);
            BuildWholeTT(Clauses);


        }

        public override bool Contains(string clause, string p)
        {
            // Get the premise by spliting the clause and get the first element.
            string premise = Regex.Split(clause, "=>")[0];
            // Get the conjucts by spliting premise
            List<string> conjucts = premise.Split('&').ToList();

            // Check the conjunctions in the clause.
            if (conjucts.Count == 1)
            {
                // If only 1, check the conjuct against p
                return premise.Equals(p);
            }
            else
            {
                // If multiple, check each of them against p
                return conjucts.Contains(p);
            }
        }

        // key section to add the result of statement
        public bool CheckStatement(string statement,int thelinenumber)
        {


            if (!statement.Contains("&"))
            {
                string[] temp = statement.Split('&');
                if (FindPosition(temp[0]) == 0 && FindPosition(temp[1]) == 1) { return false; }
                else { return true; }
            }
            else {
                string premise = Regex.Split(statement, "=>")[0];
                string aftermise = Regex.Split(statement, "=>")[1];
                string[] temp = premise.Split('&');
                
                if (FindPosition(temp[0]) == 0 && FindPosition(temp[1]) == 1)
                {
                   
                    if (FindPosition(aftermise) == 1) { return false; } else { return true; }
                }
                else { return true; }

            }


            

        }

        public bool Fetch(string symbol) {

            for (int i = 0; i < AllSymbolAndStatment.Count; i++) {
                if (AllSymbolAndStatment[i] == symbol) { return true; }
            }

            return false;
        }

        public int FindPosition(string symbol) {
            if (Fetch(symbol)) {

                for (int i = 0; i < AllSymbolAndStatment.Count; i++)
                {
                    if (AllSymbolAndStatment[i] == symbol) { return i; }
                }
            }

            return -1;
        }

        public void BuildTheModels(List<string> agenda)
        {
            // clear the deuplicated agenda
            for (int i = 0; i < agenda.Count; i++) {
                if (AllSymbolAndStatment.Contains(agenda[i])) { break; } else { AllSymbolAndStatment.Add(agenda[i]); }
            }
            TheNumberOfSymbol = AllSymbolAndStatment.Count;
            //set the init()
            int rows = (int)Math.Pow(2, AllSymbolAndStatment.Count);
            for (int i = 0; i < AllSymbolAndStatment.Count; i++)
            {
                for (int j = 0; j < (Math.Pow(2, AllSymbolAndStatment.Count)); j++)
                {

                    if (j < Math.Pow(2, AllSymbolAndStatment.Count - 1))
                    {
                        theModels[j][i] = 0;
                    }
                    else
                    {
                        theModels[j][i] = 1;
                    }
                }
            }
            
        }

        public void BuildWholeTT(List<string> clauses) {
            // add all the statement into the symbol
            for (int i = 0; i < Clauses.Count; i++)
            {
                if (AllSymbolAndStatment.Contains(Clauses[i])) { break; } else { AllSymbolAndStatment.Add(Clauses[i]); }
            }
            Algorithm();
        }
    }
}
