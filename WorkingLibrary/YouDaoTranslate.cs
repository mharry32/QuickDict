using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WorkingLibrary
{
    public class YouDaoTranslate : ITranslate
    {
        public string GetTranslate(string querytext,string p)
        {
            try
            {
                
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("UserAgent", "Openwave/ UCWEB7.0.2.37/28/999");
                var tasker = client.GetStringAsync(new Uri(Uri.EscapeUriString("http://fanyi.youdao.com/openapi.do?keyfrom=QuickDict&key=990705885&type=data&doctype=text&version=1.0&q=" + querytext)));
                var taskrs = tasker;

                var taskdetect = tasker.Wait(10000);
                if (taskdetect == true)
                {
                    var res = taskrs.GetAwaiter().GetResult();
                    var c = res.Split('\n');

                    if (c[0] == "errorCode=0")
                    {
                        var ca = c[1].Split('=');



                        return ca[1];
                    }
                    else
                    {
                        return null;
                    }
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
