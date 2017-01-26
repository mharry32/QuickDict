using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WorkingLibrary
{
   public class BookOperation
    {
        private List<string> booklist = new List<string>();

        public List<string> Booklist { get { return booklist; } }
        public BookOperation()
        {
            var bookstring =(string)ApplicationData.Current.LocalSettings.Values["bookstring"];
            foreach (string bs in bookstring.Split('/'))
            {
                if(bs!="")
                booklist.Add(bs);
            }
        }

        public void AddtoBook(string text)
        {
            if(booklist.Contains(text)==false)
            {
                booklist.Add(text);
            }
            SaveList();
        }

        public void RemoveFromBook(string text)
        {
            booklist.Remove(text);
            SaveList();
        }

        private void SaveList()
        {
            string temp = "";
            foreach(string bs in booklist)
            {
                temp = temp + bs + "/";
            }
            ApplicationData.Current.LocalSettings.Values["bookstring"] = temp;
        }

    }
}
