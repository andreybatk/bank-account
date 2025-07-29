using BankAccount.API.Helpers;
using BankAccount.API.Middlewares;
using BankAccount.BusinessLogic;
using BankAccount.BusinessLogic.Validators;
using BankAccount.Domain.Interfaces;
using BankAccount.DataAccess.Repositories;
using FluentValidation;
using BankAccount.BusinessLogic.Services;

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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();

        app.Run();
    }
}