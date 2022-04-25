using CivLaucherDotNetCore.Controleur;
using CivLauncher;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logique d'interaction pour MainFrame.xaml
    /// </summary>
    public partial class MainFrame : UserControl
    {
        ModsViewer modViewers=null;
        BankModController bmc;
        ContentControl mainContentControl;
        ScrollText contentControlLabelInfo;
        public MainFrame(BankModController bmc, ContentControl contentControl, ScrollText  contentControlLabelInfo)
        {
            this.contentControlLabelInfo = contentControlLabelInfo;
            this.mainContentControl = contentControl;
            this.bmc = bmc;
            InitializeComponent();
            //bJouer.Style = BoutonJouer;


            webview.Content = new WebView();    
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "steam://rungameid/289070";
            process.Start();
        }

        private void GoToModFrame(object sender, RoutedEventArgs e)
        {
            if (this.modViewers != null)
            {
                this.mainContentControl.Content = this.modViewers;
            }
            else
            {
                this.mainContentControl.Content = new ModsViewer(bmc, this, mainContentControl, contentControlLabelInfo);
            }
        }
    }
}
