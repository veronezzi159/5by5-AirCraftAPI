using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _5by5_AirCraftAPI.Data;
using _5by5_AirCraftAPI.Models;
using _5by5_AirCraftAPI.Services;
using System.Diagnostics.Contracts;

namespace _5by5_AirCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirCraftController : ControllerBase
    {
        private readonly _5by5_AirCraftAPIContext _context;
        private readonly ServiceCapacity _serviceCapacity;

        public AirCraftController(_5by5_AirCraftAPIContext context)
        {
            _context = context;
            _serviceCapacity = new ServiceCapacity();
        }

        // GET: api/AirCraft
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirCraft>>> GetAirCraft()
        {
          if (_context.AirCraft == null)
          {
              return NotFound();
          }
            return await _context.AirCraft.ToListAsync();
        }

        // GET: api/AirCraft/5
        [HttpGet("{rab}")]
        public async Task<ActionResult<AirCraft>> GetAirCraft(string rab)
        {
           
           
          if (_context.AirCraft == null)
          {
              return NotFound();
          }
            var airCraft = await _context.AirCraft.FindAsync(rab);

            if (airCraft == null)
            {
                return NotFound();
            }

            return airCraft;
        }

        // PUT: api/AirCraft/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //aqui é o link que ele manda para a documentação do asp.net para proteger contra overposting attacks, é um link que ensina como proteger a aplicação contra ataques de overposting e overposting é quando o usuário envia mais dados do que o necessário para a aplicação, então é uma forma de proteger a aplicação contra isso.
        
        
        [HttpPut("{rab}")]
        public async Task<IActionResult> PutAirCraft(string rab, AirCraft airCraft) //Aqui ele usa o método PutAirCraft para atualizar uma aeronave e ele passa o id e a aeronave que ele quer atualizar
        {
            if (rab != airCraft.Rab)
            {
                return BadRequest();
            }
            
            //Nesse If ele verifica a capacidade da aeronave, se a capacidade for menor ou igual a zero, ele retorna um erro de conflito
            if (!_serviceCapacity.verifyCapacity(airCraft))
            {
                return Conflict("Capacidade invalida");
            } 

            _context.Entry(airCraft).State = EntityState.Modified;//Aqui ele muda o estado da aeronave para modificado no banco de dados

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirCraftExists(rab))
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

        // POST: api/AirCraft
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754 
        
        [HttpPost] //aqui é o método que ele usa para criar uma nova aeronave e ele usa o método PostAirCraft para fazer isso
        public async Task<ActionResult<AirCraft>> PostAirCraft(AirCraft airCraft)
        {
          if (_context.AirCraft == null)
          {
              return Problem("Entity set '_5by5_AirCraftAPIContext.AirCraft'  is null.");
          }
          
          //Esse If verifica a capacidade da aeronave, se a capacidade for menor ou igual a zero ou maior que 240, ele retorna um erro de conflito
          if (!_serviceCapacity.verifyCapacity(airCraft))
            {
                return Conflict("Capacidade invalida");
            }
          
            try
            {
                _context.AirCraft.Add(airCraft);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AirCraftExists(airCraft.Rab))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAirCraft", new { rab = airCraft.Rab }, airCraft);
        }

        // DELETE: api/AirCraft/5
        [HttpDelete("{rab}")]
        public async Task<IActionResult> DeleteAirCraft(string rab)
        {
            if (_context.AirCraft == null)
            {
                return NotFound();
            }
            var airCraft = await _context.AirCraft.FindAsync(rab);
            if (airCraft == null)
            {
                return NotFound();
            }

            _context.AirCraft.Remove(airCraft);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{rab}")]

        private bool AirCraftExists(string id)
        {
            return (_context.AirCraft?.Any(e => e.Rab == id)).GetValueOrDefault();
        }
    }
}
