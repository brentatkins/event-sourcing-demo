using EventSourcing;

namespace ShoppingCart.Events
{
    public class ItemRemovedFromCart : DomainEvent
    {
        public string ProductCode { get; }
        public int Quantity { get; }

        public ItemRemovedFromCart(string entityId, string userId, string productCode, int quantity) : base(entityId, userId)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}