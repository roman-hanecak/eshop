using System.Linq;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;

namespace Visma.Bootcamp.ApiTests.Contexts
{
    public class CatalogContext : BaseContext
    {
        public void AddCatalog(CatalogDto catalogDto)
        {
            ContextDictionary.Add(catalogDto.Name, catalogDto.PublicId);
        }

        public void UpdateCatalog(CatalogDto catalogDto)
        {
            var item = ContextDictionary.FirstOrDefault(kvp => kvp.Value == catalogDto.PublicId);
            if (item.Key != null)
            {
                ContextDictionary.Remove(item.Key);    
            }
            
            AddCatalog(catalogDto);
        }
    }
}
