﻿using Microsoft.Identity.Web.Resource;

namespace Catalog.API.Products.GetProductByCategory;

//public record GetProductByCategoryRequest();
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/products/category/{category}",
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:ReadScope")]
    async (string category, ISender sender) =>
    {
      var result = await sender.Send(new GetProductByCategoryQuery(category));
      var response = result.Adapt<GetProductByCategoryResponse>();

      return Results.Ok(response);
    })
    .RequireAuthorization()
    .WithName("GetProductByCategory")
    .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .WithSummary("Get Product By Category")
    .WithDescription("Get Product By Category");
  }
}