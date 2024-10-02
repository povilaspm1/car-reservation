using CarReservation.Models;

namespace CarReservation.Services
{
    public interface IReservationService
    {
        Task<Reservation> ReserveCarAsync(Reservation reservation);
    }
}