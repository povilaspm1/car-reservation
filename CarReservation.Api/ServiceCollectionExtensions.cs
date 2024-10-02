using CarReservation.Api.Validators;
using CarReservation.DataAccess;
using CarReservation.Services;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace CarReservation.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCarServices(this IServiceCollection services)
        {
            services.AddSingleton<ICarsDatabase, CarsDatabase>();
            services.AddSingleton<IReservationService, ReservationService>();
        }

        public static void AddFluentValidationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                    .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CarValidator>();
            services.AddValidatorsFromAssemblyContaining<ReservationValidator>();
        }
    }
}
