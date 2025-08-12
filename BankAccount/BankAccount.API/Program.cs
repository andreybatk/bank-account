using BankAccount.API.Helpers;
using BankAccount.API.Middlewares;
using BankAccount.BusinessLogic;
using BankAccount.BusinessLogic.Services;
using BankAccount.BusinessLogic.Validators;
using BankAccount.DataAccess;
using BankAccount.DataAccess.Repositories;
using BankAccount.Domain;
using BankAccount.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var authenticationConfiguration = new AuthenticationConfiguration();
        builder.Configuration.Bind("Authentication", authenticationConfiguration);
        builder.Services.AddSingleton(authenticationConfiguration);

        var connectionString = builder.Configuration.GetConnectionString(
            "DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        if (builder.Environment.EnvironmentName != "Testing")
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            builder.Services.AddHangfire(config =>
                config.UsePostgreSqlStorage(options => { options.UseNpgsqlConnection(connectionString); }));

            builder.Services.AddHangfireServer();
        }

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

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        builder.Services.AddValidatorsFromAssembly(typeof(BusinessLogicAssemblyMarker).Assembly);

        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
        builder.Services.AddScoped<AccrueInterestService>();
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

        if (builder.Environment.EnvironmentName != "Testing")
        {
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = authenticationConfiguration.UrlKeycloak;
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
        }

        var app = builder.Build();

        if (builder.Environment.EnvironmentName != "Testing")
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [new AllowAnonymousDashboardAuthorizationFilter()],
                IgnoreAntiforgeryToken = true
            });

            RecurringJob.AddOrUpdate<AccrueInterestService>(
                "daily-interest",
                handler => handler.ProcessAllDepositsAsync(CancellationToken.None),
                Cron.Daily);

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }

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