﻿using Microsoft.Identity.Web.Resource;

namespace Catalog.API.Products.DeleteProduct;

//public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapDelete("/products/{id}",
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:WriteScope")]
    async (Guid id, ISender sender) =>
    {
      var result = await sender.Send(new DeleteProductCommand(id));

      var response = result.Adapt<DeleteProductResponse>();

      return Results.Ok(response);
    })
    .RequireAuthorization()
    .WithName("DeleteProduct")
    .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .ProducesProblem(StatusCodes.Status404NotFound)
    .WithSummary("Delete Product")
    .WithDescription("Delete Product");
  }
}