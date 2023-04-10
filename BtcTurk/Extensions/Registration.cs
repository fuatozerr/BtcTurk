using BtcTurk.Constants;
using BtcTurk.Context;
using BtcTurk.Filters;
using BtcTurk.Queue;
using BtcTurk.Services;
using BtcTurk.Services.Interfaces;
using BtcTurk.Validators;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BtcTurk.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrasturctureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SmsNotificationConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username(RabbitMQConstants.UserName);
                        h.Password(RabbitMQConstants.Password);

                    });
                    cfg.ReceiveEndpoint(RabbitMQConstants.SmsQueueName, ec =>
                    {
                        ec.ConfigureConsumer<SmsNotificationConsumer>(context);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            var assm = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assm);
            services.AddDbContext<BtcTurkDbContext>(conf =>
            {
                var connStr = $"Server=db;Database=BtcTurk;User=sa;Password=Fuatko123;MultipleActiveResultSets=true;Integrated Security=false;TrustServerCertificate=true"; //dockerda calısınca bu stringi kullanıyorum 

                // connstrLocal=$"Data Source=localhost;Initial Catalog=BtcTurk;Persist Security Info=True;Trust Server Certificate=true;User ID=sa;Password=Fuatko123";   localde çalıstıgım zaman bu stringi kullanıyorum
                conf.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<InstructionValidator>());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped<IInstructionService, InstructionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ILoggingService, LoggingService>();


            return services;

        }
    }
}
