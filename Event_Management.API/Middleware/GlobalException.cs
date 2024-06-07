using Event_Management.Domain.Models.System;
using System.Net;

namespace Event_Management.API.Middleware
{
	public class GlobalException : IMiddleware
	{
		private readonly ILogger<GlobalException> _logger;

		public GlobalException(ILogger<GlobalException> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{

				await next(context);
			} catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				await HandleException(context, ex);
			}

		}

		private static Task HandleException(HttpContext context, Exception ex)
		{
			int statusCode = (int)HttpStatusCode.InternalServerError;
			var methodError = ex.TargetSite?.DeclaringType?.FullName;
			var errorResponse = new ErrorResponse()
			{
				StatusCode = statusCode,
				Message = ex.GetType().ToString(),
				Location = (methodError != null ? ("Class: " + methodError + ", ") : "") + "Method: " + ex.TargetSite?.Name,
				Detail = ex.Message,
			};
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;

			return context.Response.WriteAsync(errorResponse.ToString());
		}
	}

	public static class ExceptionExtention
	{
		public static void ConfigureExceptionHandler(this IApplicationBuilder app)
		{
			app.UseMiddleware<GlobalException>();
		}
	}
}
