using CivLaucherDotNetCore.Controleur;
using CivLaucherDotNetCore.Model;
using CivLaucherDotNetCore.Vue;
using Microsoft.Extensions.Configuration;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CivLauncher
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 
    public class ScrollText
    {
        TextBlock textBlock;

        Canvas canMain;
        public ScrollText(Canvas _canMain, TextBlock textBlock)
        {
            this.canMain = _canMain;
            this.textBlock = textBlock;
        }

        public string labelInfoV
        {
            get
            { return textBlock.Text; }


            set
            {
                if (textBlock.Text != value)
                {
                    textBlock.Text = value;
                    ScrollLabelInfo();
                }
            }
        }

        public void ScrollLabelInfo()
        {



            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -textBlock.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:30"));
            textBlock.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }

    }
    public partial class MainWindow : Window
    {

        ScrollText st;
        public MainWindow()
        {
            InitializeComponent();
            this.st = new ScrollText(_canMain, labelInfo);
            st.labelInfoV = "Bon jeu";
            var configurationBuilder = new ConfigurationBuilder();
            var bindConfig = new Config();
            bindConfig.checkAndCPConfig();
            configurationBuilder.AddJsonFile(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoader/appsettings.json");
            var configuration = configurationBuilder.Build();
            configuration.Bind(bindConfig);
            //bindConfig.SaveSettings();
            BankMod bm = new BankMod(bindConfig);
            BankModController bmc = new BankModController(bm);
            bmc.GetAllModsFromConfig();
            bmc.InitialiseAllModRepoFromPath();
            this.contentControl.Content = new MainFrame(bmc, this.contentControl, st);

            //

        }
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            this.st.ScrollLabelInfo();
        }








    }


}
