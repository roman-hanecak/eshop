using RestSharp;
using Visma.Bootcamp.ApiTests.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.ApiTests.Actions
{
    public class MainAppActionBase : ActionBase
    {
        public MainAppActionBase(string resource) : base(resource)
        { }

        protected override RestResponse<T> Execute<T>(RestRequest request)
        {            
            var restClient = new RestClient("https://localhost:5001/api/");

            var response = restClient.Execute<T>(request);

            return response;
        }

        protected object AddUniqueIdentifier(CatalogDto model)
        {
            // add suffix to Name to ensure uniqueness
            model.Name += Current.TestScenarioCorrelationId;
            return model;
        }

        protected object AddUniqueIdentifier(ProductDto model)
        {
            // add suffix to Name to ensure uniqueness
            model.Name += Current.TestScenarioCorrelationId;
            return model;
        }

        protected CatalogDto RemoveUniqueIdentifier(CatalogDto catalog)
        {
            // remove suffix from Name to make it appear for the test the same
            catalog.Name = GetCleanName(catalog.Name);
            if (catalog.Products != null)
            {
                catalog.Products.ForEach(p => RemoveUniqueIdentifier(p));    
            }
            
            return catalog;
        }
        
        protected CatalogBaseDto RemoveUniqueIdentifier(CatalogBaseDto catalog)
        {
            // remove suffix from Name to make it appear for the test the same
            catalog.Name = GetCleanName(catalog.Name);
            return catalog;
        }
        
        protected ProductDto RemoveUniqueIdentifier(ProductDto catalog)
        {
            // remove suffix from Name to make it appear for the test the same
            catalog.Name = GetCleanName(catalog.Name);
            return catalog;
        }

        private string GetCleanName(string nameWithId)
        {
            return nameWithId.Replace(Current.TestScenarioCorrelationId.ToString(), "");
        }
    }
}
