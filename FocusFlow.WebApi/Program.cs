using Microsoft.EntityFrameworkCore;
using FocusFlow.Core;
using FocusFlow.WebApi.Common;
using FocusFlow.WebApi.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FocusFlow.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.AddHealthChecks();

            app.Services.ContextMigrate();

            await app.Services.SeedDataAsync();

            app.Run();
        }

        public static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder)
        {
            //Specify the application debug levels if needed
            builder.Logging
                .ClearProviders()
                .AddConsole()
                .AddDebug();

            builder.Services.AddControllers();
            builder.Services
                .AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("Database", tags: [HealthCheckType.Readiness.ToString()]);

            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(options => SwaggerHelper.SwaggerBearerAuthentication(options))
                .AddDependencies(options => options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection")));

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });
            builder.Services.AddAuthorization();
            return builder;
        }
    }
}