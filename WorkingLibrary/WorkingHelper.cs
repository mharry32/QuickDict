using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace WorkingLibrary
{
   public static class WorkingHelper
    {
        public static string SimplifyHtml(string src)
        {
            string rt = "";
            HtmlDocument hd = new HtmlDocument();
            hd.LoadHtml(src);
            var nodes = hd.DocumentNode.ChildNodes;
            foreach (var node in nodes)
            {
                if(node.InnerText.StartsWith("body{")==false&&node.InnerText.StartsWith("<!")==false)
                {
                    rt += node.InnerText;
                }
            }
            return rt;
        }

        public static string[] GetandEditString(StorageFile target)
        {
            string temp = "";
            string returntemp = "";
            string[] returnarray = new string[2];
            using (StreamReader sr = new StreamReader(target.OpenStreamForReadAsync().Result))
            {
                temp = sr.ReadToEnd();
            }
            if(temp.Length>200)
            {
                returntemp = temp.Substring(0, 200);
                temp = temp.Remove(0, 200);
                using (StreamWriter sw = new StreamWriter(target.OpenStreamForWriteAsync().Result))
                {
                    sw.BaseStream.SetLength(0);
                    sw.Write(temp);
                }
                returnarray[0] = returntemp;
                returnarray[1] = "NS";

                return returnarray;
            }
            else
            {
                returnarray[0] = temp;
                returnarray[1] = "S";

                using (StreamWriter sw = new StreamWriter(target.OpenStreamForWriteAsync().Result))
                {
                    sw.BaseStream.SetLength(0);
                    
                }
                return returnarray;
            }

        }

        public static Queue<string> DivideString(string resultst)
        {
            Queue<string> resultQueue = new Queue<string>();
            while(resultst.Length>200)
            {
                string temp = resultst.Substring(0, 200);
               resultst=resultst.Remove(0, 200);
                resultQueue.Enqueue(temp);
            }
            resultQueue.Enqueue(resultst);

            
            return resultQueue;

        }


        public static string GetSentence()
        {

            HttpClient a = new HttpClient();

            a.DefaultRequestHeaders.Add("UserAgent", "Openwave/ UCWEB7.0.2.37/28/999");
            try
            {

                var b = a.GetStringAsync(Uri.EscapeUriString("http://open.iciba.com/ds_open.php?id=51257&name=QuickDict&auth=126D889A333EF40FF0ABD11EBD2C6962"));

                string res = null;

                var bb = b.Wait(10000);
                if (bb == true)
                {
                    
                    var re = Regex.Match(b.Result, @"title=""(.*?)""", RegexOptions.Multiline);
                    res = re.Groups[1].Value;

                    return res;


                }
                else
                {
                    return null;
                }



            }
            catch
            {
                return null;
            }
        }

    }
}
