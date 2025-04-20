using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TestValidationHeaders.Api.Filter.Services;

namespace TestValidationHeaders.Api.Operations;

public sealed class AddUcidHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        var attribute = context.MethodInfo.GetCustomAttribute<ServiceFilterAttribute<UcidFilterService>>() ??
                        context.MethodInfo.DeclaringType
                            ?.GetCustomAttribute<ServiceFilterAttribute<UcidFilterService>>();

        if (attribute == null)
        {
            var attributes = context.MethodInfo.GetCustomAttributes<ServiceFilterAttribute>().ToList();

            if (attributes.Count == 0)
            {
                attributes = context.MethodInfo.DeclaringType?.GetCustomAttributes<ServiceFilterAttribute>().ToList() ??
                             [];
            }

            if (attributes.Count == 0 || attributes.All(x => x.ServiceType != typeof(UcidFilterService)))
            {
                return;
            }
        }

        var existingParameter =
            operation.Parameters.FirstOrDefault(x =>
                x.Name == Constants.UcidHeader && x.In == ParameterLocation.Header);

        if (existingParameter != null)
        {
            operation.Parameters.Remove(existingParameter);
        }

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = Constants.UcidHeader,
            In = ParameterLocation.Header,
            Required = true,
            Description = "Ucid header",
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}