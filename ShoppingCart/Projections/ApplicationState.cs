using System.Collections.Immutable;

namespace ShoppingCart.Projections
{
    public record ApplicationState(
        ImmutableDictionary<string, ShoppingCartDto> ShoppingCarts);
}