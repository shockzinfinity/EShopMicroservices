namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler(IApplicationDbContext dbContext)
  : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
  public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
  {
    var orderId = OrderId.Of(command.OrderId);
    var order = await dbContext.Orders.FindAsync([orderId], cancellationToken: cancellationToken); // findasync 를 쓰는 이유는 PK 를 사용하기 위함과 strong type id 를 사용하기 위함

    if (order is null)
    {
      throw new OrderNotFoundException(command.OrderId);
    }

    dbContext.Orders.Remove(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new DeleteOrderResult(true);
  }
}