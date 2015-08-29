using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace InventoryManager.ViewModels
{
    [Export(typeof(NewProductViewModel))]
    public class NewProductViewModel : PropertyChangedBase
    {
        private Product _product;
        private string _productName;
        private string _productPrice;
        private string _productCost;
        private readonly IEventAggregator _eventAggregator;
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Product));
        readonly DirectoryInfo _products = new DirectoryInfo(Environment.CurrentDirectory + "\\products");

        [ImportingConstructor]
        public NewProductViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                NotifyOfPropertyChange(() => ProductName);
            }
        }

        public string ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
                NotifyOfPropertyChange(() => ProductPrice);
            }
        }

        public string ProductCost
        {
            get { return _productCost; }
            set
            {
                _productCost = value;
                NotifyOfPropertyChange(() => ProductCost);
            }
        }

        public void SaveProductToFile()
        {
            _product = new Product
            {
                Name = ProductName,
                Price = Convert.ToDouble(ProductPrice),
                Cost = Convert.ToDouble(ProductCost)
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
    }
}
