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
    /// Logique d'interaction pour ModView.xaml
    /// </summary>
    public partial class ModView : UserControl
    {
        public ModController modController { get; set; }
        public Mod modData { get; set; }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            modController.updateOrInstallToLastTag((Object)Tags.SelectedItem);

        }
    }
}
