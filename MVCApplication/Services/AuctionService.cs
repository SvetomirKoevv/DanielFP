using BusinessLayer;
using DataLayer;

namespace MVCApplication.Services
{
    public class AuctionService
    {
        private readonly RevHausDbContext _context;

        public AuctionService(RevHausDbContext context)
        {
            _context = context;
        }

        public void OpenNewAuctions()
        {
            var expiredAuctions = _context.AuctionListings
                .Where(a => a.StartDateTime <= DateTime.Now && a.Status == AuctionStatus.Unstarted)
                .ToList();

            foreach (var auction in expiredAuctions)
            {
                auction.Status = AuctionStatus.Closed;
            }

            _context.SaveChanges();
        }

        public void CloseExpiredAuctions()
        {
            var expiredAuctions = _context.AuctionListings
                .Where(a => (a.StartDateTime.AddHours(a.DurationInHours)) <= DateTime.Now && a.Status == AuctionStatus.Ongoing)
                .ToList();

            foreach (var auction in expiredAuctions)
            {
                auction.Status = AuctionStatus.Closed;
                auction.Winner = _context.Users.Where(x => x.Id == _context.Bids
                    .Where(b => b.AuctionListing.Id == auction.Id)
                    .OrderByDescending(b => b.Money)
                    .Select(b => b.UserId)
                    .FirstOrDefault())
                    .FirstOrDefault();
            }

            _context.SaveChanges();
        }
    }

}
