using HazGo.Application.Common.Interfaces;
using HazGo.Domain.Repositories;
using HazGo.Infrastructure.Repositories;
using HazGo.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using HazGo.BuildingBlocks.Core.Domain;

namespace HazGo.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new System.Version(8, 0, 22))));
            services.AddMediatR(typeof(DomainEventDispatcher).GetTypeInfo().Assembly);

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IRoleModuleRepository, RoleModuleRepository>();

            return services;
        }
    }
}
