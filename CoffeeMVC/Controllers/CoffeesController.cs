using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;

namespace CoffeeMVC.Controllers
{
    //[Authorize]
    public class CoffeesController : Controller
    {
        private readonly  CoffeeDbContext _context;
        public static string baseUrl = "http://localhost:41259/api/coffees/";

        public CoffeesController (CoffeeDbContext context)
        {
            _context = context;
        }
        CafeAPI _api = new CafeAPI();
        public async Task<IActionResult> Index()
        {

            var products = await GetAll();
            return View(products);
        }

        [HttpGet]
        public async Task<List<Coffee>> GetAll()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<List<Coffee>>(jsonStr).ToList();

            return res;

        }


        // GET: Coffees/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.Coffee
                .FirstOrDefaultAsync(m => m.CoffeeId == id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // GET: Coffees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coffees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoffeeId,Name,Producer,Description,Region,Url,Image")] Coffee coffee)
        {
            var image = UploadImage(coffee.Image);
            
            if (ModelState.IsValid)
            {
                coffee.Url = await image;
                coffee.Username = User.Identity.Name;
                coffee.CoffeeId = Guid.NewGuid();
                _context.Add(coffee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coffee);
        }

        // GET: Coffees/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.Coffee.FindAsync(id);
            if (coffee == null)
            {
                return NotFound();
            }
            return View(coffee);
        }

        // POST: Coffees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CoffeeId,Name,Producer,Description,Region,Image")] Coffee coffee)
        {
            if (id != coffee.CoffeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coffee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoffeeExists(coffee.CoffeeId))
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
            return View(coffee);
        }

        // GET: Coffees/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.Coffee
                .FirstOrDefaultAsync(m => m.CoffeeId == id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // POST: Coffees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var coffee = await _context.Coffee.FindAsync(id);
            _context.Coffee.Remove(coffee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<string> UploadImage(IFormFile image)
        {

            var reader = image.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=storagevik;AccountKey=rD9uWpP2+78SFAPxhb7ehVFD5tQq4XjeJGKYz1LAuVAxONMF9x+wewbBWoAo2Lr9vN01LNSNWSkW+ASt/h8Kgg==;EndpointSuffix=core.windows.net");
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("amantescafecontainer");
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            await blob.UploadFromStreamAsync(reader);
            var uri = blob.Uri.ToString();
            return uri;
        }

        public async Task<IActionResult> Comments()
        {
            return View();
        }

        private bool CoffeeExists(Guid id)
        {
            return _context.Coffee.Any(e => e.CoffeeId == id);
        }
    }
}
