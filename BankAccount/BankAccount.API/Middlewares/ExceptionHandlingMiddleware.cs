using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace BankAccount.API.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred while processing your request",
                Status = StatusCodes.Status500InternalServerError,
                Type = exception.GetType().Name,
                Detail = exception.Message
            };

            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case BankAccount.Domain.Exceptions.ValidationException validationException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Validation error";
                    problemDetails.Type = "Validation failure";
                    problemDetails.Detail = "One or more validation errors occurred";
                    problemDetails.Extensions["errors"] = validationException.Errors;
                    break;

                case BankAccount.Domain.Exceptions.AccountNotFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Account not found";
                    break;
            }

            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
