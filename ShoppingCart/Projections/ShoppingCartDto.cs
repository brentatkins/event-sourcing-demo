using System.Collections.Immutable;
using EventSourcing.Projections;

namespace ShoppingCart.Projections
{
    public record ShoppingCartDto(
            string Id, 
            string CustomerId, 
            (string ProductCode, int Quanity)[] CartItems)
        : Dto(Id);
}