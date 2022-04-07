using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CoffeesController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public CoffeesController(CoffeeDbContext context)
        {
            _context = context;
        }

        // GET: api/Coffees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coffee>>> GetCoffee()
        {
            return await _context.Coffee.ToListAsync();
        }

        // GET: api/Coffees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coffee>> GetCoffee(Guid id)
        {
            var coffee = await _context.Coffee.FindAsync(id);

            if (coffee == null)
            {
                return NotFound();
            }

            return coffee;
        }

        // PUT: api/Coffees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoffee(Guid id, Coffee coffee)
        {
            if (id != coffee.CoffeeId)
            {
                return BadRequest();
            }

            _context.Entry(coffee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoffeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Coffees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Coffee>> PostCoffee(Coffee coffee)
        {
            _context.Coffee.Add(coffee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCoffee", new { id = coffee.CoffeeId }, coffee);
        }

        // DELETE: api/Coffees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Coffee>> DeleteCoffee(Guid id)
        {
            var coffee = await _context.Coffee.FindAsync(id);
            if (coffee == null)
            {
                return NotFound();
            }

            _context.Coffee.Remove(coffee);
            await _context.SaveChangesAsync();

            return coffee;
        }

        private bool CoffeeExists(Guid id)
        {
            return _context.Coffee.Any(e => e.CoffeeId == id);
        }
    }
}
