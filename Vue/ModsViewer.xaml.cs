using CivLaucherDotNetCore.Controleur;
using CivLauncher;
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


        public ModsViewer(Controleur.BankModController bmc, MainFrame mainFrame, ContentControl mainContentControl)
        {
            InitializeComponent();
            this.bmc=bmc;
            this.mainFrame=mainFrame;
            //ListMod.ItemsSource= bmc.bm.mods;
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListMod.ItemsSource);
            this.mainContentControl=mainContentControl;
            bmc.UpdateAllModLastAviableRelease();


            List<ModView> listModsView = new List<ModView>();
            foreach(ModController m in bmc.modsController)
            {
                listModsView.Add(new ModView(m));

            }

            Mod1.Content = listModsView[0];
            Mod2.Content = listModsView[1];
            Mod3.Content = listModsView[2];
            Mod4.Content = listModsView[3];

        }

        private void RetourMainFrame(object sender, RoutedEventArgs e)
        {
            if (this.mainFrame != null)
            {
                this.mainContentControl.Content = this.mainFrame;
            }

        }
    }
}
