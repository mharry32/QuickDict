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
using DemoMdictReader;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Runtime.Serialization.Json;
using Windows.System.Threading;
using Windows.UI.Popups;
using System.Diagnostics;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace notificationtest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class mdximport : Page
    {
        public mdximport()
        {
            this.InitializeComponent();
            mdxlistOperation mdxlist = new mdxlistOperation();
            foreach(string ltem in mdxlist.Mdxlist)
            {
                voclist.Items.Add(ltem);
            }

        }

        private async void importbt_Click(object sender, RoutedEventArgs e)
        {
            
            FileOpenPicker fp = new FileOpenPicker();
            fp.FileTypeFilter.Add(".mdx");
            var file = await fp.PickSingleFileAsync();
            if(file!=null)
            {
                pgpanel.Visibility = Visibility.Visible;
                this.IsEnabled = false;
                this.BottomAppBar.IsEnabled = false;
                
                string name = file.DisplayName;


            var state=ThreadPool.RunAsync((wa) => {
                    var dict = file.CopyAsync(ApplicationData.Current.LocalFolder,name+".mdx", NameCollisionOption.ReplaceExisting).AsTask().Result;
                bool isok = importfile(dict.Path,name);
                
                if(isok==true)
                {
                    var a = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                    {
                        pgpanel.Visibility = Visibility.Collapsed;
                        this.IsEnabled = true;
                        this.BottomAppBar.IsEnabled = true;
                        voclist.Items.Clear();
                        mdxlistOperation mdxlist = new mdxlistOperation();
                        foreach(string it in mdxlist.Mdxlist)
                        {
                            voclist.Items.Add(it);
                        }
                        voclist.UpdateLayout();


                    });
                }
                else
                {
                    var a = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                    {
                        pgpanel.Visibility = Visibility.Collapsed;
                        this.IsEnabled = true;
                        this.BottomAppBar.IsEnabled = true;
                        
                        MessageDialog md = new MessageDialog("词典格式可能需要转换，详情请点击此页下方的导入帮助", "导入失败");
                        md.ShowAsync();

                    });
                    
                }

                });
                
            }
           
        }

       

        

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (voclist.SelectedItem != null)
            {
                pgpanel.Visibility = Visibility.Visible;
                this.IsEnabled = false;
                this.BottomAppBar.IsEnabled = false;
                string name = (string)voclist.SelectedItem;

                var res = ThreadPool.RunAsync((a) =>
                {

                    ApplicationData.Current.LocalSettings.Values[name + "RL"] = (long)0;
                    var idxfile = ApplicationData.Current.LocalFolder.GetFileAsync(name + "I.json").AsTask().Result;
                    var recordfile = ApplicationData.Current.LocalFolder.GetFileAsync(name + "R.json").AsTask().Result;

                    var idxd = idxfile.DeleteAsync().AsTask();
                    var recordd = recordfile.DeleteAsync().AsTask();

                    idxd.Wait();
                    recordd.Wait();

                    mdxlistOperation mdxlist = new mdxlistOperation();
                    mdxlist.RemoveFromBook(name);

                    var re = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                    {
                        pgpanel.Visibility = Visibility.Collapsed;
                        this.IsEnabled = true;
                        this.BottomAppBar.IsEnabled = true;
                        voclist.Items.Clear();
                        foreach (string it in mdxlist.Mdxlist)
                        {
                            voclist.Items.Add(it);
                        }
                        voclist.UpdateLayout();
                        MessageDialog md = new MessageDialog("删除成功。");
                        md.ShowAsync();

                    });





                });

            }

            

        }


        private bool importfile(string path,string name)
        {
            Mdx dict = new Mdx(path);
            if (dict.GetKeys() == false)
                return false;
            else
            {

                //开始
                mdxlistOperation mdxlist = new mdxlistOperation();
                mdxlist.AddtoBook(name);
                dict.GetRecordBlocksInfo();
                
                ApplicationData.Current.LocalSettings.Values[name + "RL"] = dict.RecordListStartPosition;

                var idxfile = ApplicationData.Current.LocalFolder.CreateFileAsync(name + "I.json", CreationCollisionOption.ReplaceExisting).AsTask();
                var recordfile = ApplicationData.Current.LocalFolder.CreateFileAsync(name + "R.json", CreationCollisionOption.ReplaceExisting).AsTask();

                idxfile.Wait();
                recordfile.Wait();

                
                var idx = idxfile.Result;
                var record = recordfile.Result;


                
                
                var idxstream = idx.OpenStreamForWriteAsync().Result;
                
                var recordstream = record.OpenStreamForWriteAsync().Result;

                DataContractJsonSerializer idxsl = new DataContractJsonSerializer(dict.IdxBlockInfoList.GetType());
                DataContractJsonSerializer recordsl = new DataContractJsonSerializer(dict.RecordBlockInfoList.GetType());

                idxsl.WriteObject(idxstream, dict.IdxBlockInfoList);
                recordsl.WriteObject(recordstream, dict.RecordBlockInfoList);

                idxstream.Dispose();
                recordstream.Dispose();

                
                return true;
            }
        }

        private void gotohelp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(help));
        }
    }
}
