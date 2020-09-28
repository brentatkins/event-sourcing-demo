using EventSourcing;

namespace ShoppingCart.Events
{
    public class CartOrderPurchased : DomainEvent
    {
        public CartOrderPurchased(string userId) : base(userId)
        {
        }
    }
}