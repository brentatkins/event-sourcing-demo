using System;
using EventSourcing.Tests.TestHelpers;
using ShoppingCart.Events;
using Xunit;

namespace ShoppingCart.Tests
{
    public class ShoppingCartDomainTests : DomainTestBase<ShoppingCart>
    {
        private readonly string customerId = "some customer id";

        [Fact]
        public void CreateCart_CartIsCreated()
        {
            var entityId = Guid.NewGuid().ToString();
            
            Given()
                .When(() => ShoppingCart.Create(entityId, customerId))
                .Then(new ShoppingCartCreated(entityId, customerId));
        }
        
        [Fact]
        public void AddItemToCard_CartIsCreate_ItemIsAdded()
        {
            string productCode = "PRD_CODE";
            int quantity = 10;
            string entityId = Guid.NewGuid().ToString();
            
            Given(new ShoppingCartCreated(entityId, customerId))
                .When(cart => cart.AddItem(productCode, quantity))
                .Then(new ItemAddedToCart(entityId, productCode, quantity));
        }
    }
}