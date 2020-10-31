using EventSourcing;

namespace ShoppingCart.Events
{
    public class ItemAddedToCart : DomainEvent
    {
        public string ProductCode { get; }
        public int Quantity { get; }

        public ItemAddedToCart(string entityId, string productCode, int quantity) : base(entityId)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}