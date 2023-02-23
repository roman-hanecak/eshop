using Visma.Bootcamp.ApiTests.Actions;

namespace Visma.Bootcamp.ApiTests.Infrastructure
{
    public static class Call
    {
        public static CatalogActions Catalog = new CatalogActions();
        public static ProductActions Product = new ProductActions();
    }
}
