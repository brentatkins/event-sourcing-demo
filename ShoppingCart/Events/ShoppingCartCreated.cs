using System;
using EventSourcing;

namespace ShoppingCart.Events
{
    public class ShoppingCartCreated : DomainEvent
    {
        public string CustomerId { get; }

        public ShoppingCartCreated(string entityId, string userId, string customerId) : base(entityId, userId)
        {
            CustomerId = customerId;
        }
    }
}