using EventSourcing;

namespace ShoppingCart.Events
{
    public class ItemRemovedFromCart : DomainEvent
    {
        public string ProductCode { get; }
        public int Quantity { get; }

        public ItemRemovedFromCart(string userId, string productCode, int quantity) : base(userId)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}