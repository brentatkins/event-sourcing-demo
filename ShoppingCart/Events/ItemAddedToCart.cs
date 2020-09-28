﻿using EventSourcing;

namespace ShoppingCart.Events
{
    public class ItemAddedToCart : DomainEvent
    {
        public string ProductCode { get; }
        public int Quantity { get; }

        public ItemAddedToCart(string userId, string productCode, int quantity) : base(userId)
        {
            ProductCode = productCode;
            Quantity = quantity;
        }
    }
}