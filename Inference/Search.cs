using System.Collections.Generic;



namespace Inference
{
    abstract class Search
    {
        List<string> agenda;
        List<string> facts;
        List<string> clauses;
        List<string> entailed;
        List<int> count;

        public List<string> Agenda { get { return agenda; }  }
        public List<string> Facts { get { return facts; } }
        public List<string> Clauses { get { return clauses; } }
        public List<string> Entailed { get { return entailed; } }
        public List<int> Count { get { return count; } }
        abstract public void Process();
    }
}