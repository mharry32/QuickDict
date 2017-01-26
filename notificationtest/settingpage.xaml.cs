using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WorkingLibrary;
using System.Diagnostics;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Notifications;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace notificationtest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class settingpage : Page
    {
        
        private StorageFolder datafolder = ApplicationData.Current.LocalFolder;
        public settingpage()
        {
            this.InitializeComponent();

            mdxlistOperation mdl = new mdxlistOperation();
            foreach(string mitem in mdl.Mdxlist)
            {
                flistcb.Items.Add(mitem);
            }
            flistcb.Items.Add("有道翻译");
            
            string flsc = (string)ApplicationData.Current.LocalSettings.Values["flist"];
            
            
            foreach(var st in flsc.Split('*'))
            {
                flist.Items.Add(st);
            }

            
            
            



        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            var xmldocf = datafolder.GetFileAsync("toaster.xml").AsTask().GetAwaiter().GetResult();
            XmlDocument xmldoc = XmlDocument.LoadFromFileAsync(xmldocf).AsTask().GetAwaiter().GetResult();
            var xmltemp = datafolder.GetFileAsync("toasttemplate.xml").AsTask().GetAwaiter().GetResult();
            XmlDocument xmltemc = XmlDocument.LoadFromFileAsync(xmltemp).AsTask().GetAwaiter().GetResult();
            var getinput = xmltemc.GetElementsByTagName("input");
            List<string> flistitem = new List<string>();
            

            string flistsc = "";
            

            foreach(string item in flist.Items)
            {
                var newele = xmltemc.CreateElement("selection");
                newele.SetAttribute("id", item);
                newele.SetAttribute("content", item);
                getinput[1].AppendChild(newele);
                flistsc = flistsc + item + "*";
            }
            

           
            ApplicationData.Current.LocalSettings.Values["flist"] = flistsc.TrimEnd('*');
            


            getinput[1].Attributes.GetNamedItem("defaultInput").NodeValue = (string)flist.Items[0];
            




                xmltemc.SaveToFileAsync(xmldocf).AsTask().GetAwaiter().GetResult();

                
                this.Frame.Navigate(typeof(MainPage));
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            quickdict qd = new quickdict();
            qd.ShowAsync();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

       

        private void flistbt_Click(object sender, RoutedEventArgs e)
        {
            addlogic(flistcb, flist);
        }

       

       



        private void rmlogic(ListBox lb)
        {
            if(lb.SelectedItem!=null)
            {
                
                if(lb.Items.Count==1)
                {
                }
                else
                {
                    lb.Items.Remove(lb.SelectedItem);
                }
            }
        }

        private void addlogic(ComboBox cb,ListBox lb)
        {
            if(cb.SelectedItem!=null)
            {
                bool existed = false;
                foreach(string st in lb.Items)
                {
                    if ((string)cb.SelectedItem == st)
                        existed = true;
                }
                if(existed==true)
                {
                    MessageDialog md = new MessageDialog("项目已存在！");
                    md.ShowAsync();
                }
                else
                {
                    if(lb.Items.Count>=5)
                    {
                        MessageDialog md = new MessageDialog("由于系统限制，通知栏项目数不能多于五个。");
                        md.ShowAsync();
                    }
                    else
                    {
                        lb.Items.Add((string)cb.SelectedItem);
                        
                    }
                }
            }
        }

        private void flistrm_Click(object sender, RoutedEventArgs e)
        {
            rmlogic(flist);
        }

        
    }
}
