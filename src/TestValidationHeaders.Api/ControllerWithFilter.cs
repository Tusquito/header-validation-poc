using Microsoft.AspNetCore.Mvc;
using TestValidationHeaders.Api.Extensions;
using TestValidationHeaders.Api.Filter.Services;

namespace TestValidationHeaders.Api;

[ApiController]
[Route("api/[controller]")]
public class ControllerWithFilter(IHttpContextAccessor accessor) : ControllerBase
{
    [HttpGet("tenant")]
    [ServiceFilter(typeof(TenantFilterService))]
    public IActionResult GetTenant()
    {
        var tenant = accessor.HttpContext.GetTenantOrDefault();
        return Ok(new { Tenant = tenant.Value });
    }

    [HttpGet("ucid")]
    [ServiceFilter(typeof(UcidFilterService))]
    public IActionResult GetUcid()
    {
        var ucid = accessor.HttpContext.GetUcidOrDefault();
        return Ok(new { Ucid = ucid.Value });
    }

    [HttpGet("ucid-tenant")]
    [ServiceFilter(typeof(UcidFilterService))]
    public IActionResult GetUcidTenant()
    {
        var tenant = accessor.HttpContext.GetTenantOrDefault();
        var ucid = accessor.HttpContext.GetUcidOrDefault();

        return Ok(new { Tenant = tenant.Value, Ucid = ucid.Value });
    }
}