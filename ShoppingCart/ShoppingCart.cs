using System;
using System.Collections.Generic;
using EventSourcing;
using ShoppingCart.Events;

namespace ShoppingCart
{
    public class ShoppingCart : EventSourcedEntity
    {
        private ShoppingCart(string id, IEnumerable<DomainEvent> pastEvents) : base(id, pastEvents)
        {
        }

        private ShoppingCart(string id, IEnumerable<DomainEvent> pastEvents, string customerId, string userId) : base(id, pastEvents)
        {
            RaiseEvent(new ShoppingCartCreated(userId, customerId));
        }
        
        public static ShoppingCart Create(string customerId, string userId)
        {
            return new ShoppingCart(Guid.NewGuid().ToString(), new List<DomainEvent>(), customerId, userId);
        }

        public void AddItem(string userId, string producCode, int quantity)
        {
            RaiseEvent(new ItemAddedToCart(userId, producCode, quantity));
        }

        public void When(ShoppingCartCreated @event) { }
        
        public void When(ItemAddedToCart @event) { }
    }
}