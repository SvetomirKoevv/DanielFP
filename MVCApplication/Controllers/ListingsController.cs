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
using Microsoft.VisualBasic;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using MVCApplication.Managers;
using SendGrid;
using Microsoft.AspNetCore.Authorization;

namespace MVCApplication.Controllers
{
    public class ListingsController : Controller
    {
        private readonly ListingContext _context;
        private readonly IdentityContext _identityContext;

        public ListingsController(ListingContext context, IdentityContext identityContext)
        {
            _context = context;
            _identityContext = identityContext;
        }

        // GET: AllListings

        public async Task<IActionResult> AllListings(FilterModel filters)
        {
            IEnumerable<Listing> allListings = await _context.ReadAllAsync(true);

            if (filters.isNull)
            {
                foreach (Listing listing in allListings)
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
                    filters .Makes = JsonSerializer.Deserialize<List<string>>(filters.MakesJson);
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
                    foreach (Listing listing in allListings)
                    {
                        filters.Makes.Add(listing.Car.Make);
                    }
                    filters.Makes = filters.Makes.Distinct().ToList();
                }
                else
                {
                    foreach (Listing listing in allListings)
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

            List<Listing> filteredListings = new List<Listing>();

            foreach (Listing l in allListings)
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
                if (l.Price < filters.MinPrice || l.Price > filters.MaxPrice)
                    continue;
                if (l.Car.Mileage < filters.MinMilliage || l.Car.Mileage > filters.MaxMilliage)
                    continue;
                if (l.Car.HorsePower < filters.MinPower || l.Car.HorsePower > filters.MaxPower)
                    continue;

                filteredListings.Add(l);
            }

            int sortIndex = filters.SortTypes.IndexOf(filters.SortType);

            switch (sortIndex)
            {
                case 0: break;
                case 1: filteredListings.Reverse(); break;
                case 2: filteredListings = filteredListings.OrderBy(x => x.Price).ToList(); break;
                case 3: filteredListings = filteredListings.OrderByDescending(x => x.Price).ToList(); break;
            }


            AllListingsFilteredModel viewModel = new AllListingsFilteredModel(filteredListings, filters);

            return View(viewModel);
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            IEnumerable<Listing> mazna = await _context.ReadAllAsync(true);
            

            return View(mazna);
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || await _context.ReadAllAsync() == null)
            {
                return NotFound();
            }

            Listing listingFromDb = await _context.ReadAsync(id);

            ListingDetailsViewModel viewModel = new ListingDetailsViewModel();
            viewModel.Listing = listingFromDb;
            viewModel.allListings = (List<Listing>)await _context.ReadAllAsync(true, true);
            viewModel.allListings.Remove(listingFromDb);

            return View(viewModel);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ListingViewModel listingViewModel)
        {
            Car newCar = new Car(listingViewModel.Make, listingViewModel.Model, listingViewModel.HorsePower, listingViewModel.Mileage,listingViewModel.FuelType, listingViewModel.Transmittion, listingViewModel.Color);
            for (int i = 0; i < listingViewModel.FileUpload.FormFile.Count; i++)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await listingViewModel.FileUpload.FormFile[i].CopyToAsync(memoryStream);

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
            Listing newListing = new Listing(listingViewModel.Name, listingViewModel.Description, listingViewModel.Price);
            newListing.Car = newCar;
            ModelState.Clear(); 
            if (ModelState.IsValid)
            {
                await _context.CreateAsync(newListing);
                return RedirectToAction(nameof(Index));
            }
            return View(new ListingViewModel());
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.ReadAllAsync == null)
            {
                return NotFound();
            }

            var listing = await _context.ReadAsync(id, true, true);
            if (listing == null)
            {
                return NotFound();
            }
            ListingViewModel model = new ListingViewModel(listing, listing.Car);
            return View(model);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ListingViewModel editModel)
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
            Listing newListing = new Listing(editModel.Name, editModel.Description, editModel.Price);
            newListing.Id = id; 
            newListing.Car = newCar;
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                await _context.UpdateAsync(newListing, true, true);
                return RedirectToAction(nameof(Index));
            }
            return View(editModel);
        }

        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Listing listingFromDb = await _context.ReadAsync(id, true, true);
            return View(listingFromDb);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _context.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("FavouriteItem")]
        public async Task<IActionResult> FavouriteItem(ListingDetailsViewModel model)
        {
            var loggedInUser = await _identityContext.FindUserByNameAsync(User.Identity.Name, true);

            if (loggedInUser == null)
            {
                return Unauthorized(); // Handle case where user is not found
            }

            // Get the listing
            var listing = await _context.ReadAsync(model.Listing.Id);
            if (listing == null)
            {
                return NotFound(); // Handle case where listing doesn't exist
            }

            // Add to favorites if not already there
            if (!loggedInUser.Listings.Contains(listing))
            {
                loggedInUser.Listings.Add(listing);
                await _identityContext.UpdateAsync(loggedInUser);
            }

            return RedirectToAction(nameof(Details), new { Id = model.Listing.Id });
        }

        [HttpPost, ActionName("SendEmail")]
        public async Task<IActionResult> SendEmail(ListingDetailsViewModel modelA)
        {
            Response res = await EmailSenderManager.SendEmailAsync(modelA.ContactForm.Email, modelA.ContactForm.Username, modelA.ContactForm.Subject, modelA.ContactForm.Body);
            if (res.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(SuccessfulContact), new { returnId = modelA.Listing.Id });
            }
            else
            {
                return RedirectToAction(nameof(UnSuccessfulContact), new { returnId = modelA.Listing.Id });
            }
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> SuccessfulContact(int returnId)
        {
            return View();
        }

        [Authorize(Roles = "Administrator, User")]
        public async Task<IActionResult> UnSuccessfulContact(int returnId)
        {
            return View();
        }

    }
}
