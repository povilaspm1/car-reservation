using CarReservation.Models;

namespace CarReservation.DataAccess
{
    public class CarsDatabase : ICarsDatabase
    {
        private readonly List<Car> _cars = new();
        private readonly Dictionary<string, List<Reservation>> _carReservations = new();

        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                return _cars.ToList(); 
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<Car> GetCarByIdAsync(string id)
        {
            return _cars.FirstOrDefault(c => c.Id == id);
        }

        public async Task<bool> AddCarAsync(Car car)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_cars.Any(c => c.Id == car.Id))
                {
                    return false;
                }
                _cars.Add(car);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            await _semaphore.WaitAsync();
            try
            {
                var existingCar = await GetCarByIdAsync(car.Id);
                if (existingCar == null)
                {
                    return false;
                }

                existingCar.Make = car.Make;
                existingCar.Model = car.Model;
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            await _semaphore.WaitAsync();
            try
            {
                var car = await GetCarByIdAsync(id);
                if (car == null)
                {
                    return false;
                }

                _cars.Remove(car);
                _carReservations.Remove(car.Id);
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> IsCarAvailableAsync(string carId, DateTime startTime, DateTime endTime)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_carReservations.TryGetValue(carId, out var reservations))
                {
                    foreach (var reservation in reservations)
                    {
                        if (reservation.StartTime < endTime && startTime < reservation.EndTime)
                        {
                            return false; // The car is not available
                        }
                    }
                }
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task StoreReservationAsync(string carId, Reservation reservation)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (!_carReservations.ContainsKey(carId))
                {
                    _carReservations[carId] = new List<Reservation>();
                }
                _carReservations[carId].Add(reservation);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var activeReservations = new List<Reservation>();
                var currentTime = DateTime.UtcNow;

                foreach (var reservations in _carReservations.Values)
                {
                    var ongoingReservations = reservations
                        .Where(reservation => reservation.EndTime > currentTime)
                        .ToList();

                    activeReservations.AddRange(ongoingReservations);
                }

                return activeReservations;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
