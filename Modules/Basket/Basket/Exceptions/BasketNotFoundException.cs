using Shared.Exceptions;

namespace Basket.Exceptions;
public class BasketNotFoundException(string userName)
    : NotFoundException("ShoppingCart", userName)
{
}
