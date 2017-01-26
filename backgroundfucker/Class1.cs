using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Diagnostics;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using WorkingLibrary;
using System.Text.RegularExpressions;
using System.IO;
using HtmlAgilityPack;



namespace backgroundfucker
{
    
    

    public sealed class fuckthattoast:IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("in");
            ApplicationData data = ApplicationData.Current;
            
            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            var arg = details.Argument;


            switch(arg)
            {
                case "ENTER":
                    string input = (string)details.UserInput["message"];
                    string p1 = (string)details.UserInput["p1"];                   
                    input = input.Trim();
                    string result = "";
                    var res = new Agent().GotTransLate(input, p1);
                    var file = Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("mn.txt").AsTask().Result;
                    var mnfile = file.CopyAsync(ApplicationData.Current.LocalFolder, "mn.txt", NameCollisionOption.ReplaceExisting).AsTask().Result;
                    var mnstream = mnfile.OpenStreamForWriteAsync().Result;

                    using (StreamWriter sw = new StreamWriter(mnstream))
                    {
                        if (res == null)
                        {

                            result = "无解释";
                            sw.Write("");

                        }
                        else
                        {

                            Debug.WriteLine(res);
                            sw.Write(res);
                            result = WorkingHelper.SimplifyHtml(res);

                            Debug.WriteLine(result);
                        }

                    }
                            //TODO

                            StorageFile dsfile = ApplicationData.Current.LocalFolder.GetFileAsync("ds.txt").AsTask().Result;
                            using (StreamWriter dssw = new StreamWriter(dsfile.OpenStreamForWriteAsync().Result))
                            {
                                dssw.Write(result);
                            }

                                var rsarray = WorkingHelper.GetandEditString(dsfile);
                           
                            

                        
                    

                    input = "展开通知查看 " + input + " 的翻译";

                    if (rsarray[1] == "S")
                    {
                        StorageFile FSvoc = data.LocalFolder.GetFileAsync("vocpresentEXT.xml").AsTask().GetAwaiter().GetResult();
                        XmlDocument FSvocdoc = XmlDocument.LoadFromFileAsync(FSvoc).AsTask().GetAwaiter().GetResult();
                        var FSgetit = FSvocdoc.GetElementsByTagName("input");
                        var FSgethe = FSvocdoc.GetElementsByTagName("text");
                        FSgethe[0].AppendChild(FSvocdoc.CreateTextNode(input));
                        FSgetit[0].Attributes[1].InnerText = rsarray[0];


                        ToastNotificationManager.History.Clear();
                        ToastNotification FSvocpresent = new ToastNotification(FSvocdoc);

                        FSvocpresent.SuppressPopup = true;
                        ToastNotificationManager.CreateToastNotifier().Show(FSvocpresent);
                        ToastNotificationManager.History.Remove("fuck");
                    }
                    else
                    {

                        StorageFile voc = data.LocalFolder.GetFileAsync("vocpresent.xml").AsTask().GetAwaiter().GetResult();
                        XmlDocument vocdoc = XmlDocument.LoadFromFileAsync(voc).AsTask().GetAwaiter().GetResult();
                        var getit = vocdoc.GetElementsByTagName("input");
                        var gethe = vocdoc.GetElementsByTagName("text");
                        gethe[0].AppendChild(vocdoc.CreateTextNode(input));
                        getit[0].Attributes[1].InnerText = rsarray[0];


                        ToastNotificationManager.History.Clear();
                        ToastNotification vocpresent = new ToastNotification(vocdoc);

                        vocpresent.SuppressPopup = true;
                        ToastNotificationManager.CreateToastNotifier().Show(vocpresent);
                        ToastNotificationManager.History.Remove("fuck");
                    }
                    break;


                case "voc":
                    Debug.WriteLine("show");
                    StorageFile filet = data.LocalFolder.GetFileAsync("toaster.xml").AsTask().GetAwaiter().GetResult();
                    XmlDocument fuckdoc = XmlDocument.LoadFromFileAsync(filet).AsTask().GetAwaiter().GetResult();
                    ToastNotification fuckit = new ToastNotification(fuckdoc);
                    fuckit.SuppressPopup = true;
                    fuckit.Tag = "fuck";
                    StorageFile vocdsfile = ApplicationData.Current.LocalFolder.GetFileAsync("ds.txt").AsTask().Result;
                    using (StreamWriter sw = new StreamWriter(vocdsfile.OpenStreamForWriteAsync().Result))
                    {
                        sw.BaseStream.SetLength(0);
                    }

                        ToastNotificationManager.CreateToastNotifier().Show(fuckit);
                    break;

                case "next":
                    StorageFile nxdsfile= ApplicationData.Current.LocalFolder.GetFileAsync("ds.txt").AsTask().Result;
                    var nxrsarray = WorkingHelper.GetandEditString(nxdsfile);
                    if(nxrsarray[1]=="S")
                    {
                        StorageFile Svoc = data.LocalFolder.GetFileAsync("vocpresentEXT.xml").AsTask().GetAwaiter().GetResult();
                        XmlDocument Svocdoc = XmlDocument.LoadFromFileAsync(Svoc).AsTask().GetAwaiter().GetResult();
                        var Sgetit = Svocdoc.GetElementsByTagName("input");
                        var Sgethe = Svocdoc.GetElementsByTagName("text");
                        Sgethe[0].AppendChild(Svocdoc.CreateTextNode("最后一条翻译："));
                        Sgetit[0].Attributes[1].InnerText = nxrsarray[0];


                        ToastNotificationManager.History.Clear();
                        ToastNotification Svocpresent = new ToastNotification(Svocdoc);

                        Svocpresent.SuppressPopup = true;
                        ToastNotificationManager.CreateToastNotifier().Show(Svocpresent);
                        ToastNotificationManager.History.Remove("fuck");
                    }
                    else
                    {
                        StorageFile NSvoc = data.LocalFolder.GetFileAsync("vocpresent.xml").AsTask().GetAwaiter().GetResult();
                        XmlDocument NSvocdoc = XmlDocument.LoadFromFileAsync(NSvoc).AsTask().GetAwaiter().GetResult();
                        var NSgetit = NSvocdoc.GetElementsByTagName("input");
                        var NSgethe = NSvocdoc.GetElementsByTagName("text");
                        NSgethe[0].AppendChild(NSvocdoc.CreateTextNode("剩余的翻译:"));
                        NSgetit[0].Attributes[1].InnerText = nxrsarray[0];


                        ToastNotificationManager.History.Clear();
                        ToastNotification NSvocpresent = new ToastNotification(NSvocdoc);

                        NSvocpresent.SuppressPopup = true;
                        ToastNotificationManager.CreateToastNotifier().Show(NSvocpresent);
                        ToastNotificationManager.History.Remove("fuck");
                    }
                    break;
                    
            }



            
            

            
            


        }
    }

    public sealed class pushword : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("pushword");

            //每日一词
            string recordtime = (string)ApplicationData.Current.LocalSettings.Values["time"];
            if (DateTimeOffset.Now.Date.ToString() != recordtime)
            {
                string sentence = WorkingHelper.GetSentence();
                if (sentence != null)
                {
                    ApplicationData.Current.LocalSettings.Values["time"] = DateTimeOffset.Now.Date.ToString();

                    XmlDocument toastXml = XmlDocument.LoadFromFileAsync(ApplicationData.Current.LocalFolder.GetFileAsync("pushwordtoast.xml").AsTask().Result).AsTask().Result;
                    XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode("每日一句"));

                    var inputnode = toastXml.GetElementsByTagName("input");
                    inputnode[0].Attributes[1].InnerText = sentence;


                    ToastNotification toast = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(toast);

                }
            }



          


        }
    }







}
