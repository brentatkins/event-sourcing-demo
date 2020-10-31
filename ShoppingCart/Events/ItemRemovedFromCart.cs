using EventSourcing;

namespace ShoppingCart.Events
{
    public class ItemRemovedFromCart : DomainEvent
    {
        public string ProductCode { get; }
        public int Quantity { get; }

        public ItemRemovedFromCart(string entityId, string productCode, int quantity) : base(entityId)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}