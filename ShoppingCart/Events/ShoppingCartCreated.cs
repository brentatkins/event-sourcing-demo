using EventSourcing;

namespace ShoppingCart.Events
{
    public class ShoppingCartCreated : DomainEvent
    {
        public string CustomerId { get; }

        public ShoppingCartCreated(string userId, string customerId) : base(userId)
        {
            CustomerId = customerId;
        }
    }
}