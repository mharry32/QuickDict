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
using Windows.System;
using Edi.UWP.Helpers;
using Windows.Security.ExchangeActiveSyncProvisioning;
using System.Threading.Tasks;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace notificationtest
{
    public sealed partial class quickdict : ContentDialog
    {
        public quickdict()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        

       

        private void emailme_Click(object sender, RoutedEventArgs e)
        {
            pgring.IsActive = true;
            sendmail.IsEnabled = false;
            string rv = reviewbox.Text;
            var tk = ReportError(rv);
            tk.ContinueWith((t) => { var a = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => { pgring.IsActive = false; sendmail.IsEnabled = true; }); });
        }

        private async void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9nblggh4rk0j"));
        }

        public static async Task ReportError(string msg = null, string pageSummary = "N/A", bool includeDeviceInfo =
true)
        {
            var deviceInfo = new EasClientDeviceInformation();

            string subject = "QuickDict Windows Universal应用错误报告";
            string body = $"问题描述：{msg}  " +
                          $"（程序版本：{Utils.GetAppVersion()}, " +
                          
                          $"页面摘要：{pageSummary}";

            if (includeDeviceInfo)
            {
                body += $", 设备名：{deviceInfo.FriendlyName}, " +
                          $"操作系统：{deviceInfo.OperatingSystem}, " +
                          $"SKU：{deviceInfo.SystemSku}, " +
                          $"产品名称：{deviceInfo.SystemProductName}, " +
                          $"制造商：{deviceInfo.SystemManufacturer}, " +
                          $"固件版本：{deviceInfo.SystemFirmwareVersion}, " +
                          $"硬件版本：{deviceInfo.SystemHardwareVersion}）";
            }
            else
            {
                body += ")";
            }

            string to = "nokia18819479253@outlook.com";
            await Tasks.OpenEmailComposeAsync(to, subject, body);
        }
    }
}
