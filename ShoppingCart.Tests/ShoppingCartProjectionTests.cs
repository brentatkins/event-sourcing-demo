using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FluentAssertions;
using ShoppingCart.Events;
using ShoppingCart.Projections;
using Xunit;

namespace ShoppingCart.Tests
{
    public class ShoppingCartProjectionTests : ProjectionTestBase<ShoppingCartProjection, ShoppingCartDto>
    {
        [Fact]
        public void ShoppingCartCreated_ShouldCreateCard()
        {
            var id = "some id";
            var userId = "some user id";
            var customerId = "some customer id";
            
            this.Given(new ShoppingCartCreated(id, userId, customerId))
                .Then(new ShoppingCartDto(id, customerId, Array.Empty<(string ProductCode, int Quanity)>()));
        }
        
        [Fact]
        public void ItemAddedToCart_NotAddedBefore_ShouldBeInCart()
        {
            var id = "999";
            var userId = "user1";
            var customerId = "cust1";
            var productCode = "prod1";
            var quantity = 10;

            this.Given(new ShoppingCartCreated(id, userId, customerId),
                    new ItemAddedToCart(id, userId, productCode, quantity))
                .Then(id, cart =>
                {
                    cart.Should().NotBeNull();
                    cart!.CartItems.Should().Equal((productCode, quantity));
                });
        }
    }
}