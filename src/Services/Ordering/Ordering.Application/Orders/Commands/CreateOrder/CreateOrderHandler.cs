namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IApplicationDbContext dbContext)
  : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
  public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
  {
    // create order entity from command object
    // save to database - use (not reference!!)
    // return result

    var order = CreateNewOrder(command.Order);

    dbContext.Orders.Add(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new CreateOrderResult(order.Id.Value);
  }

  private Order CreateNewOrder(OrderDto orderDto)
  {
    // TODO: custom mapping

    var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.City, orderDto.ShippingAddress.ZipCode);
    var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.City, orderDto.BillingAddress.ZipCode);

    var newOrder = Order.Create(
      id: OrderId.Of(Guid.NewGuid()),
      customerId: CustomerId.Of(orderDto.CustomerId),
      orderName: OrderName.Of(orderDto.OrderName),
      shippingAddress: shippingAddress,
      billingAddress: billingAddress,
      payment: Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.Paymethod));

    foreach (var orderItemDto in orderDto.OrderItems)
    {
      newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
    }

    return newOrder;
  }
}