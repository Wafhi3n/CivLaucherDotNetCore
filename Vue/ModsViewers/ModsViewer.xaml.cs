using CivLaucherDotNetCore.Controleur;
using CivLaucherDotNetCore.Model;
using CivLaucherDotNetCore.Vue.Model;
using CivLauncher;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour ModsViewer.xaml
    /// </summary>
    public partial class ModsViewer : UserControl
    {
        BankModController bmc;
        MainFrame mainFrame;
        ContentControl mainContentControl;
        ScrollText contentControlLabelInfo;

       

        private ObservableCollection<ModView> OMod;
        //ComboBox Tagscb;
        public ModsViewer(Controleur.BankModController bmc, MainFrame mainFrame, ContentControl mainContentControl, ScrollText contentControlLabelInfo)
        {
            InitializeComponent();
            this.contentControlLabelInfo = contentControlLabelInfo;
            OMod = new ObservableCollection<ModView>();

            foreach( Mod m in bmc.bm.mods)
            {
                OMod.Add(new ModView(m, contentControlLabelInfo));
            }
            
            this.bmc=bmc;
            this.mainFrame=mainFrame;
            this.mainContentControl=mainContentControl;

            DataGridMod.ItemsSource = OMod;
        }
        private void update_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ((ModView)(button.DataContext)).updateBranchToTagClickAsync();
        }


        private void RetourMainFrame(object sender, RoutedEventArgs e)
        {
            if (this.mainFrame != null)
            {
                this.mainContentControl.Content = this.mainFrame;
            }

        }

        private void ComboBox_SelectionChanged(object sender, EventArgs e)
        {
            ModView mv = ((ModView)(((ComboBox)sender).DataContext));

            Tag g = (Tag)(((ComboBox)sender).SelectedItem);

            mv.tagSelect = g;


            if (mv.tagCanChange(g))
            {
                mv.changeVisibilityButtonAndStatus(true);
            }
            else
            {
                mv.changeVisibilityButtonAndStatus(false);

            }



            // Do actions
        }
    }
}
