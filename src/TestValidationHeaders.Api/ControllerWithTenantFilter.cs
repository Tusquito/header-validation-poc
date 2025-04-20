using Microsoft.AspNetCore.Mvc;
using TestValidationHeaders.Api.Extensions;
using TestValidationHeaders.Api.Filter.Services;

namespace TestValidationHeaders.Api;

[ApiController]
[ServiceFilter(typeof(TenantFilterService))]
[Route("api/[controller]")]
public class ControllerWithTenantFilter(IHttpContextAccessor accessor) : ControllerBase
{
    [HttpGet("tenant")]
    public IActionResult Get()
    {
        var tenant = accessor.HttpContext.GetTenantOrDefault();
        return Ok(new { Tenant = tenant.Value });
    }
}