using BankAccount.Domain;

namespace BankAccount.API.Middlewares;

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
        context.Response.ContentType = "application/json";

        MbResult<object> response;

        switch (exception)
        {
            case Domain.Exceptions.ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new MbResult<object>
                {
                    Value = null,
                    MbError = validationException.Errors
                };
                break;

            case Domain.Exceptions.EntityNotFoundException notFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new MbResult<object>
                {
                    Value = null,
                    MbError = notFoundException.Errors
                };
                break;

            case Domain.Exceptions.BadRequestException badRequestException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new MbResult<object>
                {
                    Value = null,
                    MbError = badRequestException.Errors
                };
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new MbResult<object>
                {
                    Value = null,
                    MbError = ["Произошла непредвиденная ошибка"]
                };
                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}