using System;
using EventSourcing;

namespace ShoppingCart.Events
{
    public class ShoppingCartCreated : DomainEvent
    {
        public string CustomerId { get; }

        public ShoppingCartCreated(string entityId, string customerId) : base(entityId)
        {
            CustomerId = customerId;
        }
    }
}