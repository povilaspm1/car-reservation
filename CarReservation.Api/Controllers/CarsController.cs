using CarReservation.DataAccess;
using CarReservation.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarReservation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarsDatabase _carsDatabase;

        public CarsController(ICarsDatabase carsDatabase)
        {
            _carsDatabase = carsDatabase;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetAllCars()
        {
            var cars = await _carsDatabase.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> AddCar([FromBody] Car newCar)
        {
            if (await _carsDatabase.GetCarByIdAsync(newCar.Id) != null)
            {
                return BadRequest("A car with this ID already exists.");
            }

            if (await _carsDatabase.AddCarAsync(newCar))
            {
                return CreatedAtAction(nameof(GetAllCars), new { id = newCar.Id }, newCar);
            }
            return StatusCode(500, "An error occurred while adding the car.");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCar([FromBody] Car updatedCar)
        {
            var existingCar = await _carsDatabase.GetCarByIdAsync(updatedCar.Id);
            if (existingCar == null)
            {
                return NotFound("Car not found.");
            }

            if (await _carsDatabase.UpdateCarAsync(updatedCar))
            {
                return NoContent();
            }
            return StatusCode(500, "An error occurred while updating the car.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(string id)
        {
            var car = await _carsDatabase.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound("Car not found.");
            }

            if (await _carsDatabase.DeleteCarAsync(id))
            {
                return NoContent();
            }
            return StatusCode(500, "An error occurred while deleting the car.");
        }
    }
}
