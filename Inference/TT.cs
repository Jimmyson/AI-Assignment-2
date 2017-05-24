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

        private bool[][] theModels;
        private List<String> AllSymbolAndStatment;
        private int TheNumberOfSymbol;

        public TT(string tell, string ask)
        {
            BuildKB(tell);
            Process(ask);   
        }

        public override void Process(string ask)
        {
            if (Fetch(ask))
            {
                int count = 0;
               
                for (int i = 0; i < (Math.Pow(2, AllSymbolAndStatment.Count)); i++)
                {
                    if (theModels[i][FindPosition(ask)] && theModels[i][AllSymbolAndStatment.Count])
                    // find if the KB is TRUE and the ask is TRUE in the same model
                        count += 1;
                }

                if (count > 0)
                    Console.WriteLine("YES: ", count);
                else
                    Console.WriteLine("NO");
            }
            else
                Console.WriteLine("NO");
        }

        public override bool Algorithm()
        {
            if (Agenda.Count > 0) { 
                // add the True and False into the whole TT map
                int rows = (int)Math.Pow(2, AllSymbolAndStatment.Count);
                for (int i = TheNumberOfSymbol; i < AllSymbolAndStatment.Count; i++)
                // this step is to add the T/F into the position for statement 
                {
                    for (int j = 0; j < (Math.Pow(2, AllSymbolAndStatment.Count)); j++)
                    // this step is pass the line number for fetch and find position
                    {
                        theModels[j][i] = CheckStatement(AllSymbolAndStatment[i], j);
                        /*
                        if (CheckStatement(AllSymbolAndStatment[i], j)) {
                            //TRUE
                            theModels[j][i] = 1;
                        }
                        else
                        {
                            //FALSE
                            theModels[j][i] = 0;
                        }*/
                    }
                }

                // add the True or False for the KB result 
                for (int j = 0; j < (Math.Pow(2, AllSymbolAndStatment.Count)); j++) 
                // this step is pass the line number for fetch and find position
                {
                    theModels[j][AllSymbolAndStatment.Count] = FindKB(j);
                    /*if (FindKB(j))
                    {
                        //TRUE
                        theModels[j][AllSymbolAndStatment.Count] = 1;
                    }
                    else
                    {
                        //FALSE
                        theModels[j][AllSymbolAndStatment.Count] = 0;
                    }*/
                }

                return true;
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
                else  //If false, add to agenda
                    Agenda.Add(sentences[i]);
            }

            //add every symbol into the agenda including the deuplicated
            //the deuplication will be remove in the next step
            for (int i = 0; i < Clauses.Count; i++) {
                Clauses[0].Replace("=>", " ");
                Clauses[0].Replace("&", " ");
                string[] temp = Clauses[0].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                {
                    Agenda.Add(temp[j]);
                }
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

            return (conjucts.Count == 1) ? premise.Equals(p) : conjucts.Contains(p);

            // Check the conjunctions in the clause.
            /*if (conjucts.Count == 1)
            {
                // If only 1, check the conjuct against p
                return premise.Equals(p);
            }
            else
            {
                // If multiple, check each of them against p
                return conjucts.Contains(p);
            }*/
        }

        // key section to add the result of statement
        public bool CheckStatement(string statement,int thelinenumber)
        {
            // currently can handle a&b=>c ; a=>c;
            string premise = Regex.Split(statement, "=>")[0];
            string aftermise = Regex.Split(statement, "=>")[1];

            if (!statement.Contains("&"))
                return (FindPosition(premise) == 0 && FindPosition(aftermise) == 1) ? false : true;
                /*if (FindPosition(premise) == 0 && FindPosition(aftermise) == 1) { return false; }
                else { return true; }*/
            else {
                //string premise = Regex.Split(statement, "=>")[0];
                //string aftermise = Regex.Split(statement, "=>")[1];
                string[] temp = premise.Split('&');
                
                if (FindPosition(temp[0]) == 0 && FindPosition(temp[1]) == 1)
                    return (FindPosition(aftermise) == 1) ? false : true;
                    //if (FindPosition(aftermise) == 1) { return false; } else { return true; }
                else
                    return true;
            }
        }

        public bool Fetch(string symbol) {

            for (int i = 0; i < AllSymbolAndStatment.Count; i++) {
                if (AllSymbolAndStatment[i] == symbol)
                    return true;
            }

            return false;
        }

        public int FindPosition(string symbol) {
            if (Fetch(symbol))
            {
                for (int i = 0; i < AllSymbolAndStatment.Count; i++)
                {
                    if (AllSymbolAndStatment[i] == symbol)
                        return i;
                }
            }

            return -1;
        }

        public void BuildTheModels(List<string> agenda)
        {
            // clear the deuplicated agenda
            for (int i = 0; i < agenda.Count; i++)
            {
                if (AllSymbolAndStatment.Contains(agenda[i]))
                    break;
                else
                    AllSymbolAndStatment.Add(agenda[i]);
            }

            TheNumberOfSymbol = AllSymbolAndStatment.Count;
            //set the init()
            int rows = (int)Math.Pow(2, AllSymbolAndStatment.Count);
            for (int i = 0; i < AllSymbolAndStatment.Count; i++)
            {
                for (int j = 0; j < (Math.Pow(2, AllSymbolAndStatment.Count)); j++)
                {
                    theModels[j][i] = (j < Math.Pow(2, AllSymbolAndStatment.Count - 1)) ? false : true;
                    /*if (j < Math.Pow(2, AllSymbolAndStatment.Count - 1))
                    {
                        //FALSE
                        theModels[j][i] = 0;
                    }
                    else
                    {
                        //TRUE
                        theModels[j][i] = 1;
                    }*/
                }
            }
            
        }

        public void BuildWholeTT(List<string> clauses) {
            // add all the statement into the symbol
            for (int i = 0; i < Clauses.Count; i++)
            {
                if (AllSymbolAndStatment.Contains(Clauses[i]))
                    break;
                else
                    AllSymbolAndStatment.Add(Clauses[i]);
            }

            if (Algorithm())
                Console.WriteLine("The whole TT map is build");
        }

        // check if all the stetement are true, if it is then return true
        public bool FindKB(int thelinenumber)
        {
            int TheSumOfStatement = 0;
            for (int i = TheNumberOfSymbol; i < AllSymbolAndStatment.Count; i++)
            {
                if (theModels[thelinenumber][i] == true)
                    TheSumOfStatement += 1; // if all the statement are true, the Sum of Statement should be the same as the number of the statement
            }
            return (TheSumOfStatement == (AllSymbolAndStatment.Count - TheNumberOfSymbol));
            /*if (TheSumOfStatement == (AllSymbolAndStatment.Count - TheNumberOfSymbol))
            { return true; }
            else { return false; }*/

        }
    }
}
