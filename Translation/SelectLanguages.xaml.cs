using ModLoader.Vue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ModLoader.Translation
{
    /// <summary>
    /// Logique d'interaction pour SelectLanguages.xaml
    /// </summary>
    public partial class SelectLanguages : UserControl
    {
        MainWindowViewModel mainView;

        public ICollectionView Languages { get { return mainView.Languages; } set { mainView.Languages = value; } }

    public SelectLanguages(MainWindowViewModel view)
        {
            InitializeComponent();
            mainView = view;
            Console.Write(Languages);
            //DataContext = view;
            selecteur.DataContext = this;
        }
    }
}
