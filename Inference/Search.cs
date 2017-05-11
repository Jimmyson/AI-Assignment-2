using System.Collections.Generic;
using System.Linq;  // For ToList();

namespace Inference
{
    abstract class Search
    {
        protected List<string> Agenda { get; } // Initial Variables (i.e. A;)  

        // List<string> facts; 
        protected List<string> Clauses { get; }  // Sentences (i.e. A => B or A & C => B)
        protected List<string> Entailed { get; set; } // Response (taken from the agenda)
        protected List<int> Count { get; } // Count the conjunctions.

        abstract public void Process(string tell);    // Implementation of the respective algorithm's output
        abstract public bool Algorithm(); // Implementation of the respective algorithm

        public bool PremiseContains(string clause, string p)
        {
            // Get the premise by spliting the clause and get the first element.
            string premise = clause.Split(new[] { "=>" }, System.StringSplitOptions.None)[0];
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
        public void BuildKB(string tell)
        {
            // Split tell into list of strings
            List<string> sentences = tell.Split(';').ToList();
            for (int i=0; i<sentences.Count;i++)
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
    }
}