using ModLoader.Translation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ModLoader.Vue
{
    public class MainWindowViewModel
    {
        public ICollectionView Languages { get; set; }

        public MainWindowViewModel()
        {
            Languages = CollectionViewSource.GetDefaultView(TranslationManager.Instance.Languages);
            //Languages.CurrentChanged += (s, e) => TranslationManager.Instance.CurrentLanguage = (CultureInfo)Languages.CurrentItem;
        }
    }
}
