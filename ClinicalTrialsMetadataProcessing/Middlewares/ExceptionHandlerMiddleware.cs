using ClinicalTrialsMetadataProcessing.Properties;
using ClinicalTrialsMetadataProcessing.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicalTrialsMetadataProcessing.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate nextRequest, ILogger<ExceptionHandlerMiddleware> logger)
{
	private readonly RequestDelegate m_nextRequest = nextRequest;
	private readonly ILogger<ExceptionHandlerMiddleware> m_logger = logger;

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await m_nextRequest(context);
		}
		catch (ValidationException ex)
		{
			await HandleExceptionAsync(context, ex.Message, HttpStatusCode.BadRequest);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, string.Format(AppErrors.InternalError, ex.Message), HttpStatusCode.InternalServerError);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, string detail, HttpStatusCode status)
	{
		m_logger.LogError(detail);

		await context.Response.WriteAsJsonAsync(new ProblemDetails
		{
			Detail = detail,
			Status = (int)status
		});
	}
}
