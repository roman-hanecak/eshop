using System;
using RestSharp;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.ApiTests.Actions
{
    public class ProductActions : MainAppActionBase
    {
        public ProductActions() : base("Products")
        {
        }

        public RestResponse<ProductDto> GetProduct(Guid id)
        {
            var request = CreateRequest(Method.Get, $"{id}");
            var result = Execute<ProductDto>(request);

            if (result.Data != null)
            {
                result.Data = RemoveUniqueIdentifier(result.Data);
            }

            return result;
        }

        public RestResponse<ProductDto> UpdateProduct(ProductDto model, Guid id)
        {
            var request = CreateRequest(Method.Put, id.ToString());
            request.AddJsonBody(AddUniqueIdentifier(model));
            // using ProductDto instead of ProductModel because of id for update
            var result = Execute<ProductDto>(request);

            if (result.Data != null)
            {
                result.Data = RemoveUniqueIdentifier(result.Data);
            }

            return result;
        }
    }
}
