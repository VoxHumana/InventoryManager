using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Xml.Serialization;
using Caliburn.Micro;

namespace InventoryManager.ViewModels
{
    [Export(typeof (InventoryViewModel))]
    public class InventoryViewModel : PropertyChangedBase, IHandle<ProductInventoryEntry>
    {
        private const string InventoryName = "Inventory";
        private readonly IEventAggregator _eventAggregator;
        private readonly string _filePath = Environment.CurrentDirectory + "//" + InventoryName + ".xml";

        private readonly XmlSerializer _xmlSerializer =
            new XmlSerializer(typeof (ObservableCollection<ProductInventoryEntry>));

        private ObservableCollection<ProductInventoryEntry> _inventoryEntries;

        [ImportingConstructor]
        public InventoryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            InventoryEntries = LoadInventory();
        }

        public ObservableCollection<ProductInventoryEntry> InventoryEntries
        {
            get { return _inventoryEntries; }
            set
            {
                _inventoryEntries = value;
                NotifyOfPropertyChange(() => InventoryEntries);
            }
        }

        //public void LoadInventoryFromXml()
        //{
        //    Debug.WriteLine("Button was pressed");
        //    FileStream file = File.OpenRead(_filePath);
        //    Inventory = (List<ProductInventoryEntry>)_xmlSerializer.Deserialize(file);
        //}
        public void Handle(ProductInventoryEntry newEntry)
        {
            InventoryEntries.Add(newEntry);
            NotifyOfPropertyChange(() => InventoryEntries);
        }

        private ObservableCollection<ProductInventoryEntry> LoadInventory()
        {
            var entry1 = new ProductInventoryEntry(5, "Belt", 10, 3);
            var entry2 = new ProductInventoryEntry(10, "Vase", 15, 5);
            var entry3 = new ProductInventoryEntry(15, "Painting", 12, 5);

            return new ObservableCollection<ProductInventoryEntry> {entry1, entry2, entry3};
        }

        public void SaveInventoryToXml()
        {
            var file = File.Create(_filePath);
            _xmlSerializer.Serialize(file, InventoryEntries);
            file.Close();
        }
    }
}