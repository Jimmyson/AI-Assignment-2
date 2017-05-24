using System.Collections.Generic;
using System.Linq;  // For ToList();

namespace Inference
{
    abstract class Search
    {
        protected List<string> agenda = new List<string>();
        protected List<string> clauses = new List<string>();
        protected List<string> entailed = new List<string>();
        protected List<int> count = new List<int>();

        /* GET / SET */
        protected List<string> Agenda { get => agenda; } // Initial Variables (i.e. A;)  
        protected List<string> Clauses { get => clauses; }  // Sentences (i.e. A => B or A & C => B)
        protected List<string> Entailed { get => entailed; set => entailed = value; } // Response (taken from the agenda)
        protected List<int> Count { get => count; } // Count the conjunctions.

        abstract public void Process();    // Implementation of the respective algorithm's output
        abstract public bool Algorithm(); // Implementation of the respective algorithm
        abstract public void BuildKB(string tell);  // Build the Facts and Clauses

        /**
         * Looks for a variable. Depends on Chaining Method.
         */
        abstract public bool Contains(string clause, string search);    //Search for the variable;
    }
}