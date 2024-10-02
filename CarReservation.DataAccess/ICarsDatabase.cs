using CarReservation.Models;

namespace CarReservation.DataAccess
{
    public interface ICarsDatabase
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(string id);
        Task<bool> AddCarAsync(Car car);
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(string id);
        Task<bool> IsCarAvailableAsync(string carId, DateTime startTime, DateTime endTime);
        Task StoreReservationAsync(string carId, Reservation reservation);
        Task<IEnumerable<Reservation>> GetActiveReservationsAsync();
    }
}