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
            var endTime = reservation.StartTime.AddMinutes((reservation.EndTime - reservation.StartTime).TotalMinutes);

            if (!string.IsNullOrEmpty(reservation.CarId))
            {
                if (await _carsDatabase.IsCarAvailableAsync(reservation.CarId, reservation.StartTime, endTime))
                {
                    await _carsDatabase.StoreReservationAsync(reservation.CarId, reservation);
                    return reservation; 
                }
                return null;
            }

            var availableCars = await _carsDatabase.GetAllCarsAsync();

            foreach (var car in availableCars)
            {
                if (await _carsDatabase.IsCarAvailableAsync(car.Id, reservation.StartTime, endTime))
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
