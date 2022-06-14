using Application.Common.Behaviors;
using Domain.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IRepository, EfRepository>();
            services.AddTransient<IFlightCache, FlightCache>();
            services.AddMemoryCache();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>),typeof(LoggingBehavior<,>));
            return services;
        }
    }
}
