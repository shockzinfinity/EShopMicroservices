using BuildingBlocks.Pagination;
using Microsoft.Identity.Web.Resource;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints;

//public record GetOrdersRequest(PaginationRequest PaginationRequest);
public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/orders",
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:ReadScope")]
    async ([AsParameters] PaginationRequest request, ISender sender) =>
    {
      var result = await sender.Send(new GetOrdersQuery(request));

      var response = result.Adapt<GetOrdersResponse>();

      return Results.Ok(response);
    })
    .RequireAuthorization()
    .WithName("GetOrders")
    .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .WithSummary("Get Orders")
    .WithDescription("Get Orders");
  }
}