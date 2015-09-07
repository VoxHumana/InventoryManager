using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Caliburn.Micro;
using InventoryManager.Properties;

namespace InventoryManager.ViewModels
{
    [Export(typeof (EditProductViewModel))]
    public class EditProductViewModel : PropertyChangedBase, IHandle<Dictionary<string, Product>>, IDataErrorInfo
    {
        private const string EditProductMessage = "editProductMessage";
        private readonly IEventAggregator _eventAggregator;
        private readonly DirectoryInfo _products = new DirectoryInfo(Environment.CurrentDirectory + "\\products");
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof (Product));
        private readonly Regex _priceRegex;
        private string _oldProductName;
        private string _productCost, _productName, _productPrice;

        [ImportingConstructor]
        public EditProductViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _priceRegex = new Regex(@"^\b\d*[.]?\d{1,2}\b$");
        }

        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                NotifyOfPropertyChange(() => ProductName);
                NotifyOfPropertyChange(() => CanSaveEditedProduct);
            }
        }

        public string ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
                NotifyOfPropertyChange(() => ProductPrice);
                NotifyOfPropertyChange(() => CanSaveEditedProduct);
            }
        }

        public string ProductCost
        {
            get { return _productCost; }
            set
            {
                _productCost = value;
                NotifyOfPropertyChange(() => ProductCost);
                NotifyOfPropertyChange(() => CanSaveEditedProduct);
            }
        }
      
        public void Handle(Dictionary<string, Product> message)
        {
            if (!message.ContainsKey(EditProductMessage)) return;
            _oldProductName = message[EditProductMessage].Name;
            ProductName = message[EditProductMessage].Name;
            ProductCost = message[EditProductMessage].Cost.ToString(CultureInfo.InvariantCulture);
            ProductPrice = message[EditProductMessage].Price.ToString(CultureInfo.InvariantCulture);
        }
        public bool CanSaveEditedProduct => !string.IsNullOrEmpty(_productName) &&
                                            !string.IsNullOrEmpty(_productCost) &&
                                            !string.IsNullOrEmpty(_productPrice) &&
                                            _priceRegex.IsMatch(ProductCost) &&
                                            _priceRegex.IsMatch(ProductPrice);

        public void SaveEditedProduct()
        {
            var product = new Product
            {
                Name = ProductName,
                Cost = Convert.ToDouble(ProductCost),
                Price = Convert.ToDouble(ProductPrice)
            };

            var pathToDelete = _products.FullName + string.Format("\\{0}.xml", RemoveWhitespace(_oldProductName));
            if (File.Exists(pathToDelete))
            {
                File.Delete(pathToDelete);
            }

            var path = _products.FullName + string.Format("\\{0}.xml", RemoveWhitespace(ProductName));
            var file = File.Create(path);
            _xmlSerializer.Serialize(file, product);
            file.Close();
            _eventAggregator.PublishOnUIThread(new Dictionary<string, Product> {{"saveProductMessage", product}});
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
                if(columnName.Equals("ProductName"))
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
                if (columnName.Equals("ProductCost"))
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
                if (columnName.Equals("ProductPrice"))
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