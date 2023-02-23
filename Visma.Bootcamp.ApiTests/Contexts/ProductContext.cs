using System.Linq;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.ApiTests.Contexts
{
    public class ProductContext : BaseContext
    {
        public void AddProduct(ProductDto catalogDto)
        {
            ContextDictionary.Add(catalogDto.Name, catalogDto.PublicId);
        }

        public void UpdateProduct(ProductDto catalogDto)
        {
            var item = ContextDictionary.FirstOrDefault(kvp => kvp.Value == catalogDto.PublicId);
            if (item.Key != null)
            {
                ContextDictionary.Remove(item.Key);    
            }
            
            AddProduct(catalogDto);
        }
    }
}
