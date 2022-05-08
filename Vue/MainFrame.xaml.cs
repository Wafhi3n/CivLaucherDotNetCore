using CivLaucherDotNetCore.Controleur;
using CivLauncher;
using Microsoft.Web.WebView2.Core;
using ModLoader.Vue;
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
        ModsViewer modViewers;
        BankModController bmc;
        ContentControl mainContentControl;
        ScrollText st;
        MainWindowViewModel mainView;
        public MainFrame(BankModController bmc, ContentControl contentControl, ScrollText  st, MainWindowViewModel view)
        {
            mainView = view;
            this.st = st;
            this.mainContentControl = contentControl;
            this.bmc = bmc;
            InitializeComponent();
            //bJouer.Style = BoutonJouer;
            //this.st.labelInfoV = "Bon jeu";
            st.ScrollLabelInfo();
            this.modViewers = new ModsViewer(bmc, this, mainContentControl, st,view);
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
                this.mainContentControl.Content = modViewers;
            }
        }
    }
}
