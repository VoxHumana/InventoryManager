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
        public NewProductViewModel NewProductModel { get; set; }
        private string _selectedLanguage;

        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                LocalizeDictionary.Instance.SetCultureCommand.Execute(_selectedLanguage);
                NotifyOfPropertyChange(() => SelectedLanguage);
            }
        }
        [ImportingConstructor]
        public MainViewModel(ProductListViewModel productListModel, InventoryViewModel newInventoryModel, NewProductViewModel newProductModel)
        {
            NewInventoryModel = newInventoryModel;
            NewProductModel = newProductModel;
            ProductListModel = productListModel;
            Languages = new ObservableCollection<string>() {"en", "zh-Hans"};           
            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.SetCultureCommand.Execute("en");
            SelectedLanguage = "en";
        }
    }
}
