using Caliburn.Micro;
using FluentAssertions;
using InventoryManager;
using InventoryManager.ViewModels;
using Moq;
using NUnit.Framework;

namespace InventoryManagerTests.Unit_Tests
{
    [TestFixture]
    public class InventoryViewModelUnitTests
    {
        private InventoryViewModel _inventoryViewModel;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<NewProductViewModel> _newProductModel;
        private Mock<EditProductViewModel> _editProductModel;

        [SetUp]
        public void Init()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _newProductModel = new Mock<NewProductViewModel>();
            _editProductModel = new Mock<EditProductViewModel>();
            _inventoryViewModel = new InventoryViewModel(_eventAggregatorMock.Object, _newProductModel.Object, _editProductModel.Object);
        }

        [Test]
        public void ViewModel_IsSubscribedToEventAggregator()
        {
            //Assert
            _eventAggregatorMock.Verify(x => x.Subscribe(_inventoryViewModel));
        }

        [Test]
        public void CanDeleteEntry_ShouldReturnTrue_WhenSelectedEntryIsNotNull()
        {
            //Arrange
            Mock<ProductInventoryEntry> selectedEntry = new Mock<ProductInventoryEntry>();

            //Act
            _inventoryViewModel.SelectedEntry = selectedEntry.Object;

            //Assert
            _inventoryViewModel.CanDeleteEntry.Should().BeTrue("because an entry has been selected");

        }

        [Test]
        public void CanDeleteEntry_ShouldReturnFalse_WhenSelectedEntryIsNull()
        {
            //Act
            _inventoryViewModel.SelectedEntry = null;

            //Assert
            _inventoryViewModel.CanDeleteEntry.Should().BeFalse("because no entry has been selected");
        }

        [Test]
        public void CanSaveInventoryToXml_ShouldReturnTrue_WhenInventoryEntriesIsNotNull()
        {
            //Arrange
            Mock<ProductInventoryEntry> entry = new Mock<ProductInventoryEntry>();

            //Act
            _inventoryViewModel.InventoryEntries.Add(entry.Object);

            //Assert
            _inventoryViewModel.CanSaveInventoryToXml.Should()
                .BeTrue("because there is at least one entry in the inventory");
        }

        [Test]
        public void CanSaveInventoryToXml_ShouldReturnFalse_WhenInventoryEntriesIsNull()
        {
            //Act
            _inventoryViewModel.InventoryEntries = null;

            //Assert
            _inventoryViewModel.CanSaveInventoryToXml.Should().BeFalse("because there are no entries in the inventory");
        }

        [Test]
        public void Handle_ShouldAddNewEntryToInventoryEntries_WhenArgumentIsNotNull()
        {
            //Arrange
            Mock<Product> product = new Mock<Product>();
            product.SetupAllProperties();
            product.Object.Name = "Foo";
            product.Object.Price = 20;
            product.Object.Cost = 10;
            ProductInventoryEntry entry = new ProductInventoryEntry(product.Object, 1);

            //Act
            _inventoryViewModel.Handle(entry);

            //Assert
            _inventoryViewModel.InventoryEntries.Should()
                .NotBeNull("because the inventory should have at least one entry");
            _inventoryViewModel.InventoryEntries.Count.Should()
                .BeGreaterThan(0, "because a new entry is added by the handle");
            _inventoryViewModel.CanSaveInventoryToXml.Should()
                .BeTrue("because there is at least one entry in the inventory to save");
        }

        [Test]
        public void Handle_ShouldDoNothing_WhenArgumentIsNull()
        {
            //Act
            _inventoryViewModel.Handle(null);

            //Assert
            _inventoryViewModel.InventoryEntries.Count.Should().Be(0, "because the inventory is empty");
            _inventoryViewModel.CanSaveInventoryToXml.Should()
                .BeFalse("because there are no entries in the inventory to save");
        }
    }
}
