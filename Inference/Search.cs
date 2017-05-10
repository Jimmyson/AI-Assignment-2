using System.Collections;

namespace Inference
{
    abstract class Search
    {
        abstract List<string> agenda;

        abstract List<string> facts;
        abstract List<string> clauses;
        abstract List<string> entailed;
        abstract List<int> count;

        abstract void Process();
    }
}