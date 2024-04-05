﻿namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler(IApplicationDbContext dbContext)
  : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
  public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
  {
    var orderId = OrderId.Of(command.Order.Id);
    var order = await dbContext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);

    if (order is null)
    {
      throw new OrderNotFoundException(command.Order.Id);
    }

    UpdateOrderWithNewValue(order, command.Order);

    dbContext.Orders.Update(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new UpdateOrderResult(true);
  }

  private void UpdateOrderWithNewValue(Order order, OrderDto orderDto)
  {
    var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.City, orderDto.ShippingAddress.ZipCode);
    var updatedBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.City, orderDto.BillingAddress.ZipCode);
    var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.Paymethod);

    order.Update(
      orderName: OrderName.Of(orderDto.OrderName),
      shippingAddress: updatedShippingAddress,
      billingAddress: updatedBillingAddress,
      payment: updatedPayment,
      status: orderDto.Status);
  }
}