using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TestValidationHeaders.Api.Filter.Services;

namespace TestValidationHeaders.Api.Operations;

public sealed class AddTenantHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        // Check if method or class has the right attribute
        var attribute = context.MethodInfo.GetCustomAttribute<ServiceFilterAttribute<TenantFilterService>>() ??
                        context.MethodInfo.DeclaringType
                            ?.GetCustomAttribute<ServiceFilterAttribute<TenantFilterService>>();

        if (attribute == null)
        {
            var attributes = context.MethodInfo.GetCustomAttributes<ServiceFilterAttribute>().ToList();

            if (attributes.Count == 0)
            {
                attributes = context.MethodInfo.DeclaringType?.GetCustomAttributes<ServiceFilterAttribute>().ToList() ??
                             [];
            }

            if (attributes.Count == 0 || attributes.All(x => x.ServiceType != typeof(TenantFilterService)))
            {
                return;
            }
        }

        var existingParameter =
            operation.Parameters.FirstOrDefault(x =>
                x.Name == Constants.TenantHeader && x.In == ParameterLocation.Header);

        if (existingParameter != null)
        {
            operation.Parameters.Remove(existingParameter);
        }

        // Add the Tenant header parameter
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = Constants.TenantHeader,
            In = ParameterLocation.Header,
            Required = true,
            Description = "Tenant header",
            Example = new OpenApiString("ONEID"),
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString("ONEID")
            }
        });
    }
}