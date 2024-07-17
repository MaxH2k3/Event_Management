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
using Event_Management.Domain.Repository.Common;
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

namespace Event_Management.Infastructure.Configuration
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this WebApplicationBuilder builder)
        {
            // Set up logging
            builder.AddLogs();

            // Set up AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Set cache redis
            builder.Services.AddRedisCache(builder.Configuration);

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
            builder.Services.AddRealTime();
			
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
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IQuartzService, QuartzService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            
            builder.Services.AddScoped<ISendMailTask, SendMailTask>();
            builder.Services.AddScoped<IPaymentTransactionService, PaymentTransactionService>();

            builder.Services.AddTransient<PaymentHandler>();

            // Set up background task
            builder.Services.AddHostedService<QueuedHostedService>();
            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            //Add MediatR
            builder.Services.AddMediatR(typeof(PaymentDto).Assembly);

            //Set up Quartz
            builder.Services.AddQuartz();

        }
	}
}
