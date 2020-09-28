using ShoppingCart.Events;
using Xunit;

namespace ShoppingCart.Tests
{
    public class ShoppingCartDomainTests : DomainTest<ShoppingCart>
    {
        private readonly string userId = "some user id";
        private readonly string customerId = "some customer id";

        [Fact]
        public void CreateCart_CartIsCreated()
        {
            Given()
                .When(() => ShoppingCart.Create(customerId, userId))
                .Then(new ShoppingCartCreated(userId, customerId));
        }
        
        [Fact]
        public void AddItemToCard_CartIsCreate_ItemIsAdded()
        {
            string productCode = "PRD_CODE";
            int quantity = 10;
            
            Given(new ShoppingCartCreated(userId, customerId))
                .When(cart => cart.AddItem(userId, productCode, quantity))
                .Then(new ItemAddedToCart(userId, productCode, quantity));
        }
    }
}