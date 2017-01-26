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
using Windows.UI.Xaml.Media.Animation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace notificationtest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Book : Page
    {
        public Book()
        {
            this.InitializeComponent();
            BookOperation bo = new BookOperation();
            foreach(string bs in bo.Booklist)
            {
                voclist.Items.Add(bs);
            }
            

        }

        private void check_Click(object sender, RoutedEventArgs e)
        {
            if(voclist.SelectedItem!=null)
            {
                string selected = (string)voclist.SelectedItem;
                this.Frame.Navigate(typeof(MainPage), selected);
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if(voclist.SelectedItem!=null)
            {
                string selected = (string)voclist.SelectedItem;
                BookOperation bo = new BookOperation();
                bo.RemoveFromBook(selected);
                voclist.Items.Remove(selected);
                voclist.UpdateLayout();
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            
            this.Frame.GoBack();
        }
    }
}
