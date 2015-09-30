using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace InventoryManager.ViewModels
{
    [Export(typeof (InventoryViewModel))]
    public class InventoryViewModel : PropertyChangedBase, IHandle<Dictionary<string, Product>>
    {
        public object EditProductModel { get; set; }

        public NewProductViewModel NewProductModel { get; set; }

        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager = new WindowManager();
        private readonly string _productsPath = Environment.CurrentDirectory + "\\products\\";
        private const string SaveProductMessage = "saveProductMessage";

        private readonly XmlSerializer _xmlSerializer =
            new XmlSerializer(typeof (Product));

        private ObservableCollection<Product> _inventory;
        private Product _selectedProduct;

        [ImportingConstructor]
        public InventoryViewModel(IEventAggregator eventAggregator, NewProductViewModel newProductViewModel,
            EditProductViewModel editProductModel)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            NewProductModel = newProductViewModel;
            EditProductModel = editProductModel;
            Inventory = new ObservableCollection<Product>();
            LoadInventoryFromXml();
        }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanDeleteProduct);
                NotifyOfPropertyChange(() => CanEditProduct);
            }
        }

        public ObservableCollection<Product> Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                NotifyOfPropertyChange(() => Inventory);
            }
        }

        private string _quantity;
        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyOfPropertyChange(() => Quantity);
            }
        }

        public bool CanEditProduct => SelectedProduct != null;

        public void EditProduct()
        {
            if (SelectedProduct == null) return;
            _eventAggregator.PublishOnUIThread(new Dictionary<string, Product>()
            {
                {"editProductMessage", SelectedProduct}
            });
            _windowManager.ShowDialog(EditProductModel);
        }

        public bool CanDeleteProduct => SelectedProduct != null;

        public void DeleteProduct()
        {
            if (SelectedProduct == null) return;
            Inventory.Remove(Inventory.First(e => e.Name.Equals(SelectedProduct.Name)));
            NotifyOfPropertyChange(() => Inventory);
        }

        public void CreateNewProduct()
        {
            _windowManager.ShowDialog(NewProductModel);
        }

        private void LoadInventoryFromXml()
        {
            var d = new DirectoryInfo(_productsPath);
            if (!d.Exists)
                d.Create();
            var productList = new ObservableCollection<Product>();
            foreach (var productFile in d.GetFiles("*.xml"))
            {
                using (var stream = new FileStream(_productsPath + productFile.Name, FileMode.Open))
                {
                    var product = (Product)_xmlSerializer.Deserialize(stream);
                    productList.Add(product);
                }
            }
            Inventory = productList;
        }
        public void Handle(Dictionary<string, Product> message)
        {
            if (!message.ContainsKey(SaveProductMessage)) return;
            LoadInventoryFromXml();
        }
    }
}