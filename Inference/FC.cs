using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inference
{
    /*
     * Read all root nodes, and prints full node path to destintion if found.
     * eg: a, b, p1, p2, p3, d
     */
    class FC
    {
        override process (kb,q )
            //kb = tell
            //q = ask
        {
            count // A table indexed by clause, initially number of premises
            inferred // A table indexed by symbol, initiated with false
            agenda // a list of symbols, initially known to be true

            while (agenda.!empty())
                {
                    p = pop agenda
                    if (!inferred(p))
                {
                    for (each kb c in premise p appears)
                    {
                        if count[c] =
                            if Head[c] = Queryable then return true
                                push(head[c].agenda)
                    }
                            }
                }

            return false;
        }
    }
}

abstract List<string> agenda;

abstract List<string> facts;
abstract List<string> clauses;
abstract List<string> entailed;
abstract List<int> count;



