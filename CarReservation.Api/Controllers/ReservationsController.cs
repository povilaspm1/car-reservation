using CarReservation.DataAccess;
using CarReservation.Models;
using CarReservation.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarReservation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ICarsDatabase _carsDatabase;
        private readonly IReservationService _reservationService;

        public ReservationsController(ICarsDatabase carsDatabase, IReservationService reservationService)
        {
            _carsDatabase = carsDatabase;
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {
            var createdReservation = await _reservationService.ReserveCarAsync(reservation);

            if (createdReservation != null)
            {
                return CreatedAtAction(nameof(CreateReservation), new { id = createdReservation.Id }, createdReservation);
            }

            return BadRequest("No cars available for the requested time or the specified car is not available.");
        }

        [HttpGet("active-reservations")]
        public async Task<IActionResult> GetActiveReservations()
        {
            var activeReservations = await _carsDatabase.GetActiveReservationsAsync();

            if (activeReservations == null || !activeReservations.Any())
            {
                return NoContent();
            }

            return Ok(activeReservations);
        }
    }
}
