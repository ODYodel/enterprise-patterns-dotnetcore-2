using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Enterprise.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Services
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddEnterpriseServices(this IServiceCollection services)
        {
            services.AddEnterpriseDALServiceCollection();
            services.AddScoped<IWidgetServices, WidgetServices>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddAutoMapper();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ServiceMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
