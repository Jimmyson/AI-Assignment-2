using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Inference
{
    /*
     * Return path of node chain, reading from destination. Does not print other root nodes.
     * Checks for loops.
     * eg: p1, p2, p3, d
     */
    class BC : Search
    {
        protected List<string> Facts;

        public BC(string tell, string ask)
        {
            // Identify Ask Parameter
            Agenda.Add(ask);

            // Build the Knowledge-Base
            BuildKB(tell);

            // Process the KB
            Process();
        }

        public override bool Algorithm()
        {
            // -> while the list of symbols are not empty
            while (Agenda.Count() > 0)
            {
                // -> get current symbol
                string query = Agenda[Agenda.Count() - 1];
                Agenda.RemoveAt(Agenda.Count() - 1);
                // -> add the entailed array
                Entailed.Add(query);

                // -> if this element is a fact then we dont need to go further
                if (!Facts.Contains(query))
                {
                    // -> .. but it isnt so..
                    // -> create array to hold new symbols to be processed 
                    List<String> p = new List<String>();
                    for (int i = 0; i < Clauses.Count(); i++)
                    {
                        // -> for each clause..
                        if (Contains(Clauses[i], query))
                        {
                            // -> that contains the symbol as its conclusion

                            List<String> temp = GetPremises(Clauses[i]);
                            for (int j = 0; j < temp.Count(); j++)
                            {
                                // add the symbols to a temp array
                                p.Add(temp[j]);
                            }
                        }
                    }
                    // -> no symbols were generated and since it isnt a fact either 
                    // -> then this sybmol and eventually ASK  cannot be implied by TELL
                    if (p.Count() == 0)
                    {
                        return false;
                    }
                    else
                    {
                        // -> there are symbols so check for previously processed ones and add to agenda
                        for (int i = 0; i < p.Count(); i++)
                        {
                            if (!Entailed.Contains(p[i]))
                                Agenda.Add(p[i]);
                        }


                    }
                }

            }// -> while end
            return true;

            //throw new NotImplementedException();
        }

        public override void BuildKB(string tell)
        {
            string[] sentences = Regex.Split(tell, ";");

            foreach (string sentence in sentences)
            {
                if (!sentence.Contains("=>"))
                    Facts.Add(sentence);
                else
                    Clauses.Add(sentence);
            }

            //throw new NotImplementedException();
        }

        public override bool Contains(string clause, string search)
        {
            string conclusion = Regex.Split(clause, "=>")[1];

            return (conclusion.Equals(search)) ? true : false;
            //throw new NotImplementedException();
        }

        public override void Process()
        {
            string output = "";

            if (Algorithm())
            {
                output += "YES: ";
                foreach(String Entail in Entailed)
                {
                    output += (Entailed.Count < 2) ? Entail : Entail + ", ";
                }
            } else
            {
                output += "NO";
            }
            Console.WriteLine(output);
            //throw new NotImplementedException();
        }

        // -> methid that returns the conjuncts contained in a clause
        public List<String> GetPremises(string clause)
        {
            // -> get the premise
            string premise = Regex.Split(clause,"=>")[0];
            List<String> temp = new List<String>();
            String[] conjuncts = Regex.Split(premise,"&");
            // -> for each conjunct
            for (int i = 0; i < conjuncts.Length; i++)
            {
                if (!Agenda.Contains(conjuncts[i]))
                    temp.Add(conjuncts[i]);
            }
            return temp;
        }
    }
}
