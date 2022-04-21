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


        private string buttonViewModVal;
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
        public ModView(Mod mod)
        {
            this.modP = mod;
            this.ModController.View = this;
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
                this.labelInfo = this.Model.repoName + " Version :" + tagSelect.FriendlyName + "Installée";
            }
            else
            {
                if (this.buttonViewMod == ButtonAction.install.ToString())
                {
                    ModController.cloneMod();
                    this.tagSelect = ModController.TagActuel();
                    Console.Write(this.tags);
                    this.derniereVersionDisponible = this.ModController.tags[0].FriendlyName;
                    this.labelInfo = this.Model.repoName + " Version :" + tagSelect.FriendlyName + "Installée";
                }


            }
        }
        
    }
}

