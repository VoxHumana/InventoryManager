﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace InventoryManager.ViewModels
{
    [Export(typeof (ProductListViewModel))]
    public class ProductListViewModel : PropertyChangedBase, IHandle<Dictionary<string, Product>>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly string _path = Environment.CurrentDirectory + "\\products\\";
        private readonly IWindowManager _windowManager = new WindowManager();
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof (Product));
        private ObservableCollection<Product> _productList;

        private string _quantity;
        private readonly string _saveProductMessage = "saveProductMessage";
        private Product _selectedProduct;

        [ImportingConstructor]
        public ProductListViewModel(IEventAggregator eventAggregator, NewProductViewModel newProductViewModel,
            EditProductViewModel editProductModel)
        {
            _eventAggregator = eventAggregator;
            NewProductModel = newProductViewModel;
            EditProductModel = editProductModel;
            _eventAggregator.Subscribe(this);
            LoadProductsFromFile();
        }

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

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
            }
        }

        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                NotifyOfPropertyChange(() => Quantity);
            }
        }

        public void Handle(Dictionary<string, Product> message)
        {
            if (!message.ContainsKey(_saveProductMessage)) return;
            LoadProductsFromFile();
        }

        public void SelectThisProduct()
        {
            if (SelectedProduct == null) return;
            Debug.WriteLine("QUANTITY IS: {0} TYPE: {1}", Quantity, Quantity.GetType());
            var inventoryEntry = new ProductInventoryEntry(SelectedProduct, Convert.ToInt32(Quantity));
            _eventAggregator.PublishOnUIThread(inventoryEntry);
        }

        public void CreateNewProduct()
        {
            _windowManager.ShowDialog(NewProductModel);
        }

        private void LoadProductsFromFile()
        {
            var d = new DirectoryInfo(_path);
            var productList = new ObservableCollection<Product>();
            foreach (var productFile in d.GetFiles("*.xml"))
            {
                using (var stream = new FileStream(_path + productFile.Name, FileMode.Open))
                {
                    var product = (Product) _xmlSerializer.Deserialize(stream);
                    productList.Add(product);
                }
            }
            ProductList = productList;
        }

        public void EditProduct()
        {
            if (SelectedProduct == null) return;
            _eventAggregator.PublishOnUIThread(new Dictionary<string, Product> {{"editProductMessage", SelectedProduct}});
            _windowManager.ShowDialog(EditProductModel);
        }

        public void DeleteProduct()
        {
            try
            {
                var d = new DirectoryInfo(_path);
            }
            catch (SecurityException securityException)
            {
                Debug.WriteLine("ERROR: Security exception was caught");
            }
            try
            {
                File.Delete(_path + string.Format("{0}.xml", RemoveWhitespace(SelectedProduct.Name)));
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Debug.WriteLine("ERROR: Could not find directory");
            }
            

            LoadProductsFromFile();
        }

        private static string RemoveWhitespace(string inputString)
        {
            return
                inputString.ToCharArray()
                    .Where(c => !char.IsWhiteSpace(c))
                    .Select(c => c.ToString())
                    .Aggregate((a, b) => a + b);
        }
    }
}