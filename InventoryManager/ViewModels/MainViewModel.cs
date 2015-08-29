using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace InventoryManager.ViewModels
{
    [Export(typeof(MainViewModel))]
    public class MainViewModel : PropertyChangedBase
    {
        public InventoryViewModel NewInventoryModel { get; set; }
        public ProductListViewModel ProductListModel { get; set; }
        [ImportingConstructor]
        public MainViewModel(ProductListViewModel productListModel, InventoryViewModel newInventoryModel)
        {
            NewInventoryModel = newInventoryModel;
            ProductListModel = productListModel;
        }
    }
}
