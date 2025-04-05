using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WhatsappApisSender.Configurations;
using WhatsappApisSender.Extensions;
using WhatsappApisSender.Handlers;
using WhatsappApisSender.Middleware;
using WhatsappApisSender.Models;
using WhatsappApisSender.Services;
using WhatsappApisSender.Storage;

var builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;
var environment = builder.Environment;

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

services.AddAuthorization();

// this connect to postgrsSql database
builder.AddIdentityCoreWithPostgreSQL();

services.AddSwagger();
services.AddOptions();
services.AddHttpClient();

// inject services
services.AddSingleton<ITokenService, TokenService>();
services.AddScoped<IStorageManager, StorageManager>();
services.Configure<WhatsAppSettings>(builder.Configuration.GetSection("WhatsAppSettings"));
services.AddScoped<IAuthenticationHandler, AuthenticationHandler>();
services.AddScoped<IWatsappSenderHandlers, WatsappSenderHandlers>();
services.AddScoped<IUserHandlers, UserHandlers>();
services.AddHostedService<MessageSenderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.InitRolesAsync().Wait();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.Run();
