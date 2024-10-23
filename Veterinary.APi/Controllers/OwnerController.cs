using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinary.API.Data;
using Veterinary.Shared.Entities;

namespace Veterinary.API.Controllers
{

    [ApiController]
    [Route("/api/owners")]
    public class OwnerController:ControllerBase
    {
        private readonly DataContext _context;

        public OwnerController(DataContext context)
        {
            _context = context;
        }

        //get de lista -Select * from owners
        [HttpGet]
        public async Task<ActionResult> Get() {
            return Ok(await _context.Owners.ToListAsync());
        }

        // Get con parametor {id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var busqueda = await _context.Owners.FirstOrDefaultAsync(x => x.Id==id);

            if (busqueda == null)
            {
                return NotFound();
            }
            else {
                return Ok(busqueda);
            }
        }
        //crear un nuevo owner
        [HttpPost]
        public async Task<ActionResult> Post(Owner owner) {
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return Ok(owner);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Owner owner)
        {
            _context.Owners.Update(owner);
            await _context.SaveChangesAsync();
            return Ok(owner);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var filasAfectadas = await _context.Owners.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (filasAfectadas == 0)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }

        }
    }
}
