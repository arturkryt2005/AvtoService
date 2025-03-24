using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AvtoService.Data;
using AvtoService.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvtoService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly AvtoServiceContext _context;

        public ContactFormController(AvtoServiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactForm>>> GetContactForm()
        {
            return await _context.ContactForm.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactForm>> GetContactForm(int id)
        {
            var contactForm = await _context.ContactForm.FindAsync(id);

            if (contactForm == null)
            {
                return NotFound();
            }

            return contactForm;
        }

        [HttpPost]
        public async Task<ActionResult<ContactForm>> PostContactForm(ContactForm contactForm)
        {
            if (string.IsNullOrEmpty(contactForm.Email))
            {
                return BadRequest("Email is required.");
            }

            contactForm.SubmissionDate = DateTime.UtcNow;
            _context.ContactForm.Add(contactForm);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetContactForm", new { id = contactForm.Id }, contactForm);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactForm(int id, ContactForm contactForm)
        {
            if (id != contactForm.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactFormExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactForm(int id)
        {
            var contactForm = await _context.ContactForm.FindAsync(id);
            if (contactForm == null)
            {
                return NotFound();
            }

            _context.ContactForm.Remove(contactForm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactFormExists(int id)
        {
            return _context.ContactForm.Any(e => e.Id == id);
        }
    }
}