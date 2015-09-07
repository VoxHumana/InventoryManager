using Caliburn.Micro;
using FluentAssertions;
using InventoryManager.ViewModels;
using Moq;
using NUnit.Framework;

namespace InventoryManagerTests
{
    [TestFixture]
    class EditProductViewModelUnitTest
    {
        private readonly Mock<IEventAggregator> _eventAggregatorMock = new Mock<IEventAggregator>();
        private EditProductViewModel _editProductViewModel;

        [SetUp]
        public void Init()
        {
            _editProductViewModel = new EditProductViewModel(_eventAggregatorMock.Object);
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
            _editProductViewModel.ProductName = productName;
            _editProductViewModel.ProductPrice = productPrice;
            _editProductViewModel.ProductCost = productCost;

            //Assert
            _editProductViewModel.CanSaveEditedProduct.Should().BeTrue("because all necessary properties are valid");
            _editProductViewModel["ProductName"].Should()
                .BeNullOrEmpty("because no validation error should show for a valid product name");
            _editProductViewModel["ProductPrice"].Should()
                .BeNullOrEmpty("because no validation error should show for a valid product price");
            _editProductViewModel["ProductCost"].Should()
                .BeNullOrEmpty("because no validation error should show for a valid product cost");
        }

        [Test]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductNameIsEmpty()
        {

            //Arrange
            const string price = "42";
            const string cost = "42";

            //Act
            _editProductViewModel.ProductName = string.Empty;
            _editProductViewModel.ProductPrice = price;
            _editProductViewModel.ProductCost = cost;

            //Assert
            _editProductViewModel.ProductName.Should().BeNullOrEmpty("because no value was assigned to product name");
            _editProductViewModel.CanSaveEditedProduct.Should().BeFalse("because the product has no name");
            _editProductViewModel["ProductName"].Should().Be("Enter product name", "because no value was assigned to product name");
        }

        [Test]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductNameIsNull()
        {

            //Arrange
            const string price = "42";
            const string cost = "42";

            //Act
            _editProductViewModel.ProductName = null;
            _editProductViewModel.ProductPrice = price;
            _editProductViewModel.ProductCost = cost;

            //Assert
            _editProductViewModel.ProductName.Should().BeNullOrEmpty("because no value was assigned to product name");
            _editProductViewModel.CanSaveEditedProduct.Should().BeFalse("because the product has no name");
            _editProductViewModel["ProductName"].Should().Be("Enter product name", "because no value was assigned to product name");
        }

        [TestCase(null)]
        [TestCase("")]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductPriceIsNullOrEmpty(string productPrice)
        {
            //Arrange
            const string name = "Product name";
            const string cost = "42";

            //Act
            _editProductViewModel.ProductName = name;
            _editProductViewModel.ProductPrice = productPrice;
            _editProductViewModel.ProductCost = cost;

            //Assert
            _editProductViewModel.ProductPrice.Should().BeNullOrEmpty("because no value was assigned to product price");
            _editProductViewModel.CanSaveEditedProduct.Should().BeFalse("because the product has no price");
            _editProductViewModel["ProductPrice"].Should().Be("Enter product price", "because no value was assigned to product price");
        }

        [TestCase(null)]
        [TestCase("")]
        public void CanSaveProduct_ShouldReturnFalse_WhenProductCostIsNullOrEmpty(string productCost)
        {
            //Arrange
            const string name = "Product name";
            const string price = "42";

            //Act
            _editProductViewModel.ProductName = name;
            _editProductViewModel.ProductPrice = price;
            _editProductViewModel.ProductCost = productCost;

            //Assert
            _editProductViewModel.ProductCost.Should().BeNullOrEmpty("because no value was assigned to product cost");
            _editProductViewModel.CanSaveEditedProduct.Should().BeFalse("because the product has no cost");
            _editProductViewModel["ProductCost"].Should().Be("Enter product cost", "because no value was assigned to product cost");
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
            _editProductViewModel.ProductName = name;
            _editProductViewModel.ProductCost = cost;
            _editProductViewModel.ProductPrice = productPrice;

            //Assert
            _editProductViewModel.CanSaveEditedProduct.Should().BeFalse($"because the '{productPrice}' is not a number");
            _editProductViewModel["ProductPrice"].Should().Be("Invalid input", $"because '{productPrice}' is not a number");
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
            _editProductViewModel.ProductName = name;
            _editProductViewModel.ProductPrice = price;
            _editProductViewModel.ProductCost = productCost;

            //Assert
            _editProductViewModel.CanSaveEditedProduct.Should().BeFalse($"because '{productCost}' is not a number");
            _editProductViewModel["ProductCost"].Should().Be("Invalid input", $"because '{productCost}' is not a number");
        }
    }
}
