
using CivLaucherDotNetCore;
using LibGit2Sharp;
//using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        private string versionValue = String.Empty;
        private string versionDisponibleValue = String.Empty;
        private TagCollection TagsValue ;


        public string path { get; set; }

        public string apiUrl { get; set; }
        public  string status { get; set; }
        public Repo repositoriInfo { get; set; }
        public string repoName { get; set; }
        public Mod(string civModFolder, string url, Repo repositoriInfo)
        {
            this.repositoriInfo = repositoriInfo;
            this.repoName = repositoriInfo.depot;
            this.path = civModFolder + "\\" + repositoriInfo.depot;
            Console.WriteLine(path);
            this.apiUrl =  url + "/" + repositoriInfo.owner + "/" + repositoriInfo.depot + "/releases/latest";
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
        public string versionDisponible
        {

            get { return this.versionDisponibleValue; }

            set
            {
                if (this.versionDisponibleValue != value)
                {
                    this.versionDisponibleValue = value;
                    NotifyPropertyChanged();
                }
            }

        }
        public TagCollection Tags
        {

            get { return TagsValue; }

            set
            {
                this.TagsValue = value;
                NotifyPropertyChanged();
            }

        }
    }

}

