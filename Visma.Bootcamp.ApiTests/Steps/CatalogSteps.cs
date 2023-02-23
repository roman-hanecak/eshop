using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CatalogSteps : BaseSteps
    {
        private readonly CatalogContext _catalogContext;
        private readonly ProductContext _productContext;
        private CatalogDto _catalogDto;
        private RestResponse<CatalogDto> _catalogResponse;
        private RestResponse _lastResponse;
        private RestResponse<object> _catalogDeleteResponse;
        private RestResponse<List<CatalogBaseDto>> _catalogListResponse;
        private RestResponse<ProductDto> _productResponse;
        private ProductDto _productDto;

        public CatalogSteps(CatalogContext catalogContext, ProductContext productContext)
        {
            _catalogContext = catalogContext;
            _productContext = productContext;
        }
        
        [Given("I create new catalog with values: name-'(.*)' description-'(.*)'")]
        [When("I create new catalog with values: name-'(.*)' description-'(.*)'")]
        public void GivenICreateNewCatalogWithValues(string name, string description)
        {
            StartCreatingNewCatalog();
            SetCatalogMandatoryValues(name, description);
            CreateThisCatalog();
            VerifyResponse(_lastResponse, HttpStatusCode.Created);
        }
        
        [Given("I create new catalog with values to fail: name-'(.*)' description-'(.*)'")]
        [When("I create new catalog with values to fail: name-'(.*)' description-'(.*)'")]
        public void GivenICreateNewCatalogWithValuesToFail(string name, string description)
        {
            StartCreatingNewCatalog();
            SetCatalogMandatoryValues(name, description);
            CreateThisCatalog();
        }
        
        [Given("I start creating new catalog")]
        [When("I start creating new catalog")]
        public void GivenIStartCreatingNewCatalog()
        {
            StartCreatingNewCatalog();
        }
        
        [Given("I set catalog mandatory values: name-'(.*)' description-'(.*)'")]
        [When("I set catalog mandatory values: name-'(.*)' description-'(.*)'")]
        public void GivenISetCatalogMandatoryValues(string name, string description)
        {
            SetCatalogMandatoryValues(name, description);
        }
        
        [Given("I add product to catalog-'(.*)' name-'(.*)' description-'(.*)' price-'(.*)'")]
        [When("I add product to catalog-'(.*)' name-'(.*)' description-'(.*)' price-'(.*)'")]
        public void GivenIAddProductToCatalog(string catalog, string name, string description, string price)
        {
            AddProductToCatalog(catalog, name, description, price);
            VerifyResponse(_lastResponse, HttpStatusCode.Created);
        }
        
        [Given("I add product to fail to catalog-'(.*)' name-'(.*)' description-'(.*)' price-'(.*)'")]
        [When("I add product to fail to catalog-'(.*)' name-'(.*)' description-'(.*)' price-'(.*)'")]
        public void GivenIAddProductToFailToCatalog(string catalog, string name, string description, string price)
        {
            AddProductToCatalog(catalog, name, description, price);
        }

        [Given("I update this catalog")]
        [When("I update this catalog")]
        public void GivenIUpdateThisCatalog()
        {
            UpdateThisCatalog();
            VerifyResponse(_lastResponse, HttpStatusCode.OK);
        }
        
        [Given("I update this catalog to fail")]
        [When("I update this catalog to fail")]
        public void GivenIUpdateThisCatalogToFail()
        {
            UpdateThisCatalog();
        }
        
        [Given("I delete catalog name-'(.*)'")]
        [When("I delete catalog name-'(.*)'")]
        public void GivenIDeleteCatalog(string name)
        {
            DeleteCatalog(_catalogContext.GetIdOrDefault(name));
        }
        
        [Given("I retrieve catalog: name-'(.*)'")]
        [When("I retrieve catalog: name-'(.*)'")]
        public void GivenIRetrieveCatalog(string name)
        {
            RetrieveCatalog(_catalogContext.GetIdOrDefault(name));
            VerifyResponse(_lastResponse, HttpStatusCode.OK);
        }
        
        [Given("I retrieve catalog to fail: name-'(.*)'")]
        [When("I retrieve catalog to fail: name-'(.*)'")]
        public void GivenIRetrieveCatalogToFail(string name)
        {
            RetrieveCatalog(_catalogContext.GetIdOrDefault(name));
        }
        
        [Given("I retrieve all catalogs")]
        [When("I retrieve all catalogs")]
        public void GivenIRetrieveAllCatalogs()
        {
            RetrieveAllCatalogs();
            VerifyResponse(_lastResponse, HttpStatusCode.OK);
        }

        [Then("I see catalog values: name-'(.*)' description-'(.*)'")]
        public void ThenISeeCatalogValues(string name, string description)
        {
            Assert.That(_catalogDto.Name, Is.EqualTo(name), $"Name does not match");
            Assert.That(_catalogDto.Description, Is.EqualTo(description), $"Description does not match");
        }

        [Then("I see catalog last response is status-'(.*)'")]
        public void ThenISeeCatalogLastResponseIsStatus(HttpStatusCode status)
        {
            VerifyResponse(_lastResponse, status);
        }

        [Then("I see catalog list contains catalog: name-'(.*)' description-'(.*)'")]
        public void ThenISeeCatalogListContainsCatalog(string name, string description)
        {
            var firstOrDefault = _catalogListResponse.Data.FirstOrDefault(x => x.Name == name);

            Assert.NotNull(firstOrDefault, 
                $"Catalog [{name}] not found in list [{string.Join(",", _catalogListResponse.Data.Select(x => x.Name))}]");
            Assert.That(firstOrDefault.Name, Is.EqualTo(name), $"Name does not match");
            Assert.That(firstOrDefault.Description, Is.EqualTo(description), $"Description does not match");
        }

        [Then("I see items in the list count-'(.*)'")]
        public void ThenISeeItemsInTheListCount(int count)
        {
            Assert.That(_catalogListResponse.Data.Count, Is.EqualTo(count), $"Count does not match");
        }

        [Then("I see catalog product list values: name-'(.*)' description-'(.*)' price-'(.*)'")]
        public void ThenISeeCatalogProductListValues(string name, string description, string price)
        {
            var firstOrDefault = _catalogResponse.Data.Products.FirstOrDefault(x => x.Name == name);

            Assert.NotNull(firstOrDefault,
                $"Product [{name}] not found in list [{string.Join(",", _catalogResponse.Data.Products.Select(x => x.Name))}]");
            AssertProductValues(firstOrDefault, name, description, price);
        }

        [Then("I see catalog product values: name-'(.*)' description-'(.*)' price-'(.*)'")]
        public void ThenISeeCatalogProductValues(string name, string description, string price)
        {
            AssertProductValues(_productDto, name, description, price);
        }
        
        #region Private

        private void StartCreatingNewCatalog()
        {
            _catalogDto = new CatalogDto();
        }

        private void SetCatalogMandatoryValues(string name, string description)
        {
            _catalogDto.Name = name;
            _catalogDto.Description = description;
        }

        private void CreateThisCatalog()
        {
            _catalogResponse = Call.Catalog.CreateCatalog(_catalogDto);
            _lastResponse = _catalogResponse;

            if (_catalogResponse.IsSuccessful)
            {
                _catalogContext.AddCatalog(_catalogResponse.Data);
                _catalogDto = _catalogResponse.Data;
            }
        }

        private void AddProductToCatalog(string catalog, string name, string description, string price)
        {
            var productModel = new ProductDto()
            {
                Name = name,
                Description = description,
                Price = Decimal.Parse(price)
            };

            _productResponse = Call.Catalog.AddProductToCatalog(_catalogContext.GetIdOrDefault(catalog), productModel);
            _lastResponse = _productResponse;

            if (_productResponse.IsSuccessful)
            {
                _productContext.AddProduct(_productResponse.Data);
                _productDto = _productResponse.Data;
            }
        }

        private void UpdateThisCatalog()
        {
            _catalogResponse = Call.Catalog.UpdateCatalog(_catalogDto, _catalogDto.PublicId);
            _lastResponse = _catalogResponse;

            if (_catalogResponse.IsSuccessful)
            {
                _catalogContext.UpdateCatalog(_catalogResponse.Data);
                _catalogDto = _catalogResponse.Data;
            }
        }

        private void DeleteCatalog(Guid guid)
        {
            _catalogDeleteResponse = Call.Catalog.DeleteCatalog(guid);
            _lastResponse = _catalogDeleteResponse;
        }

        private void RetrieveCatalog(Guid guid)
        {
            _catalogResponse = Call.Catalog.GetCatalog(guid);
            _lastResponse = _catalogResponse;
            
            if (_catalogResponse.IsSuccessful)
            {
                _catalogDto = _catalogResponse.Data;
            }
        }

        private void RetrieveAllCatalogs()
        {
            _catalogListResponse = Call.Catalog.GetAllCatalogs();
            _lastResponse = _catalogListResponse;
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
