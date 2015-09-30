using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Caliburn.Micro;
using InventoryManager.Properties;

namespace InventoryManager.ViewModels
{
    [Export(typeof(NewProductViewModel))]
    public class NewProductViewModel : PropertyChangedBase, IDataErrorInfo
    {
        private Product _product;
        private string _productName, _productPrice, _productCost, _productSupplier;
        private bool _initialProductName, _initialProductPrice, _initialProductCost;
        private readonly Regex _priceRegex;
        private readonly IEventAggregator _eventAggregator;
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Product));
        private readonly DirectoryInfo _products = new DirectoryInfo(Environment.CurrentDirectory + "\\products");

        [ImportingConstructor]
        public NewProductViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _initialProductPrice = _initialProductName = _initialProductCost = true;
            _priceRegex = new Regex(@"^\b\d*[.]?\d{1,2}\b$");
        }
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                NotifyOfPropertyChange(() => ProductName);
                NotifyOfPropertyChange(() => CanSaveProductToFile);
                _initialProductName = false;
            }
        }

        public string ProductSupplier
        {
            get { return _productSupplier; }
            set
            {
                _productSupplier = value;
                NotifyOfPropertyChange(() => ProductSupplier);
            }
        }

        public string ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
                NotifyOfPropertyChange(() => ProductPrice);
                NotifyOfPropertyChange(() => CanSaveProductToFile);
                _initialProductPrice = false;
            }
        }

        public string ProductCost
        {
            get { return _productCost; }
            set
            {
                _productCost = value;
                NotifyOfPropertyChange(() => ProductCost);
                NotifyOfPropertyChange(() => CanSaveProductToFile);
                _initialProductCost = false;
            }
        }

        public bool CanSaveProductToFile => !string.IsNullOrEmpty(_productName) &&
                                            !string.IsNullOrEmpty(_productCost) &&
                                            !string.IsNullOrEmpty(_productPrice) &&
                                            _priceRegex.IsMatch(ProductCost) &&
                                            _priceRegex.IsMatch(ProductPrice);

        public void SaveProductToFile()
        {
            _product = new Product
            {
                Name = ProductName,
                Price = Convert.ToDouble(ProductPrice),
                Cost = Convert.ToDouble(ProductCost),
                Supplier = ProductSupplier
            };

            if (!_products.Exists)
            {
                _products.Create();
            }
            string path = _products.FullName + string.Format("\\{0}.xml", RemoveWhitespace(ProductName));
            var file = File.Create(path);
            _xmlSerializer.Serialize(file, _product);
            file.Close();
            _eventAggregator.PublishOnUIThread(new Dictionary<string, Product> {{ "saveProductMessage", _product }} );
        }

        private string RemoveWhitespace(string inputString)
        {
            return
                inputString.ToCharArray()
                    .Where(c => !char.IsWhiteSpace(c))
                    .Select(c => c.ToString())
                    .Aggregate((a, b) => a + b);
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName.Equals("ProductName") && !_initialProductName)
                {
                    if (string.IsNullOrEmpty(ProductName))
                    {
                        result = Resources.EnterProductName;
                    }
                    else
                    {
                        if (!(ProductName is string))
                            result = Resources.InvalidInput;
                    }
                }
                if (columnName.Equals("ProductCost") && !_initialProductCost)
                {
                    if (string.IsNullOrEmpty(ProductCost))
                    {
                        result = Resources.EnterProductCost;
                    }
                    else 
                    {
                        if (!_priceRegex.IsMatch(ProductCost))
                            result = Resources.InvalidInput;
                    }
                }
                if (columnName.Equals("ProductPrice") && !_initialProductPrice)
                {
                    if (string.IsNullOrEmpty(ProductPrice))
                    {
                        result = Resources.EnterProductPrice;
                    }
                    else
                    {
                        if (!_priceRegex.IsMatch(ProductPrice))
                            result = Resources.InvalidInput;
                    }
                    
                }
                return result;
            }
        }

        public string Error => string.Empty;
    }
}
