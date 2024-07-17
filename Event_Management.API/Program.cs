using Event_Management.API.Hub;
using Event_Management.API.Middleware;
using Event_Management.Domain.Constants;
using Event_Management.Infastructure.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using SwaggerThemes;


var builder = WebApplication.CreateBuilder(args);

// Set up config
builder.Services.AddScoped<GlobalException>();

// Add services to the container.
builder.AddInfrastructure();

// Set up context
builder.Services.AddHttpContextAccessor();

// Set up cors
builder.Services.AddCors();

//Set size limit for request
builder.Services.Configure<KestrelServerOptions>(options =>
{
	options.Limits.MaxRequestBodySize = DefaultSystem.LimitFile; // 1GB
});

// Set up session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
	app.UseSwagger();
    app.UseSwaggerThemes(Theme.Dracula);
	app.UseSwaggerUI();
}

//inject middleware watch dog logs
/*app.UseWatchDogExceptionLogger();
app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "123";
    opt.UseRegexForBlacklisting = true;
    opt.Blacklist = "api/v1/events, logo/*, api/v1/feedback, api/v1/enums, api/v1/constants, api/v1/response-message, api/v1/participants, api/v1/payment, api/v1/sponsor, api/v1/tags";
});*/

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.UseSession();

app.ConfigureExceptionHandler();

app.MapHub<CheckinHub>(DefaultSystem.CheckinHubConnection);

app.Run();
