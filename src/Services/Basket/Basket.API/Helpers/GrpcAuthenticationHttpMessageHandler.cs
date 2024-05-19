using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

namespace Basket.API.Helpers;

public class GrpcAuthenticationHttpMessageHandler(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
  : DelegatingHandler
{
  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    var scopes = new[] { configuration["GrpcSettings:Scopes"]! };

    try
    {
      var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
      request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }
    catch (MicrosoftIdentityWebChallengeUserException ex)
    {
      await tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(scopes, ex.MsalUiRequiredException);
      var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
      request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }
    catch (MsalUiRequiredException ex)
    {
      await tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeaderAsync(scopes, ex);
      var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
      request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }

    return await base.SendAsync(request, cancellationToken);
  }
}