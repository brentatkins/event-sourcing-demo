using System;
using System.Linq;
using EventSourcing;
using EventSourcing.Projections;
using ShoppingCart.Events;

namespace ShoppingCart.Projections
{
    public class ShoppingCartProjection : IProjection<ShoppingCartDto>
    {
        public ShoppingCartDto? Project(ShoppingCartDto? state, DomainEvent @event)
        {
            var result = (state, @event) switch
            {
                (null, ShoppingCartCreated created) => 
                    new ShoppingCartDto(created.EntityId, created.CustomerId, Array.Empty<(string ProductCode, int Quanity)>()),
                
                (not null, ItemAddedToCart itemAddedToCart) => 
                    state with {
                        CartItems = state.CartItems.Append((itemAddedToCart.ProductCode, itemAddedToCart.Quantity)).ToArray()
                    },
                _ => state
            };

            return result;
        }
    }
}