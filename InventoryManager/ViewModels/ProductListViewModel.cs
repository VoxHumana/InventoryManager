using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace InventoryManager.ViewModels
{
    [Export(typeof(ProductListViewModel))]
    public class ProductListViewModel : PropertyChangedBase, IHandle<Dictionary<string, Product>>
    {
        private string _saveProductMessage = "saveProductMessage";
        private readonly string _path = Environment.CurrentDirectory + "\\products\\";
        private readonly IEventAggregator _eventAggregator;
        readonly IWindowManager _windowManager = new WindowManager();
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Product));
        private ObservableCollection<Product> _productList;

        public NewProductViewModel NewProductModel { get; set; }
        public EditProductViewModel EditProductModel { get; set; }
        public ObservableCollection<Product> ProductList
        {
            get { return _productList; }
            set
            {
                _productList = value;
                NotifyOfPropertyChange(() => ProductList);
            }
        }
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyOfPropertyChange(() => Quantity);
            }
        }
        [ImportingConstructor]
        public ProductListViewModel(IEventAggregator eventAggregator, NewProductViewModel newProductViewModel, EditProductViewModel editProductModel)
        {
            _eventAggregator = eventAggregator;
            NewProductModel = newProductViewModel;
            EditProductModel = editProductModel;
            _eventAggregator.Subscribe(this);
            LoadProductsFromFile();
        }

        public void SelectThisProduct()
        {
            var inventoryEntry = new ProductInventoryEntry(SelectedProduct, Quantity);
            _eventAggregator.PublishOnUIThread(inventoryEntry);
        }

        public void CreateNewProduct()
        {
            _windowManager.ShowDialog(NewProductModel);
        }

        public void LoadProductsFromFile()
        {
            DirectoryInfo d = new DirectoryInfo(_path);
            ObservableCollection<Product> productList = new ObservableCollection<Product>();
            foreach (FileInfo productFile in d.GetFiles("*.xml"))
            {
                using (FileStream stream = new FileStream(_path + productFile.Name, FileMode.Open))
                {
                    Product product = (Product)_xmlSerializer.Deserialize(stream);
                    productList.Add(product);
                }
            }
            ProductList = productList;
        }

        public void EditProduct()
        {
            if (SelectedProduct == null) return;
            _eventAggregator.PublishOnUIThread(new Dictionary<string, Product>() {{ "editProductMessage", SelectedProduct }} );
            _windowManager.ShowDialog(EditProductModel);
        }

        public void Handle(Dictionary<string, Product> message)
        {
            if(!message.ContainsKey(_saveProductMessage)) return;
            LoadProductsFromFile();
        }
    }
}
