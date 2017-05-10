using System.Collections.Generic;

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
            // Check the conjuncts in the clause.
            // If only 1, check the conjuct against p
            // If multiple, check each of them against p
            return false;
        }
        public void BuildKB(string tell)
        {
            //Look for Inference
            //If true, add to clause and count the conjuctions (A&B or Z)
            //If false, add to agenda
        }
    }
}