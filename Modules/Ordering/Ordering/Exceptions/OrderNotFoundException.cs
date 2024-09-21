using Shared.Exceptions;

namespace Ordering.Exceptions;
public class OrderNotFoundException(Guid orderId)
    : NotFoundException("Order", orderId)
{
}