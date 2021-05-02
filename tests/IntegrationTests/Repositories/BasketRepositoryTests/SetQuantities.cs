using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.UnitTests.Builders;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.eShopWeb.IntegrationTests.Repositories.BasketRepositoryTests
{
    public class SetQuantities
    {
        private readonly CatalogContext _catalogContext;
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly BasketBuilder BasketBuilder = new BasketBuilder();

        public SetQuantities()
        {
            var dbOptions = new DbContextOptionsBuilder<CatalogContext>()
                .UseSqlServer("Data Source=localhost; Initial Catalog=TestCatalog; User Id=sa; Password=P@ssW0rd!")
                .Options;
            _catalogContext = new CatalogContext(dbOptions);
            _catalogContext.Database.EnsureCreated();
            _basketRepository = new EfRepository<Basket>(_catalogContext);
        }

        [Fact]
        public async Task RemoveEmptyQuantities()
        {
            var basket = BasketBuilder.WithOneBasketItem();
            var basketService = new BasketService(_basketRepository, null);
            await _basketRepository.AddAsync(basket);
            _catalogContext.SaveChanges();

            await basketService.SetQuantities(BasketBuilder.BasketId, new Dictionary<string, int>() { { BasketBuilder.BasketId.ToString(), 0 } });

            Assert.Equal(0, basket.Items.Count);
        }
    }
}
