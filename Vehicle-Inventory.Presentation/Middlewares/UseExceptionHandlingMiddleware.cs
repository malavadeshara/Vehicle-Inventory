//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json;
//using System.Net;
//using Vehicle_Inventory.Application.Common;
////using Vehicle_Inventory.Domain.Common;
//using Vehicle_Inventory.Domain.Exceptions;
//using Vehicle_Inventory.Infrastructure.Exceptions;

//namespace Vehicle_Inventory.API.Middleware;

//public class UseExceptionHandlingMiddleware
//{
//    private readonly RequestDelegate _next;

//    public UseExceptionHandlingMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (DomainException ex)
//        {
//            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
//        }
//        catch (ValidationException ex)
//        {
//            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.ErrorCode.ToString());
//        }
//        catch (InfrastructureException ex)
//        {
//            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Message);
//        }
//        catch (UnauthorizedAccessException)
//        {
//            await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, "Unauthorized");
//        }
//        catch (KeyNotFoundException ex)
//        {
//            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
//        }
//        catch (Exception ex)
//        {
//            // For any unhandled exceptions
//            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
//        }
//    }

//    private static async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
//    {
//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = (int)statusCode;

//        var response = new
//        {
//            StatusCode = context.Response.StatusCode,
//            Message = message
//        };

//        var json = JsonConvert.SerializeObject(response);
//        await context.Response.WriteAsync(json);
//    }
//}




using System.Net;
using System.Text.Json;
using Vehicle_Inventory.Application.Exceptions;
using Vehicle_Inventory.Domain.Exceptions;
using Vehicle_Inventory.Infrastructure.Exceptions;

namespace Vehicle_Inventory.API.Middlewares
{
    public class UseExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public UseExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Code.ToString());
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Code.ToString());
            }
            catch (InfrastructureException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Code.ToString());
            }
            catch (UnauthorizedAccessException)
            {
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, "Unauthorized");
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new { error = message });

            return context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseExceptionHandlingMiddleware>();
        }
    }
}