using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WorkingLibrary
{
   public class mdxlistOperation
    {
        private List<string> mdxlist = new List<string>();

        public List<string> Mdxlist { get { return mdxlist; } }
        public mdxlistOperation()
        {
            var mdxstring = (string)ApplicationData.Current.LocalSettings.Values["mdxlist"];
            foreach (string bs in mdxstring.Split('*'))
            {
                if (bs != "")
                    mdxlist.Add(bs);
            }
        }

        public void AddtoBook(string text)
        {
            if (mdxlist.Contains(text) == false)
            {
                mdxlist.Add(text);
            }
            SaveList();
        }

        public void RemoveFromBook(string text)
        {
            mdxlist.Remove(text);
            SaveList();
        }

        private void SaveList()
        {
            string temp = "";
            foreach (string bs in mdxlist)
            {
                temp = temp + bs + "*";
            }
            ApplicationData.Current.LocalSettings.Values["mdxlist"] = temp;
        }
    }
}
