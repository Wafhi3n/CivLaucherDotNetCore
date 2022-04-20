using CivLaucherDotNetCore.Controleur;
using CivLauncher;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
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
        //ComboBox Tagscb;
        public ModsViewer(Controleur.BankModController bmc, MainFrame mainFrame, ContentControl mainContentControl)
        {
            InitializeComponent();
            this.bmc=bmc;
            this.mainFrame=mainFrame;
            //ListMod.ItemsSource= bmc.bm.mods;
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListMod.ItemsSource);
            this.mainContentControl=mainContentControl;
            bmc.UpdateAllModLastAviableRelease();
            //this.Tagscb = tagsCB;
            //mainContentControl.ta
            /*List<ModView> listModsView = new List<ModView>();
            foreach(ModController m in bmc.modsController)
            {
                listModsView.Add(new ModView(m));

            }*/

            
            DataGridMod.ItemsSource = bmc.bm.mods;
            Console.WriteLine(bmc);


        }
        private void update_Click(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;

            ((Mod)(button.DataContext)).mc.updateBranchToTagClick();

            
            //tagsCB.

                 //((Mod)((Button)sender).Tag).mc.updateBranchToTag(tagsCB);


                /*.updateBranchToTag()*/;

            //sender.Tag.

            // sender.
            //modController.updateOrInstallToLastTag((Object)Tags.SelectedItem);

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
            ((Mod)(((ComboBox)sender).DataContext)).mc.tagSelect = (Tag)(((ComboBox)sender).SelectedItem);
            // Do actions
        }
    }
}
