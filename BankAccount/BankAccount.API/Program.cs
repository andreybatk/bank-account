using BankAccount.API.Middlewares;
using BankAccount.BusinessLogic;
using BankAccount.BusinessLogic.Validators;
using FluentValidation;

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


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ValidationExceptionHandlingMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
