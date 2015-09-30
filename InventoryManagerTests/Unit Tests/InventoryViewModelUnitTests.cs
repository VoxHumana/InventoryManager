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
            Mock<Product> selectedEntry = new Mock<Product>();

            //Act
            _inventoryViewModel.SelectedProduct = selectedEntry.Object;

            //Assert
            _inventoryViewModel.CanDeleteProduct.Should().BeTrue("because an entry has been selected");

        }

        [Test]
        public void CanDeleteEntry_ShouldReturnFalse_WhenSelectedEntryIsNull()
        {
            //Act
            _inventoryViewModel.SelectedProduct = null;

            //Assert
            _inventoryViewModel.CanDeleteProduct.Should().BeFalse("because no entry has been selected");
        }
    }
}
