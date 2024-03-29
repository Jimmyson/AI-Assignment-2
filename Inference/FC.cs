﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace Inference
{
    /*
     * Read all root nodes, and prints full node path to destintion if found.
     * eg: a, b, p1, p2, p3, d
     */
    class FC : Search
    {
        private string Ask;

        public FC(string tell, string ask)
        {
            this.Ask = ask;
            BuildKB(tell);
            Process();
        }

        // Write result of Front-Chain algorithm
        public override void Process()
        {
            string output = "";
            if (Algorithm())
            {
                output = "YES: ";
                for (int i = 0; i < Entailed.Count; i++)
                {
                    output += Entailed[i] + ", ";
                }
                output += Ask;
            }
            else
            {
                output = "NO";
            }
            Console.WriteLine(output);
        }

        // Start the Front-Chain process
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
                    if (Contains(Clauses[i], p))
                    {
                        //Decrement Count at i and checks if zero
                        if ((--Count[i]) == 0)
                        {
                            // Get the Conclusion
                            string head = Regex.Split(Clauses[i], "=>")[1];
                            // Check if reach ask
                            if (head.Equals(Ask))
                                return true;
                            Agenda.Add(head);
                        }

                    }

                }
            }
            // Cannot be entailed
            return false;
        }

        // Build Knowledge Base
        public override void BuildKB(string tell)
        {
            // Split tell into list of strings
            List<string> sentences = tell.Split(';').ToList();

            // Remove the last element which is empty due to split.
            sentences.RemoveAt(sentences.Count - 1);
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

        // Check if p appears in the premise of given clause
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
    }
}


