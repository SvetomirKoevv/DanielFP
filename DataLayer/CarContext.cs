using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class CarContext
    {
        private RevHausDbContext dBContext;

        public CarContext(RevHausDbContext context)
        {
            this.dBContext = context;
        }

        public async Task CreateAsync(Car item)
        {
            try
            {
                dBContext.Cars.Add(item);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Car carFromDb = await ReadAsync(key, true, false);
                dBContext.Cars.Remove(carFromDb);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Car>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                List<Car> query = await dBContext.Cars.ToListAsync();
               
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Car> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Car car = await dBContext.Cars.FindAsync(key);
              
                return car;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Car item, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Car carFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (carFromDb == null) { await CreateAsync(item); return; }

                carFromDb.Make = item.Make;
                carFromDb.Model = item.Model;
                carFromDb.Transmittion = item.Transmittion;
                carFromDb.Color = item.Color;
                carFromDb.FuelType = item.FuelType;
                carFromDb.HorsePower = item.HorsePower;
                carFromDb.Mileage = item.Mileage;

                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
