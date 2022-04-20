
using CivLaucherDotNetCore;
using CivLaucherDotNetCore.Controleur;
using LibGit2Sharp;
//using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace CivLauncher
{
    public class Mod : DynamicObject, INotifyPropertyChanged
    {
        //Repository repository;
        public event PropertyChangedEventHandler PropertyChanged;

        public ModController mc { get; set; }

        private string versionValue = String.Empty;

        private ObservableCollection<Tag> tagsVal;
        public ObservableCollection<Tag> tags
        {

            set { tagsVal = value; }
            get { return tagsVal; }
        }

        internal string? repoUrl;

        public string path { get; set; }

        public string apiUrl { get; set; }
        public  string status { get; set; }
        public Repo repositoriInfo { get; set; }
        public string repoName { get; set; }
        public Mod(string civModFolder, string url, Repo repositoriInfo)
        {
            this.tags = new ObservableCollection<Tag>();

            this.repositoriInfo = repositoriInfo;
            this.repoName = repositoriInfo.depot;
            this.path = civModFolder + "\\" + repositoriInfo.depot;
            Console.WriteLine(path);
            this.apiUrl =  url + "/" + repositoriInfo.owner + "/" + repositoriInfo.depot;
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string version {

            get { return this.versionValue; } 
            
            set {
                if (this.versionValue != value)
                {
                    this.versionValue = value;
                    NotifyPropertyChanged();
                }
            } 
        
        }
        public string derniereVersionDisponible
        {

            get { return this.tags[0].FriendlyName; }

            set
            {
               
            }

        }



        

        public void updateBranchToTag(Tag t)
        {
            this.mc.updateBranchToTag(t);
        }


    }

}

