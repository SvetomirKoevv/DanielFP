using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class ListingContext
    {
        private RevHausDbContext dBContext;

        public ListingContext(RevHausDbContext context)
        {
            this.dBContext = context;
        }

        public async Task CreateAsync(Listing item)
        {
            try
            {
                dBContext.Cars.Add(item.Car);
                dBContext.Image.AddRange(item.Car.Images);
                dBContext.Listings.Add(item);
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
                Listing listingFromDb = await ReadAsync(key, true, false);
                dBContext.Listings.Remove(listingFromDb);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Listing>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                List<Listing> query = await dBContext.Listings.ToListAsync();
                if (useNavigationalProperties)
                {
                    query = await dBContext.Listings
                            .Include(x => x.Users)
                            .ToListAsync();
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Listing> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Listing listing = await dBContext.Listings.FindAsync(key);
                if (useNavigationalProperties)
                {
                    listing = await dBContext.Listings
                        .Include(x => x.Car)
                        .Include(x => x.Car.Images)
                        .Include(x => x.Users)
                        .FirstOrDefaultAsync(x => x.Id == key);
                }
                return listing;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Listing item, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Listing listingFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (listingFromDb == null) { await CreateAsync(item); return; }

                listingFromDb.Car = item.Car;
                listingFromDb.Name = item.Name;
                listingFromDb.Description = item.Description;

                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
