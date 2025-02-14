using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class BidContext
    {
        private RevHausDbContext dBContext;

        public BidContext(RevHausDbContext context)
        {
            this.dBContext = context;
        }

        public async Task CreateAsync(Bid item)
        {
            try
            {
                dBContext.Bids.Add(item);
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
                Bid bidFromDb = await ReadAsync(key, true, false);
                dBContext.Bids.Remove(bidFromDb);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Bid>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                List<Bid> query = await dBContext.Bids.ToListAsync();
                if (useNavigationalProperties)
                {
                    query = await dBContext.Bids
                            .Include(x => x.User)
                            .ToListAsync();
                }
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Bid> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Bid bid = await dBContext.Bids.FindAsync(key);
                if (useNavigationalProperties)
                {
                    bid = await dBContext.Bids
                            .Include(x => x.User)
                            .FirstOrDefaultAsync(x => x.Id == key);
                }
                return bid;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Bid item, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                Bid bidFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (bidFromDb == null) { await CreateAsync(item); return; }

                bidFromDb.User = item.User;
                bidFromDb.Money = item.Money;

                await dBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
