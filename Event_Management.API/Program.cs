using Event_Management.API.Hub;
using Event_Management.API.Middleware;
using Event_Management.API.Utilities;
using Event_Management.Domain.Constants.System;
using Event_Management.Infastructure.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using Serilog;
using SwaggerThemes;

var builder = WebApplication.CreateBuilder(args);

// Set up config
builder.Services.AddScoped<GlobalException>();
builder.AddInfrastructure();

// Set up context
builder.Services.AddHttpContextAccessor();

// Set up cors
builder.Services.AddCors();

//Set size limit for request
builder.Services.Configure<KestrelServerOptions>(options =>
{
	options.Limits.MaxRequestBodySize = 1073741824; // 1GB
});

builder.Services.Configure<FormOptions>(options =>
{
	options.MultipartBodyLengthLimit = 1073741824; // 1GB
});

// Default services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseCors(app => app
	.AllowAnyOrigin()
		.AllowAnyMethod()
			.AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
    app.UseSwaggerThemes(SwaggerHelper.GetTheme());
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ConfigureExceptionHandler();

app.MapHub<CheckinHub>(DefaultSystem.CheckinHubConnection);

app.Run();
