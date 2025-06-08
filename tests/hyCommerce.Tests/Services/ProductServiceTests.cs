using AutoFixture;
using FluentAssertions;
using hyCommerce.Application.Services;
using hyCommerce.Domain;
using hyCommerce.Domain.Entities;
using hyCommerce.Domain.Entities.Helpers;
using hyCommerce.Domain.Interfaces;
using Moq;

namespace hyCommerce.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;
        private readonly Fixture _fixture;

        public ProductServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object, _unitOfWork.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetProducts_WithValidParams_ReturnsProductsList()
        {
            var productParams = _fixture.Create<ProductParams>();
            var products = _fixture.CreateMany<Product>(3).ToList();

            _productRepositoryMock.Setup(repo => repo.GetProducts(productParams))
                .ReturnsAsync(products);

            var result = await _productService.GetProducts(productParams);

            result.Should().NotBeNull();
            result.Should().HaveCount(3);

            _productRepositoryMock.Verify(repo => repo.GetProducts(It.IsAny<ProductParams>()), Times.Once);
        }

        [Fact]
        public async Task GetProduct_WithValidParam_ReturnProduct()
        {
            //Arrange
            var product = _fixture.Create<Product>();
            product.Id = 1;

            _productRepositoryMock.Setup(repo => repo.GetProduct(1))
                .ReturnsAsync(product);

            //Act
            var result = await _productService.GetProduct(1);

            //Assert
            result.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);

            _productRepositoryMock.Verify(repo => repo.GetProduct(It.IsAny<int>()), Times.Once);
        }
    }
}
