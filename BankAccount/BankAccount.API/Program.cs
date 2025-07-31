using BankAccount.API.Helpers;
using BankAccount.API.Middlewares;
using BankAccount.BusinessLogic;
using BankAccount.BusinessLogic.Services;
using BankAccount.BusinessLogic.Validators;
using BankAccount.DataAccess.Repositories;
using BankAccount.Domain;
using BankAccount.Domain.Interfaces;
using FluentValidation;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BankAccount.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var authenticationConfiguration = new AuthenticationConfiguration();
        builder.Configuration.Bind("Authentication", authenticationConfiguration);
        builder.Services.AddSingleton(authenticationConfiguration);
        builder.Services.AddHttpClient();

        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

        builder.Services.AddSwaggerGen(c =>
        {
            var xmlApi = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            if (File.Exists(xmlApi))
                c.IncludeXmlComments(xmlApi, includeControllerXmlComments: true);

            var businessLogicAssembly = typeof(BusinessLogicAssemblyMarker).Assembly;
            var xmlBusinessLogic = Path.Combine(AppContext.BaseDirectory, $"{businessLogicAssembly.GetName().Name}.xml");
            if (File.Exists(xmlBusinessLogic))
                c.IncludeXmlComments(xmlBusinessLogic);

            var domainAssembly = typeof(DomainAssemblyMarker).Assembly;
            var xmlDomain = Path.Combine(AppContext.BaseDirectory, $"{domainAssembly.GetName().Name}.xml");
            if (File.Exists(xmlDomain))
                c.IncludeXmlComments(xmlDomain);

            c.SchemaFilter<EnumSchemaFilter>();

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT token. Example: {your token}"
            });
            c.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<BusinessLogicAssemblyMarker>();
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssembly(typeof(BusinessLogicAssemblyMarker).Assembly);

        builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        builder.Services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();
        builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
        builder.Services.AddSingleton<IClientVerificationService, ClientVerificationService>();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = authenticationConfiguration.URLKeycloak;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = authenticationConfiguration.ValidIssuer,
                ValidAudience = authenticationConfiguration.ValidAudience,
                ValidateIssuer = true,
                ValidateAudience = true
            };
        });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();

        app.Run();
    }
}