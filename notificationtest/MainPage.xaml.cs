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
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.ApplicationModel.Background;
using Windows.System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using System.Diagnostics;
using WorkingLibrary;
using Windows.System.Threading;
using Windows.Data.Html;


//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace notificationtest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
       private StorageFolder datafolder = ApplicationData.Current.LocalFolder;
       
        public MainPage()
        {
            this.InitializeComponent();
            mdxlistOperation mdxlist = new mdxlistOperation();
            foreach(string mitem in mdxlist.Mdxlist)
            {
                chosemdx.Items.Add(mitem);
            }
            chosemdx.Items.Add("有道翻译");

            chosemdx.SelectedIndex = 0;
            
        }

        

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(settingpage));
        }

        

        private void search_Click(object sender, RoutedEventArgs e)
        {
            this.BottomAppBar.IsEnabled = false;
            chosemdx.IsEnabled = false;
            inputbx.IsEnabled = false;
            pb.Visibility = Visibility.Visible;
            string present = "";

            string p =(string) chosemdx.SelectedItem;
            string inputtext = inputbx.Text.Trim();
          var work=ThreadPool.RunAsync((c) =>
            {


                var res = new Agent().GotTransLate(inputtext, p);

                if(res==null)
                {
                    present = null;
                }
                else
                {
                    present = res;
                    
                }
                    
                
            });
            work.Completed += (a, b) =>
            {
               var nonw=this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {
                    mnv.NavigateToString("");
                    

                    pb.Visibility = Visibility.Collapsed;

                    this.BottomAppBar.IsEnabled = true;
                    chosemdx.IsEnabled = true;
                    inputbx.IsEnabled = true;
                    
                   if(present==null)
                    {

                        mnv.NavigateToString("无释义");
                        
                        
                    }
                   else
                    {
                        mnv.NavigateToString(present);
                        
                       
                        
                    }



                });
            };
            
            
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
           
            this.Frame.Navigate(typeof(mdximport));
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("shit");
            string pr = e.Parameter as string;
            if(pr!=null)
            {
                Debug.WriteLine("pr!=null");
                if(pr=="(voc)")
                {
                    StorageFile vocdsfile = ApplicationData.Current.LocalFolder.GetFileAsync("ds.txt").AsTask().Result;
                    using (StreamWriter sw = new StreamWriter(vocdsfile.OpenStreamForWriteAsync().Result))
                    {
                        sw.BaseStream.SetLength(0);
                    }
                    var file = ApplicationData.Current.LocalFolder.GetFileAsync("mn.txt").AsTask().Result;
                    var textstream = file.OpenStreamForReadAsync().Result;
                    using (StreamReader sr = new StreamReader(textstream))
                    {
                        string res = sr.ReadToEnd();
                        Debug.WriteLine(res);
                        mnv.NavigateToString(res);
                        
                    }
                }
            }
        }

       
    }
}
