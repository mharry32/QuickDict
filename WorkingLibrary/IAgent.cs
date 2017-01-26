using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingLibrary
{
   public interface IAgent
    {
        string GotTransLate(string querytext, string p = null);
    }
}
