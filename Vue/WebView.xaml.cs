using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CivLaucherDotNetCore.Vue
{
    /// <summary>
    /// Logique d'interaction pour webView.xaml
    /// </summary>
    /// 
    public partial class WebView : UserControl
    {
        public WebView2 webViewV;
        public WebView()
        {
            InitializeComponent();
            //this.webViewV = webView;
            //iniWebView();
            //webView.Source = new Uri("https://cpl.gg/");
        }


        public async void iniWebView()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoder/Webview2"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoder/Webview2/");
            }


            CoreWebView2Environment tweb = await CoreWebView2Environment.CreateAsync("", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoder/Webview2",
                                        new CoreWebView2EnvironmentOptions(null, "FR", null));

            //await webView.EnsureCoreWebView2Async(tweb);
        }


    }
}
