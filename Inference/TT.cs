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
        private List<String> TheSymbol;


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

                for (int i = 0; i < Clauses.Count; i++)
                {
                    // Check if 
                    if (Contains(Clauses[i], p))
                    {
                        //Decrement Count at i and checks if zero
                        if ((--Count[i]) == 0)
                        {
                            // Get the Conclusion
                            string head = Regex.Split(Clauses[i], "=>")[1];
                            // Check if reach ask
                            if (head.Equals(ask))
                                return true;
                            Agenda.Add(head);
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


        public bool Fetch(string symbol) {

            for (int i = 0; i < TheSymbol.Count; i++) {
                if (TheSymbol[i] == symbol) { return true; }
            }

            return false;
        }

        public int FindPosition(string symbol) {
            if (Fetch(symbol)) {

                for (int i = 0; i < TheSymbol.Count; i++)
                {
                    if (TheSymbol[i] == symbol) { return i; }
                }
            }

            return -1;
        }

        public int[][] BuildTheModels(List<string> agenda)
        {
            // clear the deuplicated agenda
            for (int i = 0; i < agenda.Count; i++) {
                if (TheSymbol.Contains(agenda[i])) { break; } else { TheSymbol.Add(agenda[i]); }
            }
            //set the init()
            int rows = (int)Math.Pow(2, TheSymbol.Count);
            for (int i = 0; i < TheSymbol.Count; i++)
            {
                for (int j = 0; j < (Math.Pow(2, TheSymbol.Count)); j++)
                {

                    if (j < Math.Pow(2, TheSymbol.Count - 1))
                    {
                        theModels[j][i] = 0;
                    }
                    else
                    {
                        theModels[j][i] = 1;
                    }
                }
            }
            return theModels;
        }
       

    }
}
