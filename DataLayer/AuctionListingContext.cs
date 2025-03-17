using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class AuctionListingContext : IDb<AuctionListing, int>
    {
        private RevHausDbContext dBContext;

        public AuctionListingContext(RevHausDbContext context)
        {
            this.dBContext = context;
        }

        public async Task CreateAsync(AuctionListing item)
        {
            try
            {
                dBContext.Cars.Add(item.Car);
                dBContext.AuctionListings.Add(item);
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
                AuctionListing auctionListingFromDb = await ReadAsync(key, true, false);
                dBContext.AuctionListings.Remove(auctionListingFromDb);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<AuctionListing>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                List<AuctionListing> query = await dBContext.AuctionListings.ToListAsync();
                if (useNavigationalProperties)
                {
                    query = await dBContext.AuctionListings
                            .Include(x => x.Bids)
                            .Include(x => x.Car)
                            .Include(x => x.Car.Images)
                            .ToListAsync();
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AuctionListing> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                AuctionListing auction = await dBContext.AuctionListings.FindAsync(key);
                if (useNavigationalProperties)
                {
                    auction = await dBContext.AuctionListings
                            .Include(x => x.Bids)
                            .Include(x => x.Car)
                            .Include(x => x.Car.Images)
                            .FirstOrDefaultAsync(x => x.Id == key);
                }
                return auction;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(AuctionListing item, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                AuctionListing auctionFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (auctionFromDb == null) { await CreateAsync(item); return; }

                auctionFromDb.Car = item.Car;
                auctionFromDb.StartingPrice = item.StartingPrice;
                auctionFromDb.Name = item.Name;
                auctionFromDb.Description = item.Description;
                auctionFromDb.StartDateTime = item.StartDateTime;
                auctionFromDb.DurationInHours = item.DurationInHours;
               
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
