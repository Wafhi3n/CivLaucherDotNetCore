
using CivLaucherDotNetCore;
using CivLaucherDotNetCore.Controleur;
using CivLaucherDotNetCore.Vue;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            configurationBuilder.AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            var bindConfig = new Config();
            configuration.Bind(bindConfig);

            BankMod bm = new BankMod(bindConfig);
            BankModController bmc = new BankModController(bm);
            bmc.GetAllModsFromConfig();
            bmc.InitialiseAllModRepoFromPath();

            this.contentControl.Content = new MainFrame(bmc,this.contentControl);

        }


    }


}
