using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Text.RegularExpressions;
using DemoMdictReader;
using System.Runtime.Serialization.Json;
using Windows.Data.Html;
using System.Diagnostics;

namespace WorkingLibrary
{
    public class mdxTranslate : ITranslate
    {
        public string GetTranslate(string querytext,string p)
        {
           
            
                    try
                    {
                        var file = ApplicationData.Current.LocalFolder.GetFileAsync(p+".mdx").AsTask().Result;
                        var iblfile = ApplicationData.Current.LocalFolder.GetFileAsync(p + "I.json").AsTask().Result;
                        var rbifile = ApplicationData.Current.LocalFolder.GetFileAsync(p + "R.json").AsTask().Result;
                if (file == null || iblfile == null || rbifile == null)
                    return null;
                else
                {
                    string jsonst = "";
                    var ibl = iblfile.OpenStreamForReadAsync().Result;
                    using (StreamReader sr = new StreamReader(ibl))
                    {
                        jsonst = sr.ReadToEnd();
                    }



                    string mtoken = @"{""Value1"":(.*?),""Value2"":(.*?),""Value3"":""" + querytext + @"""}";
                    var mc = Regex.Match(jsonst, mtoken, RegexOptions.RightToLeft);

                    if (mc.Groups.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        Debug.WriteLine(mc.Groups[1].Value);
                        Debug.WriteLine(mc.Groups[2].Value);
                        long v1 = long.Parse(mc.Groups[1].Value);
                        long v2 = long.Parse(mc.Groups[2].Value);
                        Mdx dict = new Mdx(file.Path);
                        Debug.WriteLine(dict.IsInit);
                        Debug.WriteLine(file.Path);
                        dict.RecordListStartPosition = (long)ApplicationData.Current.LocalSettings.Values[p + "RL"];
                        Debug.WriteLine(dict.RecordListStartPosition);
                        var rbi = rbifile.OpenStreamForReadAsync().Result;
                        DataContractJsonSerializer rbis = new DataContractJsonSerializer(typeof(List<MdictHelper.Tuple<long, long>>));
                        dict.RecordBlockInfoList = (List<MdictHelper.Tuple<long, long>>)rbis.ReadObject(rbi);


                        var res = dict.GetKeyValue(new MdictHelper.Tuple<long, long, string>(v1, v2, querytext));
                        Debug.WriteLine(res);
                        if (res == null)
                        {
                            return null;
                        }
                        else
                        {
                            
                            
                            return res;
                        }
                    }

                }
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                        //记录日志
                    }

                    
                    
                    return null;
                }

               
            
        


       
    }
}
