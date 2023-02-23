using System;
using System.Net;
using NUnit.Framework;
using RestSharp;
using TechTalk.SpecFlow;
using Visma.Bootcamp.ApiTests.Contexts;
using Visma.Bootcamp.ApiTests.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.ApiTests.Steps
{
    [Binding]
    public class ProductSteps : BaseSteps
    {
        private readonly CatalogContext _catalogContext;
        private readonly ProductContext _productContext;
        private RestResponse _lastResponse;
        private RestResponse<ProductDto> _productResponse;
        private ProductDto _productDto;

        public ProductSteps(CatalogContext catalogContext, ProductContext productContext)
        {
            _catalogContext = catalogContext;
            _productContext = productContext;
        }
        
        [Given("I retrieve product: name-'(.*)'")]
        [When("I retrieve product: name-'(.*)'")]
        public void GivenIRetrieveProduct(string name)
        {
            RetrieveProduct(_productContext.GetIdOrException(name));
            VerifyResponse(_lastResponse, HttpStatusCode.OK);
        }
        
        [Given("I retrieve product to fail: name-'(.*)'")]
        [When("I retrieve product to fail: name-'(.*)'")]
        public void GivenIRetrieveProductToFail(string name)
        {
            RetrieveProduct(_productContext.GetIdOrDefault(name));
        }

        [Then("I see product values: name-'(.*)' description-'(.*)' price-'(.*)'")]
        public void ThenISeeProductValues(string name, string description, string price)
        {
            AssertProductValues(_productDto, name, description, price);
        }

        [Then("I see product last response is status-'(.*)'")]
        public void ThenISeeProductLastResponseIsStatus(HttpStatusCode status)
        {
            VerifyResponse(_lastResponse, status);
        }
        
        #region Private
        
        private void StartCreatingNewProduct()
        {
            _productDto = new ProductDto();
        }
        
        private void SetProductMandatoryValues(string name, string description, string price)
        {
            _productDto.Name = name;
            _productDto.Description = description;
            _productDto.Price = Decimal.Parse(price);
        }
        
        private void UpdateThisProduct()
        {
            _productResponse = Call.Product.UpdateProduct(_productDto, _productDto.PublicId);
            _lastResponse = _productResponse;

            if (_productResponse.IsSuccessful)
            {
                _productContext.UpdateProduct(_productResponse.Data);
                _productDto = _productResponse.Data;
            }
        }
        
        private void RetrieveProduct(Guid guid)
        {
            _productResponse = Call.Product.GetProduct(guid);
            _lastResponse = _productResponse;

            if (_productResponse.IsSuccessful)
            {
                _productDto = _productResponse.Data;
            }
        }
        
        private void DeleteProduct(Guid guid)
        {
            // TODO: add implementation
        }

        private void AssertProductValues(ProductDto productDto, string name, string description, string price)
        {
            Assert.That(productDto.Name, Is.EqualTo(name), $"Name does not match");
            Assert.That(productDto.Description, Is.EqualTo(description), $"Description does not match");
            Assert.That(productDto.Price, Is.EqualTo(Decimal.Parse(price)), $"Price does not match");
        }
        
        #endregion
    }
}
