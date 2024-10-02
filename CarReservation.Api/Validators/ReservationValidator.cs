using CarReservation.Models;
using FluentValidation;

namespace CarReservation.Api.Validators
{
    public class ReservationValidator : AbstractValidator<Reservation>
    {
        public ReservationValidator()
        {
            RuleFor(reservation => reservation.CarId)
               .NotEmpty().WithMessage("Car ID is required.")
               .Length(4).WithMessage("Car ID must be 4 characters.")
               .Matches(@"^C\d+$").WithMessage("Car ID must follow the pattern 'C<number>'.")
               .When(reservation => !string.IsNullOrEmpty(reservation.CarId));

            RuleFor(reservation => reservation.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future.")
                .LessThan(DateTime.UtcNow.AddDays(1)).WithMessage("Start time must be within the next 24 hours.");

            RuleFor(reservation => reservation.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(reservation => reservation.StartTime).WithMessage("End time must be after the start time.")
                .LessThan(reservation => reservation.StartTime.AddHours(2)).WithMessage("Reservation duration cannot exceed 2 hours.");
        }
    }
}
