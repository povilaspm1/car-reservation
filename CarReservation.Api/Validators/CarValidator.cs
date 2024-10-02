using CarReservation.Models;
using FluentValidation;

namespace CarReservation.Api.Validators
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(car => car.Id)
                .NotEmpty().WithMessage("Car ID is required.")
                .Length(4, 4).WithMessage("Car ID must be 4 characters.")
                .Matches(@"^C\d+$").WithMessage("Car ID must follow the pattern 'C<number>'.");

            RuleFor(car => car.Make)
                .NotEmpty().WithMessage("Make is required.")
                .Length(1, 100).WithMessage("Make must be between 1 and 100 characters.");

            RuleFor(car => car.Model)
                .NotEmpty().WithMessage("Model is required.")
                .Length(1, 100).WithMessage("Model must be between 1 and 100 characters.");
        }
    }
}
