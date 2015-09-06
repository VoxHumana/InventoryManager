using System.Diagnostics;
using System.Globalization;
using Caliburn.Micro;
using FluentAssertions;
using InventoryManager.ViewModels;
using Moq;
using NUnit.Framework;

namespace InventoryManagerTests
{
    [TestFixture]
    public class NewProductViewModelTest
    {

        private readonly Mock<IEventAggregator> _eventAggregatorMock = new Mock<IEventAggregator>();
        private NewProductViewModel _newProductViewModel;

        [SetUp]
        public void Init()
        {
            _newProductViewModel = new NewProductViewModel(_eventAggregatorMock.Object);
        }

        [TestCase("Product name", "42", "42")]
        [TestCase("Product name", "4.2", "42")]
        [TestCase("Product name", "42", "4.2")]
        [TestCase("Product name", "4.22", "4.22")]
        [TestCase("Product name", "444444444.22", "444444444.22")]
        public void CanSaveProduct_ShouldReturnTrue_WhenProductPropertiesValid(string productName, string productPrice, string productCost)
        {
            
            //Arrange

            //Act
            _newProductViewModel.ProductName = productName;
            _newProductViewModel.ProductPrice = productPrice;
            _newProductViewModel.ProductCost = productCost;

            //Assert
            _newProductViewModel.CanSaveProductToFile.Should().BeTrue("because all necessary properties are valid");
        }

        [Test]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductNameIsEmpty()
        {

            //Arrange
            const string price = "42";
            const string cost = "42";

            //Act
            _newProductViewModel.ProductName = string.Empty;
            _newProductViewModel.ProductPrice = price;
            _newProductViewModel.ProductCost = cost;

            //Assert
            _newProductViewModel.ProductName.Should().BeNullOrEmpty("because no value was assigned to product name");
            _newProductViewModel.CanSaveProductToFile.Should().BeFalse("because the product has no name");
            _newProductViewModel["ProductName"].Should().Be("Enter product name", "because no value was assigned to product name");
        }

        [Test]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductNameIsNull()
        {

            //Arrange
            const string price = "42";
            const string cost = "42";

            //Act
            _newProductViewModel.ProductName = null;
            _newProductViewModel.ProductPrice = price;
            _newProductViewModel.ProductCost = cost;

            //Assert
            _newProductViewModel.ProductName.Should().BeNullOrEmpty("because no value was assigned to product name");
            _newProductViewModel.CanSaveProductToFile.Should().BeFalse("because the product has no name");
            _newProductViewModel["ProductName"].Should().Be("Invalid input", "because no value was assigned to product name");
        }

        [TestCase(null)]
        [TestCase("")]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductPriceIsNullOrEmpty(string productPrice)
        {
            //Arrange
            const string name = "Product name";
            const string cost = "42";

            //Act
            _newProductViewModel.ProductName = name;
            _newProductViewModel.ProductPrice = productPrice;
            _newProductViewModel.ProductCost = cost;

            //Assert
            _newProductViewModel.ProductPrice.Should().BeNullOrEmpty("because no value was assigned to product price");
            _newProductViewModel.CanSaveProductToFile.Should().BeFalse("because the product has no price");
            _newProductViewModel["ProductPrice"].Should().Be("Enter product price", "because no value was assigned to product price");
        }

        [TestCase(null)]
        [TestCase("")]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductCostIsNullOrEmpty(string productCost)
        {
            //Arrange
            const string name = "Product name";
            const string price = "42";

            //Act
            _newProductViewModel.ProductName = name;
            _newProductViewModel.ProductPrice = price;
            _newProductViewModel.ProductCost = productCost;

            //Assert
            _newProductViewModel.ProductCost.Should().BeNullOrEmpty("because no value was assigned to product cost");
            _newProductViewModel.CanSaveProductToFile.Should().BeFalse("because the product has no cost");
            _newProductViewModel["ProductCost"].Should().Be("Enter product cost", "because no value was assigned to product cost");
        }

        [TestCase("this is not numeric")]
        [TestCase("!@#%$%^^&*")]
        [TestCase(" 1 one 2 two 3 three")]
        [TestCase("1   2   3   4")]
        [TestCase("$1234")]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductPriceIsNotANumber(string productPrice)
        {
            //Arrange
            const string name = "Product name";
            const string cost = "42";

            //Act
            _newProductViewModel.ProductName = name;
            _newProductViewModel.ProductCost = cost;
            _newProductViewModel.ProductPrice = productPrice;

            //Assert
            _newProductViewModel.CanSaveProductToFile.Should().BeFalse($"because the '{productPrice}' is not a number");
            _newProductViewModel["ProductPrice"].Should().Be("Invalid input", $"because '{productPrice}' is not a number");
        }

        [TestCase("this is not numeric")]
        [TestCase("!@#%$%^^&*")]
        [TestCase(" 1 one 2 two 3 three")]
        [TestCase("1   2   3   4")]
        [TestCase("$1234")]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductCostIsNotANumber(string productCost)
        {
            //Arrange
            const string name = "Product name";
            const string price = "42";

            //Act
            _newProductViewModel.ProductName = name;
            _newProductViewModel.ProductPrice = price;
            _newProductViewModel.ProductCost = productCost;

            //Assert
            _newProductViewModel.CanSaveProductToFile.Should().BeFalse($"because '{productCost}' is not a number");
            _newProductViewModel["ProductCost"].Should().Be("Invalid input", $"because '{productCost}' is not a number");
        }

    }
}
