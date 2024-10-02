using CarReservation.Api.Validators;
using CarReservation.Models;
using FluentValidation.TestHelper;

namespace CarReservation.Api.Tests
{
    public class ReservationValidatorTests
    {
        private readonly ReservationValidator _validator;

        public ReservationValidatorTests()
        {
            _validator = new ReservationValidator();
        }

        [Fact]
        public void Should_Have_Error_When_CarId_Is_Not_4_Characters()
        {
            var reservation = new Reservation { CarId = "C12", StartTime = DateTime.UtcNow.AddHours(1), EndTime = DateTime.UtcNow.AddHours(2) };
            var result = _validator.TestValidate(reservation);
            result.ShouldHaveValidationErrorFor(r => r.CarId);
        }

        [Fact]
        public void Should_Have_Error_When_CarId_Does_Not_Match_Pattern()
        {
            var reservation = new Reservation { CarId = "ABC", StartTime = DateTime.UtcNow.AddHours(1), EndTime = DateTime.UtcNow.AddHours(2) };
            var result = _validator.TestValidate(reservation);
            result.ShouldHaveValidationErrorFor(r => r.CarId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_CarId_Is_Valid()
        {
            var reservation = new Reservation { CarId = "C123", StartTime = DateTime.UtcNow.AddHours(1), EndTime = DateTime.UtcNow.AddHours(2) };
            var result = _validator.TestValidate(reservation);
            result.ShouldNotHaveValidationErrorFor(r => r.CarId);
        }

        [Fact]
        public void Should_Have_Error_When_StartTime_Is_In_Past()
        {
            var reservation = new Reservation { CarId = "C123", StartTime = DateTime.UtcNow.AddHours(-1), EndTime = DateTime.UtcNow.AddHours(1) };
            var result = _validator.TestValidate(reservation);
            result.ShouldHaveValidationErrorFor(r => r.StartTime);
        }

        [Fact]
        public void Should_Have_Error_When_StartTime_Is_Not_In_24_Hours()
        {
            var reservation = new Reservation { CarId = "C123", StartTime = DateTime.UtcNow.AddHours(25), EndTime = DateTime.UtcNow.AddHours(26) };
            var result = _validator.TestValidate(reservation);
            result.ShouldHaveValidationErrorFor(r => r.StartTime);
        }

        [Fact]
        public void Should_Have_Error_When_EndTime_Is_Before_StartTime()
        {
            var reservation = new Reservation { CarId = "C123", StartTime = DateTime.UtcNow.AddHours(2), EndTime = DateTime.UtcNow.AddHours(1) };
            var result = _validator.TestValidate(reservation);
            result.ShouldHaveValidationErrorFor(r => r.EndTime);
        }

        [Fact]
        public void Should_Have_Error_When_EndTime_Exceeds_2_Hours_From_StartTime()
        {
            var reservation = new Reservation { CarId = "C123", StartTime = DateTime.UtcNow.AddHours(1), EndTime = DateTime.UtcNow.AddHours(3) };
            var result = _validator.TestValidate(reservation);
            result.ShouldHaveValidationErrorFor(r => r.EndTime);
        }

        [Fact]
        public void Should_Not_Have_Error_When_EndTime_Is_Valid()
        {
            var reservation = new Reservation { CarId = "C123", StartTime = DateTime.UtcNow.AddHours(1), EndTime = DateTime.UtcNow.AddHours(2) };
            var result = _validator.TestValidate(reservation);
            result.ShouldNotHaveValidationErrorFor(r => r.EndTime);
        }
    }
}