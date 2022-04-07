using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;

namespace CoffeeMVC.Controllers
{
    public class MidiaGalleriesController : Controller
    {
        private readonly CoffeeDbContext _context;

        public MidiaGalleriesController(CoffeeDbContext context)
        {
            _context = context;
        }

        // GET: MidiaGalleries
        public async Task<IActionResult> Index()
        {
            return View(await _context.MidiaGalleries.ToListAsync());
        }

        // GET: MidiaGalleries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var midiaGallery = await _context.MidiaGalleries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (midiaGallery == null)
            {
                return NotFound();
            }

            return View(midiaGallery);
        }

        // GET: MidiaGalleries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MidiaGalleries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url, Image")] MidiaGallery midiaGallery)
        {
            var image = UploadImage(midiaGallery.Image);
            if (ModelState.IsValid)
            {
                midiaGallery.Url = await image;
                _context.Add(midiaGallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(midiaGallery);
        }

        // GET: MidiaGalleries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var midiaGallery = await _context.MidiaGalleries.FindAsync(id);
            if (midiaGallery == null)
            {
                return NotFound();
            }
            return View(midiaGallery);
        }

        // POST: MidiaGalleries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url")] MidiaGallery midiaGallery)
        {
            if (id != midiaGallery.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(midiaGallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MidiaGalleryExists(midiaGallery.Id))
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
            return View(midiaGallery);
        }

        // GET: MidiaGalleries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var midiaGallery = await _context.MidiaGalleries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (midiaGallery == null)
            {
                return NotFound();
            }

            return View(midiaGallery);
        }

        // POST: MidiaGalleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var midiaGallery = await _context.MidiaGalleries.FindAsync(id);
            _context.MidiaGalleries.Remove(midiaGallery);
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

        private bool MidiaGalleryExists(int id)
        {
            return _context.MidiaGalleries.Any(e => e.Id == id);
        }
    }
}
