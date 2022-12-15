using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageRipper;

namespace ImageRipper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageRipperController : ControllerBase
    {
        private readonly PhotoContext _context;

        public ImageRipperController(PhotoContext context)
        {
            _context = context;
        }

        // GET: api/ImageRipper
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collage>>> GetCollages()
        {
          if (_context.Collages == null)
          {
              return NotFound();
          }
            return await _context.Collages.ToListAsync();
        }

        // GET: api/ImageRipper/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Collage>> GetCollage(string id)
        {
          if (_context.Collages == null)
          {
              return NotFound();
          }
            var collage = await _context.Collages.FindAsync(id);

            if (collage == null)
            {
                return NotFound();
            }

            return collage;
        }

        // PUT: api/ImageRipper/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollage(string id, Collage collage)
        {
            if (id != collage.Id)
            {
                return BadRequest();
            }

            _context.Entry(collage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollageExists(id))
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

        // POST: api/ImageRipper
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Collage>> PostCollage(Collage collage)
        {
          if (_context.Collages == null)
          {
              return Problem("Entity set 'PhotoContext.Collages'  is null.");
          }
            _context.Collages.Add(collage);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CollageExists(collage.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCollage", new { id = collage.Id }, collage);
        }

        // DELETE: api/ImageRipper/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollage(string id)
        {
            if (_context.Collages == null)
            {
                return NotFound();
            }
            var collage = await _context.Collages.FindAsync(id);
            if (collage == null)
            {
                return NotFound();
            }

            _context.Collages.Remove(collage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CollageExists(string id)
        {
            return (_context.Collages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
