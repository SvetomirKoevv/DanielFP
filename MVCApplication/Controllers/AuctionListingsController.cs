using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;

namespace MVCApplication.Controllers
{
    public class AuctionListingsController : Controller
    {
        private readonly RevHausDbContext _context;

        public AuctionListingsController(RevHausDbContext context)
        {
            _context = context;
        }

        // GET: AuctionListings
        public async Task<IActionResult> Index()
        {
              return _context.AuctionListings != null ? 
                          View(await _context.AuctionListings.ToListAsync()) :
                          Problem("Entity set 'RevHausDbContext.AuctionListings'  is null.");
        }

        // GET: AuctionListings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AuctionListings == null)
            {
                return NotFound();
            }

            var auctionListing = await _context.AuctionListings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auctionListing == null)
            {
                return NotFound();
            }

            return View(auctionListing);
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
        public async Task<IActionResult> Create([Bind("Id,StartingPrice,Name,Description")] AuctionListing auctionListing)
        {
            if (ModelState.IsValid)
            {
                _context.Add(auctionListing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auctionListing);
        }

        // GET: AuctionListings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AuctionListings == null)
            {
                return NotFound();
            }

            var auctionListing = await _context.AuctionListings.FindAsync(id);
            if (auctionListing == null)
            {
                return NotFound();
            }
            return View(auctionListing);
        }

        // POST: AuctionListings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartingPrice,Name,Description")] AuctionListing auctionListing)
        {
            if (id != auctionListing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auctionListing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionListingExists(auctionListing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(auctionListing);
        }

        // GET: AuctionListings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AuctionListings == null)
            {
                return NotFound();
            }

            var auctionListing = await _context.AuctionListings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auctionListing == null)
            {
                return NotFound();
            }

            return View(auctionListing);
        }

        // POST: AuctionListings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AuctionListings == null)
            {
                return Problem("Entity set 'RevHausDbContext.AuctionListings'  is null.");
            }
            var auctionListing = await _context.AuctionListings.FindAsync(id);
            if (auctionListing != null)
            {
                _context.AuctionListings.Remove(auctionListing);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuctionListingExists(int id)
        {
          return (_context.AuctionListings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
