using CarReservation.DataAccess;
using CarReservation.Models;

namespace CarReservation.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ICarsDatabase _carsDatabase;

        public ReservationService(ICarsDatabase carsDatabase)
        {
            _carsDatabase = carsDatabase;
        }

        public async Task<Reservation> ReserveCarAsync(Reservation reservation)
        {
            if (!string.IsNullOrEmpty(reservation.CarId))
            {
                if (await _carsDatabase.IsCarAvailableAsync(reservation.CarId, reservation.StartTime, reservation.EndTime))
                {
                    await _carsDatabase.StoreReservationAsync(reservation.CarId, reservation);
                    return reservation; 
                }
                return null;
            }

            var availableCars = await _carsDatabase.GetAllCarsAsync();

            foreach (var car in availableCars)
            {
                if (await _carsDatabase.IsCarAvailableAsync(car.Id, reservation.StartTime, reservation.EndTime))
                {
                    reservation.CarId = car.Id;

                    await _carsDatabase.StoreReservationAsync(car.Id, reservation);
                    return reservation; 
                }
            }

            return null; 
        }
    }
}
