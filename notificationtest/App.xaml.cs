using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Popups;

namespace notificationtest
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += App_UnhandledException;
            
            this.Resuming += (a, b) => { ToastNotificationManager.History.Clear(); };
            StorageFile ttp = Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("toasttemplate.xml").AsTask().GetAwaiter().GetResult();
            ttp.CopyAsync(ApplicationData.Current.LocalFolder, "toasttemplate.xml", NameCollisionOption.ReplaceExisting).AsTask().GetAwaiter().GetResult();
            if(ApplicationData.Current.LocalSettings.Values.ContainsKey("flist")==false)
            ApplicationData.Current.LocalSettings.Values["flist"] = "有道翻译";
                        
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("sc") == false)
                ApplicationData.Current.LocalSettings.Values["sc"] = "";
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("bookstring") == false)
                ApplicationData.Current.LocalSettings.Values["bookstring"] = "";
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("mdxlist") == false)
                ApplicationData.Current.LocalSettings.Values["mdxlist"] = "";
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("tempvoc") == false)
                ApplicationData.Current.LocalSettings.Values["tempvoc"] = "";
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey("time") == false)
                ApplicationData.Current.LocalSettings.Values["time"] = "";
            if (ApplicationData.Current.LocalFolder.TryGetItemAsync("mn.txt").AsTask().Result == null)
            {
               var file=Package.Current.InstalledLocation.GetFileAsync("mn.txt").AsTask().Result;
              var res=file.CopyAsync(ApplicationData.Current.LocalFolder, "mn.txt").AsTask().Result;
            }
            if (ApplicationData.Current.LocalFolder.TryGetItemAsync("ds.txt").AsTask().Result == null)
            {
                var file = ApplicationData.Current.LocalFolder.CreateFileAsync("ds.txt").AsTask().Result;
            }



        }

        private async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageDialog md= new MessageDialog(e.Message + Environment.NewLine + "如果遇到了LayoutCycle问题，请将系统的分辨率调小一些", "出现错误");
            md.Commands.Add(new UICommand("去设置",async (a) => { await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:screenrotation")); }));
            md.Commands.Add(new UICommand("关闭"));
            md.ShowAsync();
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        /// 

        protected override void OnActivated(IActivatedEventArgs args)
        {

            if(args.Kind==ActivationKind.ToastNotification)
            {
                var toastargs = args as ToastNotificationActivatedEventArgs;
                var arguments = toastargs.Argument;
                if(arguments=="(voc)")
                {
                    Frame root = Window.Current.Content as Frame;
                    Debug.WriteLine("v");

                    root = Window.Current.Content as Frame;
                    if (root == null) { root = new Frame(); root.ContentTransitions = new TransitionCollection();
                        NavigationThemeTransition nt = new NavigationThemeTransition();
                        nt.DefaultNavigationTransitionInfo = new SlideNavigationTransitionInfo();
                        root.ContentTransitions.Add(nt); Window.Current.Content = root; }
                    /*if (root.Content == null) { */Debug.WriteLine("rot"); root.Navigate(typeof(MainPage),"(voc)"); 
                    Window.Current.Activate();
                }
                else
                {
                    Frame root = Window.Current.Content as Frame;
                    

                    root = Window.Current.Content as Frame;
                    if (root == null) { root = new Frame(); root.ContentTransitions = new TransitionCollection();
                        NavigationThemeTransition nt = new NavigationThemeTransition();
                        nt.DefaultNavigationTransitionInfo = new SlideNavigationTransitionInfo();
                        root.ContentTransitions.Add(nt); Window.Current.Content = root; }
                    if (root.Content == null) { root.Navigate(typeof(MainPage)); }
                    Window.Current.Activate();
                }
            }

            //判断是哪个toast激活的，向主页传不同的参数
           
            
            
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            
            Frame rootFrame = Window.Current.Content as Frame;

            ToastNotificationManager.History.Clear();

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();
                
                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.ContentTransitions = new TransitionCollection();
                NavigationThemeTransition nt = new NavigationThemeTransition();
                nt.DefaultNavigationTransitionInfo = new SlideNavigationTransitionInfo();
                
                
                rootFrame.ContentTransitions.Add(nt);
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置



                

                var container = ApplicationData.Current.LocalSettings;
                
                


                bool toaster = false;
                
                bool vocpresentname = false;

                bool vocext = false;

                bool pushwordtoast = false;
               
                var result = ApplicationData.Current.LocalFolder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery).AsTask().GetAwaiter().GetResult();
                foreach(var filename in result)
                {

                    if (filename.Name == "toaster.xml")
                        toaster = true;
                    else if (filename.Name == "vocpresent.xml")
                        vocpresentname = true;
                    else if (filename.Name == "vocpresentEXT.xml")
                        vocext = true;
                    else if (filename.Name == "pushwordtoast.xml")
                        pushwordtoast = true;
                    

                }
                if(toaster==false)
                {
                    StorageFile dbfile = Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("toaster.xml").AsTask().GetAwaiter().GetResult();
                    dbfile.CopyAsync(ApplicationData.Current.LocalFolder, "toaster.xml", NameCollisionOption.ReplaceExisting).AsTask().GetAwaiter().GetResult();
                }
                
                if(vocpresentname==false)
                {
                    StorageFile vocpresent = Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("vocpresent.xml").AsTask().GetAwaiter().GetResult();
                    vocpresent.CopyAsync(ApplicationData.Current.LocalFolder, "vocpresent.xml", NameCollisionOption.ReplaceExisting).AsTask().GetAwaiter().GetResult();
                }

                if(vocext==false)
                {
                    var voce = Package.Current.InstalledLocation.GetFileAsync("vocpresentEXT.xml").AsTask().Result;
                   var temp=voce.CopyAsync(ApplicationData.Current.LocalFolder, "vocpresentEXT.xml", NameCollisionOption.ReplaceExisting).AsTask().Result;
                }
                if(pushwordtoast==false)
                {
                    var voce = Package.Current.InstalledLocation.GetFileAsync("pushwordtoast.xml").AsTask().Result;
                    var temp = voce.CopyAsync(ApplicationData.Current.LocalFolder, "pushwordtoast.xml", NameCollisionOption.ReplaceExisting).AsTask().Result;
                }
                
               
                
                
                var fuckname = "fucktoast";
                var pushwordname = "pushword";
                
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == fuckname)
                    {

                        task.Value.Unregister(false);
                        break;
                    }
                }


                foreach(var task in BackgroundTaskRegistration.AllTasks)
                {
                    if(task.Value.Name == pushwordname)
                    {
                        task.Value.Unregister(false);
                        break;
                    }
                }

                
                BackgroundExecutionManager.RemoveAccess();
                BackgroundExecutionManager.RequestAccessAsync();
                

                    var builder = new BackgroundTaskBuilder();
                    builder.Name = fuckname;
                    builder.TaskEntryPoint = "backgroundfucker.fuckthattoast";
                    builder.SetTrigger(new ToastNotificationActionTrigger());
                    builder.Register();


                var pushbuilder = new BackgroundTaskBuilder();
                pushbuilder.Name = pushwordname;
                pushbuilder.TaskEntryPoint = "backgroundfucker.pushword";
                pushbuilder.SetTrigger(new TimeTrigger(60, false));
                pushbuilder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                pushbuilder.Register();



                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name==fuckname)
                    {

                        Debug.WriteLine("ok");
                    }
                }

                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == pushwordname)
                    {

                        Debug.WriteLine("ok");
                    }
                }

                // 参数
                rootFrame.Navigate(typeof(MainPage));
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动

            var data = ApplicationData.Current;
                StorageFile file = data.LocalFolder.GetFileAsync("toaster.xml").AsTask().GetAwaiter().GetResult();
                XmlDocument fuckdoc = XmlDocument.LoadFromFileAsync(file).AsTask().GetAwaiter().GetResult();
                Debug.WriteLine(fuckdoc.GetXml());
                ToastNotification fuckit = new ToastNotification(fuckdoc);
                fuckit.SuppressPopup = true;
                fuckit.Tag = "fuck";
                ToastNotificationManager.CreateToastNotifier().Show(fuckit);
            
            
            
            deferral.Complete();
        }
    }
}
