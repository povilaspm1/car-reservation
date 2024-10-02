using CarReservation.DataAccess;
using CarReservation.Models;
using Moq;

namespace CarReservation.Services.Tests
{
    public class ReservationServiceTests
    {
        private readonly Mock<ICarsDatabase> _mockCarsDatabase;
        private readonly ReservationService _reservationService;

        public ReservationServiceTests()
        {
            _mockCarsDatabase = new Mock<ICarsDatabase>();
            _reservationService = new ReservationService(_mockCarsDatabase.Object);
        }

        [Fact]
        public async Task ReserveCarAsync_WithValidCarId_ReservesCar()
        {
            var reservation = new Reservation
            {
                CarId = "C1",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(30)
            };

            _mockCarsDatabase.Setup(db => db.IsCarAvailableAsync(reservation.CarId, reservation.StartTime, reservation.EndTime))
                .ReturnsAsync(true);

            _mockCarsDatabase.Setup(db => db.StoreReservationAsync(reservation.CarId, reservation))
                .Returns(Task.CompletedTask);

            var result = await _reservationService.ReserveCarAsync(reservation);

            Assert.NotNull(result);
            Assert.Equal(reservation.CarId, result.CarId);
            _mockCarsDatabase.Verify(db => db.StoreReservationAsync(reservation.CarId, reservation), Times.Once);
        }

        [Fact]
        public async Task ReserveCarAsync_WithInvalidCarId_ReturnsNull()
        {
            var reservation = new Reservation
            {
                CarId = "C1",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(30)
            };

            _mockCarsDatabase.Setup(db => db.IsCarAvailableAsync(reservation.CarId, reservation.StartTime, reservation.EndTime))
                .ReturnsAsync(false);

            var result = await _reservationService.ReserveCarAsync(reservation);

            Assert.Null(result);
            _mockCarsDatabase.Verify(db => db.StoreReservationAsync(It.IsAny<string>(), It.IsAny<Reservation>()), Times.Never);
        }

        [Fact]
        public async Task ReserveCarAsync_WithoutCarId_ReservesFirstAvailableCar()
        {
            var reservation = new Reservation
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(30)
            };

            var availableCars = new List<Car>
        {
            new Car { Id = "C1" },
            new Car { Id = "C2" }
        };

            _mockCarsDatabase.Setup(db => db.GetAllCarsAsync()).ReturnsAsync(availableCars);
            _mockCarsDatabase.Setup(db => db.IsCarAvailableAsync("C1", reservation.StartTime, reservation.EndTime))
                .ReturnsAsync(true);

            _mockCarsDatabase.Setup(db => db.StoreReservationAsync("C1", reservation))
                .Returns(Task.CompletedTask);

            var result = await _reservationService.ReserveCarAsync(reservation);

            Assert.NotNull(result);
            Assert.Equal("C1", result.CarId);
            _mockCarsDatabase.Verify(db => db.StoreReservationAsync("C1", reservation), Times.Once);
        }

        [Fact]
        public async Task ReserveCarAsync_NoAvailableCars_ReturnsNull()
        {
            var reservation = new Reservation
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(30)
            };

            var availableCars = new List<Car>
        {
            new Car { Id = "C1" },
            new Car { Id = "C2" }
        };

            _mockCarsDatabase.Setup(db => db.GetAllCarsAsync()).ReturnsAsync(availableCars);
            _mockCarsDatabase.Setup(db => db.IsCarAvailableAsync(It.IsAny<string>(), reservation.StartTime, reservation.EndTime))
                .ReturnsAsync(false);

            var result = await _reservationService.ReserveCarAsync(reservation);

            Assert.Null(result);
        }
    }
}