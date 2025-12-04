using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Shared.Http
{
    public class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context != null)
        {
            if (context.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId))
            {
                request.Headers.Add("X-Correlation-Id", correlationId.ToString());
            }
            else if (context.Items.TryGetValue("X-Correlation-Id", out var correlationIdItem))
            {
                request.Headers.Add("X-Correlation-Id", correlationIdItem?.ToString());
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
}