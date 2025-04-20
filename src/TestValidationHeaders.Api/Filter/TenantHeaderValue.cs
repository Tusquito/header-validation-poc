namespace TestValidationHeaders.Api.Filter;

public sealed class TenantHeaderValue(string? tenant)
{
    public string? Value { get; } = tenant;

    public static implicit operator TenantHeaderValue(string? tenant)
    {
        return new TenantHeaderValue(tenant);
    }
}