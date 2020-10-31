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

        private ShoppingCart(string id, IEnumerable<DomainEvent> pastEvents, string customerId) : base(id, pastEvents)
        {
            RaiseEvent(new ShoppingCartCreated(this.Id, customerId));
        }
        
        public static ShoppingCart Create(string entityId, string customerId)
        {
            return new ShoppingCart(entityId, new List<DomainEvent>(), customerId);
        }

        public void AddItem(string producCode, int quantity)
        {
            RaiseEvent(new ItemAddedToCart(this.Id, producCode, quantity));
        }
    }
}