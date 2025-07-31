using BankAccount.API.Helpers;
using BankAccount.API.Middlewares;
using BankAccount.BusinessLogic;
using BankAccount.BusinessLogic.Services;
using BankAccount.BusinessLogic.Validators;
using BankAccount.DataAccess.Repositories;
using BankAccount.Domain.Interfaces;
using FluentValidation;
using System.Reflection;
using BankAccount.Domain;

namespace BankAccount.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();

        app.Run();
    }
}