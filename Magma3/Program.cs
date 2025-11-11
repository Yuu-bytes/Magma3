using Magma3.Application;
using Magma3.Config;
using Magma3.Infraestructure;
using Magma3.WebClient;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var rateLimitConfig = builder.Configuration.GetSection("RateLimitConfig").Get<RateLimitConfig>() ?? new RateLimitConfig();

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
     RateLimitPartition.GetFixedWindowLimiter(
         partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
     factory: partition => new FixedWindowRateLimiterOptions
     {
         PermitLimit = rateLimitConfig.PermitLimit,
         Window = TimeSpan.FromSeconds(rateLimitConfig.WindowInSeconds),
         QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
         QueueLimit = rateLimitConfig.QueueLimit
     }));

    rateLimiterOptions.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            error = "Too many requests. Please try again later.",
            retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
   ? retryAfter.TotalSeconds
            : rateLimitConfig.WindowInSeconds
        }, cancellationToken);
    };
});

builder.Services.AddApplication();

builder.Services.AddMagma3Dependecy(builder.Configuration);

var databasePath = builder.Configuration.GetValue<string>("DatabasePath") ?? "Data/Magma3.db";
builder.Services.AddInfraDependency(databasePath);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("AccessToken", new OpenApiSecurityScheme
    {
        Description = "ApiKey for authorization. Example: '00000000-0000-0000-0000-000000000000'",
        In = ParameterLocation.Header,
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "AccessTokenScheme"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "AccessToken"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
