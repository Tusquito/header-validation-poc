namespace TestValidationHeaders.Api.Filter;

public sealed class UcidHeaderValue(string? ucid)
{
    public string? Value { get; } = ucid;

    public static implicit operator UcidHeaderValue(string? ucid)
    {
        return new UcidHeaderValue(ucid);
    }
}