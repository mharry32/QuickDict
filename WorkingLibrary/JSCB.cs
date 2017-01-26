using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Diagnostics;
namespace WorkingLibrary
{
    public class JSCB : ITranslate
    {
        public string[] GetTranslate(string p1, string p2, string querytext)
        {
            Debug.WriteLine("here");
            if(p1=="英"&&p2=="汉")
            {
                HttpClient a = new HttpClient();

                a.DefaultRequestHeaders.Add("UserAgent", "Openwave/ UCWEB7.0.2.37/28/999");
                try
                {

                    var b = a.GetStringAsync(Uri.EscapeUriString("http://dict-co.iciba.com/api/dictionary.php?w=" + querytext + "&type=xml&key=D932A18B20CD6154991CDDD84734F084"));

                    string res = null;

                    var bb = b.Wait(10000);
                    if (bb == true)
                    {
                        
                        var rex = Regex.Match(b.Result, "<acceptation>(.*?)</acceptation>", RegexOptions.Singleline | RegexOptions.Multiline);
                        res = rex.Groups[1].Value.Trim();
                        Debug.WriteLine(res);
                        string[] result = new string[2];
                        result[0] = "金山词霸";
                        result[1] = res;
                        
                        return result;

                       
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
            else
            {
                return null;
            }
        }
    }
}
