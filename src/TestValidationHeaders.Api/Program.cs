using FluentValidation;
using Microsoft.OpenApi.Models;
using TestValidationHeaders.Api.Filter.Services;
using TestValidationHeaders.Api.Operations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<TenantFilterService>();
builder.Services.AddScoped<UcidFilterService>();

builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo { Title = "TestValidationHeaders", Version = "v1" });
    opts.OperationFilter<AddTenantHeaderParameter>();
    opts.OperationFilter<AddUcidHeaderParameter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestValidationHeaders v1"); });
}

app.MapControllers();
app.UseHttpsRedirection();

await app.RunAsync();