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
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("admin");
                        h.Password("123456");
                    });
                    cfg.ReceiveEndpoint("SmsNotificationConsumer", ec =>
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
                var connStr = configuration["BtcTurkDbConnectionString"].ToString();
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
