
using CivLaucherDotNetCore;
using CivLaucherDotNetCore.Controleur;
using CivLaucherDotNetCore.Model;
using CivLaucherDotNetCore.Vue;
using Microsoft.Extensions.Configuration;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CivLauncher
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

        InitializeComponent();








            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(AppDomain.CurrentDomain.BaseDirectory+"\\appsettings.json");
            var configuration = configurationBuilder.Build();
            var bindConfig = new Config();
            configuration.Bind(bindConfig);

            BankMod bm = new BankMod(bindConfig);
            BankModController bmc = new BankModController(bm);
            bmc.GetAllModsFromConfig();
            bmc.InitialiseAllModRepoFromPath();

            this.contentControl.Content = new MainFrame(bmc,this.contentControl);

        }

        public async void iniWebView()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoder/Webview2"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoder/Webview2/");
            }


            await CoreWebView2Environment.CreateAsync("", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/ModLoder/Webview2",
                                        new CoreWebView2EnvironmentOptions(null, "FR", null));
        }

    }


}
