using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingLibrary
{
    public class Agent : IAgent
    {
        public string GotTransLate(string querytext, string p = null)
        {
            if (p == "有道翻译")
                return new YouDaoTranslate().GetTranslate(querytext, p);
            else
            {
                return new mdxTranslate().GetTranslate(querytext, p);
            }
        }
    }
}
