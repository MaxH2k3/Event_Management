using Event_Management.Application.BackgroundTask;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.ExternalServices;
using Event_Management.Application.Service;
using Event_Management.Application.Service.Account;
using Event_Management.Application.Service.Authentication;
using Event_Management.Application.Service.FileService;
using Event_Management.Application.Service.Job;
using Event_Management.Application.Service.Notification;
using Event_Management.Application.Service.Payments;
using Event_Management.Application.Service.Payments.PayPalService;
using Event_Management.Application.ServiceTask;
using Event_Management.Application.Validators;
using Event_Management.Domain.Repository;
using Event_Management.Domain.Repository.Common;
using Event_Management.Domain.Service;
using Event_Management.Domain.Service.TagEvent;
using Event_Management.Domain.UnitOfWork;

using Event_Management.Infrastructure.Configuration;
using Event_Management.Infrastructure.ExternalServices.ApiClients;
using Event_Management.Infrastructure.ExternalServices.Oauth2;
using Event_Management.Infrastructure.Repository;
using Event_Management.Infrastructure.UnitOfWork;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Serilog;
using System.Text.Json;
//using Event_Management.Application.Service.SQL;

namespace Event_Management.Infastructure.Configuration
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this WebApplicationBuilder builder)
        {
			// Set up AutoMapper
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			// Set cache redis
			builder.Services.AddStackExchangeRedisCache(options =>
            {
                var redisConnection = builder.Configuration["Redis:HostName"];
				var redisPassword = builder.Configuration["Redis:Password"];
                options.Configuration = $"{redisConnection},password={redisPassword}";
			});
			builder.Services.AddDistributedMemoryCache();

            // Set up logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
				.CreateLogger();

			builder.Host.UseSerilog();

			// Set up FluentMail
			builder.Services.AddFluentEmail(builder.Configuration);

			//set up configuration JWT
			builder.Services.AddJWT(builder.Configuration);

			// Set up policies
			builder.Services.AddPolicies();

			//Set up vnpay
			builder.Services.AddVnpay(builder.Configuration);

			//Set up Stripe
			builder.Services.AddStripe(builder.Configuration);

            //Set up Paypal
            builder.Services.AddPayPal(builder.Configuration);

            //Setup Currency
            builder.Services.UpdateCurrency(builder.Configuration); 

            // Set up Swagger
            builder.Services.AddSwagger();

            //Setup FluentValidation
            builder.Services.AddValidatorsFromAssemblyContaining<EventRequestDtoValidator>();

			// Set up SignalR
			builder.Services
				.AddSignalR(option =>
				{
					option.EnableDetailedErrors = true;
					option.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
					option.MaximumReceiveMessageSize = 5 * 1024 * 1024; // 5MB
				})
				.AddJsonProtocol(option =>
				{
					option.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				});
			
            
            // Set up schedule job.
            builder.Services.AddSingleton<StdSchedulerFactory>();

            // Set up services to the container.
            builder.Services.AddScoped<IAvatarApiClient, AvatarApiClient>();
            builder.Services.AddScoped<IGoogleTokenValidation, GoogleTokenValidation>();
            builder.Services.AddScoped<IRegisterEventService, RegisterEventService>();
			builder.Services.AddScoped<ITagService, TagService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
            builder.Services.AddScoped<IEventService, Event_Management.Application.Service.EventService>();
            builder.Services.AddScoped<IJWTService, JWTService>();
            builder.Services.AddScoped<IPayPalService, PayPalService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<ISponsorEventService, SponsorEventService>();
			builder.Services.AddScoped<ICurrentUser, CurrentUserService>();
			builder.Services.AddScoped<IEmailService, EmailService>();
			builder.Services.AddScoped<ICacheRepository, CacheRepository>();
			builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            //builder.Services.AddScoped<ISqlService, SqlService>();
            //builder.Services.AddScoped<ICurrentUser, CurrentUserService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IQuartzService, QuartzService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            
            builder.Services.AddScoped<SendMailTask>();

            builder.Services.AddTransient<PaymentHandler>();

            // Set up background task
            builder.Services.AddHostedService<QueuedHostedService>();
            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            //Add MediatR
            builder.Services.AddMediatR(typeof(PaymentDto).Assembly);

            //Set up Quartz
            builder.Services.AddQuartz(q =>
            {
                //q.UseMicrosoftDependencyInjectionJobFactory();
                //name of your job that you created in the Jobs folder.
                var jobKey = new JobKey("AllEventStatusToEndedJob");
                var jobKey2 = new JobKey("AllEventStatusToOngoingJob");
                q.AddJob<AllEventStatusToOngoingJob>(opts => opts.WithIdentity(jobKey2));
                q.AddJob<AllEventStatusToEndedJob>(opts => opts.WithIdentity(jobKey));
                
                q.AddTrigger(opts => opts.ForJob(jobKey2)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(3600).WithRepeatCount(1).Build())
                    .WithDescription("Auto update status for all events")
                );
                q.AddTrigger(opts => opts.ForJob(jobKey)
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(3600).WithRepeatCount(1).Build())
                    .WithDescription("Auto update status for all events")
                );
                
            });
            builder.Services.AddQuartzHostedService(q =>
            {
                q.WaitForJobsToComplete = true;
                q.AwaitApplicationStarted = true;
            });

            builder.Services.AddHttpClient(); // Add this line to register the IHttpClientFactory service

            builder.Services.AddHttpClient("PayPal", c =>
            {
                c.BaseAddress = new Uri("https://api.sandbox.paypal.com"); // Use "https://api.paypal.com" for live environment
            });
        }
	}
}
