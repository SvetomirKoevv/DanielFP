using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using MVCApplication.Models;
using System.Security.Claims;
using System.Text.Json;

namespace MVCApplication.Controllers
{
    public class AuctionListingsController : Controller
    {
        private readonly AuctionListingContext _context;
        private readonly BidContext bidsContext;
        private readonly IdentityContext _identityContext;

        public AuctionListingsController
            (AuctionListingContext context, 
            BidContext bidsContext_,
            IdentityContext identityContext)
        {
            _context = context;
            bidsContext = bidsContext_;
            _identityContext = identityContext;
        }

        // GET: AuctionListings
        public async Task<IActionResult> AllAuctions(FilterModel filters)
        {
            IEnumerable<AuctionListing> allListings = await _context.ReadAllAsync(true);

            if (filters.isNull)
            {
                foreach (AuctionListing listing in allListings)
                {
                    filters.Makes.Add(listing.Car.Make);
                    filters.Fuels.Add(listing.Car.FuelType.ToString());
                    filters.Transmitions.Add(listing.Car.Transmittion.ToString());
                    filters.Colors.Add(listing.Car.Color.ToString());
                }
                filters.Makes = filters.Makes.Distinct().ToList();
                filters.Fuels = filters.Fuels.Distinct().ToList();
                filters.Transmitions = filters.Transmitions.Distinct().ToList();
                filters.Colors = filters.Colors.Distinct().ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(filters.ModelsJson))
                {
                    filters.Models = JsonSerializer.Deserialize<List<string>>(filters.ModelsJson);
                }
                if (!string.IsNullOrEmpty(filters.MakesJson))
                {
                    filters.Makes = JsonSerializer.Deserialize<List<string>>(filters.MakesJson);
                }
                if (!string.IsNullOrEmpty(filters.TransmitionsJson))
                {
                    filters.Transmitions = JsonSerializer.Deserialize<List<string>>(filters.TransmitionsJson);
                }
                if (!string.IsNullOrEmpty(filters.ColorsJson))
                {
                    filters.Colors = JsonSerializer.Deserialize<List<string>>(filters.ColorsJson);
                }
                if (!string.IsNullOrEmpty(filters.FuelsJson))
                {
                    filters.Fuels = JsonSerializer.Deserialize<List<string>>(filters.FuelsJson);
                }

                if (filters.Make == null)
                {
                    foreach (AuctionListing listing in allListings)
                    {
                        filters.Makes.Add(listing.Car.Make);
                    }
                    filters.Makes = filters.Makes.Distinct().ToList();
                }
                else
                {
                    foreach (AuctionListing listing in allListings)
                    {
                        filters.Models.Clear();
                        if (listing.Car.Make == filters.Make)
                        {
                            filters.Models.Add(listing.Car.Model);
                        }
                        filters.Models = filters.Models.Distinct().ToList();
                    }
                }
            }

            List<AuctionListing> filteredListings = new List<AuctionListing>();

            foreach (AuctionListing l in allListings)
            {
                if (filters.Model != null && filters.Model != l.Car.Model)
                    continue;
                if (filters.Make != null && filters.Make != l.Car.Make)
                    continue;
                if (filters.Transmition != null && filters.Transmition != l.Car.Transmittion.ToString())
                    continue;
                if (filters.Fuel != null && filters.Fuel != l.Car.FuelType.ToString())
                    continue;
                if (filters.Color != null && filters.Color != l.Car.Color.ToString())
                    continue;
                if (l.Bids.Last().Money < filters.MinPrice || l.Bids.Last().Money > filters.MaxPrice)
                    continue;
                if (l.Car.Mileage < filters.MinMilliage || l.Car.Mileage > filters.MaxMilliage)
                    continue;
                if (l.Car.HorsePower < filters.MinPower || l.Car.HorsePower > filters.MaxPower)
                    continue;

                filteredListings.Add(l);
            }

            int sortIndex = filters.SortTypes.IndexOf(filters.SortType);

            switch(sortIndex)
            {
                case 0: break;
                case 1: filteredListings.Reverse(); break;
                case 2: filteredListings = filteredListings.OrderBy(x => x.StartingPrice).ToList(); break;
                case 3: filteredListings = filteredListings.OrderByDescending(x => x.StartingPrice).ToList(); break;
            }

            AllAuctionListingsModel viewModel = new AllAuctionListingsModel(filteredListings, filters);

            return View(viewModel);
        }

        // GET: AuctionListings
        public async Task<IActionResult> Index()
        {
              return await _context.ReadAllAsync() != null ? 
                          View(await _context.ReadAllAsync(true)) :
                          Problem("Entity set 'RevHausDbContext.AuctionListings'  is null.");
        }

        // GET: AuctionListings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || await _context.ReadAllAsync() == null)
            {
                return NotFound();
            }

            var auctionListing = await _context.ReadAsync(id, true, true);

            Bid highestBid = auctionListing.Bids
                             .OrderByDescending(b => b.Money)
                             .FirstOrDefault();

            if (auctionListing == null)
            {
                return NotFound();
            }

            List<AuctionListingWithVBids<AuctionListing>> auctionListingWithVBids = new List<AuctionListingWithVBids<AuctionListing>>();

            foreach(AuctionListing l in (List<AuctionListing>)await _context.ReadAllAsync(true))
            {
                if (l == auctionListing) continue;

                Bid hBid = l.Bids.
                    OrderByDescending(b => b.Money)
                    .FirstOrDefault();

                auctionListingWithVBids.Add(new AuctionListingWithVBids<AuctionListing>(l, l.Bids, hBid.Money));
            }            

            ListingAuctionDetailsViewModel viewModel = new ListingAuctionDetailsViewModel();
            viewModel.allListings = auctionListingWithVBids;
            viewModel.Listing = auctionListing;
            viewModel.HighestBid = highestBid.Money;

            return View(viewModel);
        }

        // GET: AuctionListings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuctionListings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuctionListingCreateModel auctionModel)
        {
            Car newCar = new Car(auctionModel.Make, auctionModel.Model, auctionModel.HorsePower, auctionModel.Mileage, auctionModel.FuelType, auctionModel.Transmittion, auctionModel.Color);
            for (int i = 0; i < auctionModel.FileUpload.FormFile.Count; i++)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await auctionModel.FileUpload.FormFile[i].CopyToAsync(memoryStream);

                    if (memoryStream.Length < 20971520)
                    {
                        newCar.Images.Add(new Image(memoryStream.ToArray(), newCar));
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }
            DateTime startDateTime = new DateTime(auctionModel.StartDate.Year, 
                                                  auctionModel.StartDate.Month, 
                                                  auctionModel.StartDate.Day, 
                                                  DateTime.Now.Hour, 
                                                  DateTime.Now.Minute, 
                                                  DateTime.Now.Second);

            AuctionListing aListing = new AuctionListing(newCar, auctionModel.StartingPrice, auctionModel.Name, auctionModel.Description, startDateTime, auctionModel.Duration);
            User loggedInUser = await _identityContext.FindUserByNameAsync(User.Identity.Name);
            Bid newBid = new Bid(loggedInUser, aListing, aListing.StartingPrice);
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                await _context.CreateAsync(aListing);
                await bidsContext.CreateAsync(newBid);
                return RedirectToAction(nameof(Index));
            }
            return View(auctionModel);
        }

        // GET: AuctionListings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.ReadAllAsync() == null)
            {
                return NotFound();
            }

            var auctionListing = await _context.ReadAsync(id, true, true);
            if (auctionListing == null)
            {
                return NotFound();
            }

            if (auctionListing.Status == AuctionStatus.Closed)
            {
                return RedirectToAction(nameof(AuctionEnded));
            }

            AuctionListingCreateModel model = new AuctionListingCreateModel(auctionListing, auctionListing.Car);

            return View(model);
        }

        public async Task<IActionResult> AuctionEnded()
        {
            return View();
        }

        // POST: AuctionListings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuctionListingCreateModel editModel)
        {
            Car newCar = new Car(editModel.Make, editModel.Model, editModel.HorsePower, editModel.Mileage, editModel.FuelType, editModel.Transmittion, editModel.Color);
            for (int i = 0; i < editModel.FileUpload.FormFile.Count; i++)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await editModel.FileUpload.FormFile[i].CopyToAsync(memoryStream);

                    if (memoryStream.Length < 20971520)
                    {
                        newCar.Images.Add(new Image(memoryStream.ToArray(), newCar));
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }
            AuctionListing newListing = new AuctionListing(newCar, editModel.StartingPrice, editModel.Name, editModel.Description, editModel.StartDate, editModel.Duration);
            newListing.Id = id;
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                await _context.UpdateAsync(newListing, true, true);
                return RedirectToAction(nameof(Index));
            }
            return View(editModel);
        }

        // GET: AuctionListings/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || await _context.ReadAllAsync() == null)
            {
                return NotFound();
            }

            var auctionListing = await _context.ReadAsync(id,true, true);
            if (auctionListing == null)
            {
                return NotFound();
            }

            List<Bid> allBids = (List<Bid>)await bidsContext.ReadAllAsync(true);

            Bid hBid = auctionListing.Bids.
                    OrderByDescending(b => b.Money)
                    .FirstOrDefault();

             AuctionListingWithVBids<AuctionListing> viewModel = new AuctionListingWithVBids<AuctionListing>(auctionListing, allBids, hBid.Money);

            return View(viewModel);
        }

        // POST: AuctionListings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _context.ReadAllAsync() == null)
            {
                return Problem("Entity set 'RevHausDbContext.AuctionListings'  is null.");
            }
            var auctionListing = await _context.ReadAsync(id, true, true);
            if (auctionListing != null)
            {
                await _context.DeleteAsync(auctionListing.Id);
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> BidOnItem(BidViewModel biModel)
        {
            if (biModel.BidPrice <= 0)
            {
                TempData["BidError"] = "Bid must be greater than 0.";
                return RedirectToAction(nameof(Details), new { id = biModel.ListingId });
            }

            if (biModel.BidPrice == null)
            {
                TempData["BidError"] = "Bid must be greater than 0.";
                return RedirectToAction(nameof(Details), new { id = biModel.ListingId });
            }

            if (biModel.BidPrice > 5000)
            {
                TempData["BidError"] = "Bid must be less than 5000.";
                return RedirectToAction(nameof(Details), new { id = biModel.ListingId });
            }

            User loggedInUser = await _identityContext.FindUserByNameAsync(User.Identity.Name);

            // Get the listing from the database
            var listing = await _context.ReadAsync(biModel.ListingId, true);
            if (DateTime.Now > listing.StartDateTime.AddHours(listing.DurationInHours))
            {
                TempData["BidError"] = "Auction has ended";
                return RedirectToAction(nameof(Details), new { id = biModel.ListingId });
            }

            if (listing == null)
            {
                return NotFound("Listing not found.");
            }

            List<Bid> allBids = (List<Bid>)await bidsContext.ReadAllAsync(true);  

            Bid highestBid = listing.Bids
                            .OrderByDescending(b => b.Money)
                            .FirstOrDefault();

            Bid newBid = new Bid(loggedInUser, listing, highestBid.Money + biModel.BidPrice);

            await bidsContext.CreateAsync(newBid);

            return RedirectToAction(nameof(Details), new { id = biModel.ListingId });
        }

        private bool AuctionListingExists(int id)
        {
            return _context.ReadAsync(id) != null;
        }
    }
}
