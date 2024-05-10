using Catalog.API.Products.GetProducts;
using Microsoft.Identity.Web.Resource;

namespace Catalog.API.Products.GetProductById;

//public record GetProductByIdRequest();
public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/products/{id}",
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:WriteScope")]
    async (Guid id, ISender sender) =>
    {
      var result = await sender.Send(new GetProductByIdQuery(id));

      var response = result.Adapt<GetProductByIdResponse>();

      return Results.Ok(response);
    })
    .RequireAuthorization()
    .WithName("GetProductById")
    .Produces<GetProductsResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .WithSummary("Get Product By Id")
    .WithDescription("Get Product By Id");
  }
}