using AutoFixture;
using eCommerceAPI.API.RequestHelpers;
using eCommerceAPI.Core.Models;
using eCommerceAPI.Core.Services;
using eCommerceAPI.Core.Services.Interfaces;
using eCommerceAPI.Infrastructures.Repositories;
using FluentAssertions;
using Moq;

namespace eCommerceAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;
        private readonly Fixture _fixture;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object);
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

            result.Data.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
            result.Data.Should().HaveCount(3);

            _productRepositoryMock.Verify(repo => repo.GetProducts(It.IsAny<ProductParams>()), Times.Once);
        }
    }
}
