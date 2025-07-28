using BankAccount.API.Middlewares;
using BankAccount.BusinessLogic;
using BankAccount.BusinessLogic.Validators;
using BankAccount.Domain.Interfaces;
using BankAccount.DataAccess.Repositories;
using FluentValidation;
using BankAccount.BusinessLogic.Accounts;

namespace BankAccount.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<BusinessLogicAssemblyMarker>();
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            builder.Services.AddValidatorsFromAssembly(typeof(BusinessLogicAssemblyMarker).Assembly);

            builder.Services.AddSingleton<IAccountService, AccountService>();
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
}
