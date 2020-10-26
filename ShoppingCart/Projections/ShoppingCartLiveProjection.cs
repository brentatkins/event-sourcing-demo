using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EventSourcing;
using EventSourcing.Projections;
using ShoppingCart.Events;

namespace ShoppingCart.Projections
{
    public class ShoppingCartLiveProjection : LiveProjection<ApplicationState>
    {
        public ShoppingCartLiveProjection(IEnumerable<DomainEvent> events) : base(events)
        {
        }
        
        protected override ApplicationState Handle(ApplicationState state, DomainEvent @event)
        {
            return @event switch
            {
                ShoppingCartCreated shoppingCartCreated => state with {
                    ShoppingCarts = state.ShoppingCarts.Add(shoppingCartCreated.EntityId,
                        new ShoppingCartDto(
                            shoppingCartCreated.EntityId,
                            shoppingCartCreated.CustomerId,
                            Array.Empty<(string ProductCode, int Quanity)>()))},
                
                
                ItemAddedToCart itemAddedToCart => this.ItemAddedToCart(state, itemAddedToCart),
                
                _ => state
            };
        }
        
        protected override ApplicationState GetEmptyState()
        {
            return new ApplicationState(ImmutableDictionary<string, ShoppingCartDto>.Empty);
        }

        private ApplicationState ItemAddedToCart(ApplicationState state, ItemAddedToCart itemAddedToCart)
        {
            var originalCart = state.ShoppingCarts[itemAddedToCart.EntityId];
            var newCart = originalCart with { CartItems = originalCart.CartItems
                .Append((itemAddedToCart.ProductCode, itemAddedToCart.Quantity)).ToArray()};

            return state with {
                ShoppingCarts = state.ShoppingCarts.SetItem(itemAddedToCart.EntityId, newCart)};
        }
    }
}