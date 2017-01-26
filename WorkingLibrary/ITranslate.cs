using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkingLibrary
{
   public interface ITranslate
    {
        string GetTranslate(string querytext,string p=null);
    }
}
