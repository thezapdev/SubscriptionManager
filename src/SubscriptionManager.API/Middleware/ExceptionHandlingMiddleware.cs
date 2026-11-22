
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManager.Domain.Exceptions;

namespace SubscriptionManager.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(
                ex,
                "An unexpected error occurred while processing your request.");

            await HandleExceptionAsync(context, ex);
        }
    }

    public static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        if (context.Response.HasStarted)
        {
            throw exception;
        }

        var problemDetails = MapExceptionToProblemDetails(exception);

        context.Response.Clear();

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status
            ?? (int)HttpStatusCode.InternalServerError;

        var json = JsonSerializer.Serialize(problemDetails);

        await context.Response.WriteAsync(json);
    }

    private static ProblemDetails MapExceptionToProblemDetails(
        Exception exception)
    {
        return exception switch
        {
            InvalidSubscriptionStateException ex => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Subscription state conflict",
                Detail = ex.Message
            },

            DomainException ex => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Domain validation error",
                Detail = ex.Message
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Detail = "An unexpected error occurred. Please try again later."
            }
        };
    }
}
