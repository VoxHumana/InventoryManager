using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace InventoryManager.ViewModels
{
    [Export(typeof(EditProductViewModel))]
    public class EditProductViewModel: PropertyChangedBase , IHandle<Dictionary<string, Product>>
    {
        private readonly IEventAggregator _eventAggregator;
        private string _editProductMessage = "editProductMessage";
        private string _oldProductName;
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Product));
        readonly DirectoryInfo _products = new DirectoryInfo(Environment.CurrentDirectory + "\\products");

        [ImportingConstructor]
        public EditProductViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value;
                NotifyOfPropertyChange(() => ProductName);
            }
        }

        private string _productPrice;
        public string ProductPrice
        {
            get { return _productPrice; }
            set
            {
                _productPrice = value;
                NotifyOfPropertyChange(() => ProductPrice);
            }
        }

        private string _productCost;
        public string ProductCost
        {
            get { return _productCost; }
            set
            {
                _productCost = value;
                NotifyOfPropertyChange(() => ProductCost);
            }
        }

        public void SaveEditedProduct()
        {
            Product product = new Product()
            {
                Name = ProductName,
                Cost = Convert.ToDouble(ProductCost),
                Price = Convert.ToDouble(ProductPrice)
            };

            string pathToDelete = _products.FullName + String.Format("\\{0}.xml", RemoveWhitespace(_oldProductName));
            File.Delete(pathToDelete);

            string path = _products.FullName + string.Format("\\{0}.xml", RemoveWhitespace(ProductName));
            FileStream file = File.Create(path);
            _xmlSerializer.Serialize(file, product);
            file.Close();
            _eventAggregator.PublishOnUIThread(new Dictionary<string, Product> { { "saveProductMessage", product } });
        }

        public void Handle(Dictionary<string, Product> message)
        {
            if (!message.ContainsKey(_editProductMessage)) return;
            _oldProductName = message[_editProductMessage].Name;
            ProductName = message[_editProductMessage].Name;
            ProductCost = message[_editProductMessage].Cost.ToString(CultureInfo.InvariantCulture);
            ProductPrice = message[_editProductMessage].Price.ToString(CultureInfo.InvariantCulture);
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
