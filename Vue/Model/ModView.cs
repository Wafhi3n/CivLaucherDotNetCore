using CivLaucherDotNetCore.Controleur;
using CivLaucherDotNetCore.Model;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CivLaucherDotNetCore.Vue.Model
{
    public enum ButtonAction
    {
        install = 0 ,
        maj     = 1 ,
        rien    = 2    
    }

    

    class ModView : INotifyPropertyChanged
    {
        private Mod modP;
        public event PropertyChangedEventHandler PropertyChanged;
        System.Windows.Controls.TextBlock contentControlLabelInfo;

        private string buttonViewModVal;
       
        
        System.Windows.Visibility visibility;

        public System.Windows.Visibility buttonUpdateVisible { get
            {
                return visibility;

            }
            set { this.visibility = value;
                NotifyPropertyChanged();
            }
        }
            
        public string buttonViewMod
        {
            get
            {
                return buttonViewModVal;
            }
            set
            {
                if (this.buttonViewModVal != value)
                {
                    this.buttonViewModVal = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Tag tagSelectVal;
        public Tag tagSelect
        {
            get
            {
                return tagSelectVal;
            }
            set
            {
                if (this.tagSelectVal != value)
                {
                    this.tagSelectVal = value;
                }
                NotifyPropertyChanged();
            }
        }
        public Mod Model {

            get { return modP; }

            set
            {
                if (this.modP != value)
                {
                    this.modP = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool tagCanChange(Tag g)
        {

            string a = this.ModController.InstalledVersion();
            string b = this.tagSelect.Target.Sha;
            return /*(this.tagSelect != g && */ a != b;
        }

        internal void changeVisibilityButtonAndStatus(bool v)
        {

            if (v)
            {
                this.buttonUpdateVisible = System.Windows.Visibility.Visible;

                if (!this.modP.mController.isInstalled())
                {
                    changeButtonMod(ButtonAction.install);
                    // button to install
                }
                else
                {
                    changeButtonMod(ButtonAction.maj);
                    //button to maj
                }


            }
            else
            {
                this.buttonUpdateVisible = System.Windows.Visibility.Hidden;
            }


        }


        public ModView(Mod mod, System.Windows.Controls.TextBlock contentControlLabelInfo)
        {
            this.modP = mod;
            this.ModController.View = this;
            this.contentControlLabelInfo = contentControlLabelInfo;
            //changeVisibilityButtonAndStatus(false);


            if (ModController.isInstalled())
            {
                this.tagSelect = ModController.TagActuel();
                this.derniereVersionDisponible = ModController.tags[0].FriendlyName;
                if(this.tagSelect != this.ModController.tags.First())
                {
                    changeButtonMod(ButtonAction.maj);

                }
                else
                {
                    changeButtonMod(ButtonAction.rien);

                }
            }
            else
            {
                changeButtonMod(ButtonAction.install);
            }

        }
        public void changeButtonMod(ButtonAction a)
        {
            this.buttonViewMod = a.ToString();
        }
        public ModController ModController { get { return modP.mController; } }

        public string repoName
        {
            get
            {
                return modP.repoName;
            }

            set
            {
                modP.repoName = value;
                NotifyPropertyChanged();
            }
        }
        public string IndexVersionActuel
        {
            get
            {
                return ModController.IndexVersionActuel.ToString();
            }

            set
            {
                /*modP.mController.version = value;*/
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<Tag> tags
        {
            get
            {
                return ModController.tags;
            }

            set
            {
                /*modP.mController.version = value;*/
                NotifyPropertyChanged();
            }
        }
        private string derniereVersionDisponibleValue;
        public string derniereVersionDisponible
        {
            get
            {
                return derniereVersionDisponibleValue;
            }

            set
            {

                derniereVersionDisponibleValue = value;
                NotifyPropertyChanged();
            }
        }

        private string labelInfoValue;
        public string labelInfo
        {
            get
            {
                return labelInfoValue;
            }

            set
            {

                labelInfoValue = value;
                NotifyPropertyChanged();
            }
        }
        public void updateBranchToTagClick()
        {
            if (tagSelect != null)
            {
                ModController.updateBranchToTag(tagSelect);
                InfoLabelModInstall(this.Model.repoName, tagSelect.FriendlyName);
                changeVisibilityButtonAndStatus(false);
            }
            else
            {
                if (this.buttonViewMod == ButtonAction.install.ToString())
                {
                    ModController.cloneMod();
                    this.tagSelect = ModController.TagActuel();
                    Console.Write(this.tags);
                    this.derniereVersionDisponible = this.ModController.tags[0].FriendlyName;
                    InfoLabelModInstall(this.Model.repoName, tagSelect.FriendlyName);
                    changeVisibilityButtonAndStatus(false);
                }


            }
        }

        public void InfoLabelModInstall(string mod,string tag)
        {
            contentControlLabelInfo.Text = mod + " " + tag + " " + "Installé";
        }


    }
}

