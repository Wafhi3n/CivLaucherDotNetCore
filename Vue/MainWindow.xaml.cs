using CivLaucherDotNetCore.Controleur;
using CivLaucherDotNetCore.Model;
using CivLaucherDotNetCore.Vue;
using Microsoft.Extensions.Configuration;
using ModLoader.Vue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace CivLauncher
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    /// 



       public partial class MainWindow : Window
    {

        ScrollText st;
        //MainWindowViewModel mainView;
        public MainWindow()
        {
            InitializeComponent();
            //mainView = new MainWindowViewModel();
            //DataContext = mainView;
            this.st = new ScrollText(_canMain, labelInfo);
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
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.st.ScrollLabelInfo();
        }
    }
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
                //if (textBlock.Text != value)
                //{
                    textBlock.Text = value;
                    ScrollLabelInfo();
                //}
            }
        }
        public void ScrollLabelInfo()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -this.textBlock.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:30"));
            this.textBlock.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }

        public void setLabelUpdate(BankModController bmc)
        {
            bmc.DisplayisUpdateAviable();
            //setTextUpdateAviable(lmc);
        }
        public void setTextUpdateAviable(string a)
        {
            if (labelInfoV == "Bon Jeu"){
                labelInfoV = "";
            }
            else {
                labelInfoV += a;
                ScrollLabelInfo();

            }

        }
    }
}

