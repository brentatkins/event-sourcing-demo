using System.Collections.Immutable;
using EventSourcing;
using EventSourcing.Projections;
using ShoppingCart.Events;

namespace ShoppingCart.Tests.TempProj
{
    public class ShoppingCartProjection : IProjection<ShoppingCartDto>
    {
        public ShoppingCartDto? Project(ShoppingCartDto? state, DomainEvent @event)
        {
            var result = (state, @event) switch
            {
                (null, ShoppingCartCreated created) => 
                    new ShoppingCartDto(created.CustomerId, ImmutableDictionary<string, int>.Empty),
                
                (not null, ItemAddedToCart itemAddedToCart) => 
                    state with {
                        CartItems = state.CartItems.ContainsKey(itemAddedToCart.ProductCode) 
                            ? state.CartItems.SetItem(itemAddedToCart.ProductCode, state.CartItems[itemAddedToCart.ProductCode] + itemAddedToCart.Quantity)
                            : state.CartItems.Add(itemAddedToCart.ProductCode, itemAddedToCart.Quantity)
                    },
                _ => state
            };

            return result;
        }
    }

    public record ShoppingCartDto(string CustomerId, ImmutableDictionary<string, int> CartItems)
        : Dto(CustomerId);
}