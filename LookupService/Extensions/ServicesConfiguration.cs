using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System;

namespace LookupService.Extension
{
    public static class ServicesConfiguration
    {
        public static void ConfigureHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient("credit-data", client =>
            {
                client.BaseAddress = new Uri(Constants.CreditDataBaseURI);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options =>
            {

            });
    }
}
