using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using WPFLocalizeExtension.Engine;

namespace InventoryManager.ViewModels
{
    [Export(typeof(MainViewModel))]
    public class MainViewModel : PropertyChangedBase
    {
        public ObservableCollection<string> Languages { get; set; }
        public InventoryViewModel NewInventoryModel { get; set; }
        public ProductListViewModel ProductListModel { get; set; }
        [ImportingConstructor]
        public MainViewModel(ProductListViewModel productListModel, InventoryViewModel newInventoryModel)
        {
            NewInventoryModel = newInventoryModel;
            ProductListModel = productListModel;
            Languages = new ObservableCollection<string>() {"en", "zh-Hans", "de", "ru", "fr", "br"};
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.SetCultureCommand.Execute("zh-Hans");
        }
    }
}
