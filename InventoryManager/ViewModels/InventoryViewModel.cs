using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private ProductInventoryEntry _selectedEntry;

        [ImportingConstructor]
        public InventoryViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            InventoryEntries = new ObservableCollection<ProductInventoryEntry>();
            LoadInventoryFromXml();
        }

        public ProductInventoryEntry SelectedEntry
        {
            get { return _selectedEntry; }
            set
            {
                _selectedEntry = value;
                NotifyOfPropertyChange(() => SelectedEntry);
            }
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

        public void DeleteEntry()
        {
            if (SelectedEntry == null) return;
            InventoryEntries.Remove(InventoryEntries.First(e => e.Name.Equals(SelectedEntry.Name)));
            NotifyOfPropertyChange(() => InventoryEntries);
        }

        public void LoadInventoryFromXml()
        {
            try
            {
                FileStream file = File.OpenRead(_filePath);
                InventoryEntries = (ObservableCollection<ProductInventoryEntry>) _xmlSerializer.Deserialize(file);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Inventory file could not be found, creating one");
                File.Create(_filePath);
            }          
        }
        public void SaveInventoryToXml()
        {
            var file = File.Create(_filePath);
            _xmlSerializer.Serialize(file, InventoryEntries);
            file.Close();
        }

        public void Handle(ProductInventoryEntry newEntry)
        {
            if (newEntry == null)
            {
                Debug.WriteLine("Null object passed to inventory");
                return;
            }
            InventoryEntries.Add(newEntry);
            NotifyOfPropertyChange(() => InventoryEntries);
        }
    }
}