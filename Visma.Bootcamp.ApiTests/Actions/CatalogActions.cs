using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using Visma.Bootcamp.ApiTests.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.ApiTests.Actions
{
    public class CatalogActions : MainAppActionBase
    {
        public CatalogActions() : base("Catalogs")
        {
        }

        public RestResponse<CatalogDto> CreateCatalog(CatalogDto model)
        {
            var request = CreateRequest(Method.Post, "");
            request.AddJsonBody(AddUniqueIdentifier(model));
            // using CatalogDto instead of CatalogModel because of id for update 
            var result = Execute<CatalogDto>(request);

            if (result.Data != null)
            {
                result.Data = RemoveUniqueIdentifier(result.Data);
            }

            return result;
        }

        public RestResponse<ProductDto> AddProductToCatalog(Guid id, ProductDto model)
        {
            var request = CreateRequest(Method.Post, $"{id}/products");
            request.AddJsonBody(AddUniqueIdentifier(model));
            // using CatalogDto instead of CatalogModel because of id for update
            var result = Execute<ProductDto>(request);

            if (result.Data != null)
            {
                result.Data = RemoveUniqueIdentifier(result.Data);
            }

            return result;
        }

        public RestResponse<CatalogDto> UpdateCatalog(CatalogDto model, Guid id)
        {
            var request = CreateRequest(Method.Put, id.ToString());
            request.AddJsonBody(AddUniqueIdentifier(model));
            // using CatalogDto instead of CatalogModel because of id for update
            var result = Execute<CatalogDto>(request);

            if (result.Data != null)
            {
                result.Data = RemoveUniqueIdentifier(result.Data);
            }

            return result;
        }

        public RestResponse<object> DeleteCatalog(Guid id)
        {
            var request = CreateRequest(Method.Delete, id.ToString());
            var result = Execute<object>(request);
            return result;
        }

        public RestResponse<CatalogDto> GetCatalog(Guid id)
        {
            var request = CreateRequest(Method.Get, $"{id}");
            var result = Execute<CatalogDto>(request);

            if (result.Data != null)
            {
                result.Data = RemoveUniqueIdentifier(result.Data);
            }

            return result;
        }

        public RestResponse<List<CatalogBaseDto>> GetAllCatalogs()
        {
            var request = CreateRequest(Method.Get, "");
            var result = Execute<List<CatalogBaseDto>>(request);

            if (result.Data != null)
            {
                // filter out catalogs from other tests
                result.Data = result.Data.Where(x => x.Name.Contains(Current.TestScenarioCorrelationId.ToString())).ToList();
                result.Data.ForEach(c => RemoveUniqueIdentifier(c));
            }

            return result;
        }
    }
}
