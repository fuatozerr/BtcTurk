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

            //x.UsingRabbitMq((context, config) =>
            //{
            //    config.Host($"amqp://guest:guest@http://localhost:8080");

            //    //config.Host(new Uri("localhost:8080"), h =>
            //    //{
            //    //    h.Username("guest");
            //    //    h.Password("guest");
            //    //});

            //    config.ReceiveEndpoint("sms-notification-queue", e =>
            //    {
            //        e.ConfigureConsumer<SmsNotificationConsumer>(context);
            //    });
            //    x.AddConsumer<SmsNotificationConsumer>();

            //    config.AutoStart = true;

            //});

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SmsNotificationConsumer>();

                // add the bus to the container
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("admin");
                        h.Password("123456");
                    });
                    cfg.ReceiveEndpoint("SmsNotificationConsumer", ec =>
                    {
                        // Configure a single consumer
                        ec.ConfigureConsumer<SmsNotificationConsumer>(context);
                    });

                    // or, configure the endpoints by convention
                    cfg.ConfigureEndpoints(context);
                });
            });

            var assm = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assm);
            //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<InstructionValidator>());
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
            return services;

            //di burada eklensin.
        }
    }
}
