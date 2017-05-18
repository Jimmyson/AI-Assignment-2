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




        public Boolean TT-Check-All(LogicalExpression  KB, LogicalExpression aphla, List symbols, Map model) {
            return 

        }

    }
}
