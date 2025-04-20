using TestValidationHeaders.Api.Filter;

namespace TestValidationHeaders.Api.Extensions;

public static class HttpContextExtensions
{
    public static string? GetStringHeaderOrDefault(this HttpContext? ctx, string headerName, string? defaultValue = null)
    {
        ArgumentNullException.ThrowIfNull(ctx);

        return !ctx.Request.Headers.TryGetValue(headerName, out var headerValue)
            ? defaultValue
            : headerValue.FirstOrDefault() ?? defaultValue;
    }
    
    public static TenantHeaderValue GetTenantOrDefault(this HttpContext? ctx)
    {
        return ctx.GetStringHeaderOrDefault(Constants.TenantHeader);
    }
    
    public static UcidHeaderValue GetUcidOrDefault(this HttpContext? ctx)
    {
        return ctx.GetStringHeaderOrDefault(Constants.UcidHeader);
    }
    
    
}