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

// Configure cookie policy
services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always;
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://192.168.1.10:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options => 
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = "authjs.session-token";
        options.Cookie.Domain = "localhost";
        options.Cookie.Path = "/";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    })
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

// Force HTTPS redirection
app.UseHttpsRedirection();

// Use cookie policy
app.UseCookiePolicy();

// Add CORS middleware
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Initialize roles
app.InitRolesAsync().Wait();

app.Run();
