using BtcTurk.Context;
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
            services.AddDbContext<BtcTurkDbContext>(conf =>
            {
                var connStr = configuration["BtcTurkDbConnectionString"].ToString();
                conf.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });
            return services;

            //di burada eklensin.
        }
    }
}
