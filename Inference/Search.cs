using System.Collections.Generic;
using System.Linq;  // For ToList();

namespace Inference
{
    abstract class Search
    {
        List<string> agenda; // Initial Variables (i.e. A;)  

        // List<string> facts; 
        List<string> clauses;  // Sentences (i.e. A => B or A & C => B)
        List<string> entailed; // Response (taken from the agenda)
        List<int> count; // Count the conjunctions.

        abstract public void Process();    // Implementation of the respective algorithm's output
        abstract public bool Algorithm(); // Implementation of the respective algorithm

        public bool PremiseContains(string clause, string p)
        {
            // Check the conjuncts in the clause.
            // If only 1, check the conjuct against p
            // If multiple, check each of them against p
            return false;
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
                    clauses.Add(sentences[i]);
                    count.Add(sentences[i].Split('&').Length);
                }
                else
                {
                    //If false, add to agenda
                    agenda.Add(sentences[i]);
                }
            }
        }
    }
}