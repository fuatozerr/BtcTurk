using BtcTurk.Context;
using BtcTurk.Filters;
using BtcTurk.Services;
using BtcTurk.Services.Interfaces;
using BtcTurk.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BtcTurk.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrasturctureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
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
