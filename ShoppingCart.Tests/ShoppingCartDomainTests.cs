using System;
using EventSourcing.Tests.TestHelpers;
using ShoppingCart.Events;
using Xunit;

namespace ShoppingCart.Tests
{
    public class ShoppingCartDomainTests : DomainTestBase<ShoppingCart>
    {
        private readonly string userId = "some user id";
        private readonly string customerId = "some customer id";

        [Fact]
        public void CreateCart_CartIsCreated()
        {
            var entityId = Guid.NewGuid().ToString();
            
            Given()
                .When(() => ShoppingCart.Create(entityId, customerId, userId))
                .Then(new ShoppingCartCreated(entityId, userId, customerId));
        }
        
        [Fact]
        public void AddItemToCard_CartIsCreate_ItemIsAdded()
        {
            string productCode = "PRD_CODE";
            int quantity = 10;
            string entityId = Guid.NewGuid().ToString();
            
            Given(new ShoppingCartCreated(entityId, userId, customerId))
                .When(cart => cart.AddItem(userId, productCode, quantity))
                .Then(new ItemAddedToCart(entityId, userId, productCode, quantity));
        }
    }
}