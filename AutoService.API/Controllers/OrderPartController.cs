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
    public class OrderPartsController : ControllerBase
    {
        private readonly AvtoServiceContext _context;

        public OrderPartsController(AvtoServiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderPart>>> GetOrderParts()
        {
            return await _context.OrderParts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderPart>> GetOrderPart(int id)
        {
            var orderPart = await _context.OrderParts.FindAsync(id);

            if (orderPart == null)
            {
                return NotFound();
            }

            return orderPart;
        }

        [HttpPost]
        public async Task<ActionResult<OrderPart>> PostOrderPart(OrderPart orderPart)
        {
            _context.OrderParts.Add(orderPart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderPart", new { id = orderPart.Id }, orderPart);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderPart(int id, OrderPart orderPart)
        {
            if (id != orderPart.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderPart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderPartExists(id))
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
        public async Task<IActionResult> DeleteOrderPart(int id)
        {
            var orderPart = await _context.OrderParts.FindAsync(id);
            if (orderPart == null)
            {
                return NotFound();
            }

            _context.OrderParts.Remove(orderPart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderPartExists(int id)
        {
            return _context.OrderParts.Any(e => e.Id == id);
        }
    }
}