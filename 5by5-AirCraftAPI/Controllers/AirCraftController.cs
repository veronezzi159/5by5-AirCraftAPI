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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _5by5_AirCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirCraftController : ControllerBase
    {
        private readonly _5by5_AirCraftAPIContext _context;
        private readonly ServiceCnpj _serviceCnpj;
        private readonly ServiceDataFormat _serviceDataFormat;

        public AirCraftController(_5by5_AirCraftAPIContext context, ServiceCnpj cnpj,ServiceDataFormat dataFormat)
        {
            _context = context;
            _serviceCnpj = cnpj;
            _serviceDataFormat = dataFormat;
        }

        // GET: api/AirCraft
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AirCraftDTO>>> GetAirCraft()
        {
            var dto = new List<AirCraftDTO>();
          if (_context.AirCraft == null)
          {
              return NotFound();
          }
           var BD =  await _context.AirCraft.ToListAsync();
            foreach(var item in BD)
            {
                dto.Add(new AirCraftDTO { Rab = item.Rab,Capacity = item.Capacity, DTRegistry = _serviceDataFormat.MaskDate(item.DTRegistry),DTLastFlight = _serviceDataFormat.MaskDate(item.DTLastFlight),CnpjCompany = item.CnpjCompany });
            }
            return dto;
        }

        // GET: api/AirCraft/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AirCraft>> GetAirCraft(string id)
        {
           
           
          if (_context.AirCraft == null)
          {
              return NotFound();
          }
            var airCraft = await _context.AirCraft.FindAsync(id);

            if (airCraft == null)
            {
                return NotFound();
            }

            return airCraft;
        }

        // PUT: api/AirCraft/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirCraft(string id, AirCraft airCraft)
        {
            if (id != airCraft.Rab)
            {
                return BadRequest();
            }

            _context.Entry(airCraft).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirCraftExists(id))
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
        [HttpPost]
        public async Task<ActionResult<AirCraft>> PostAirCraft(AirCraft airCraft)
        {
            if (_serviceCnpj.ValidationCnpj(airCraft.CnpjCompany) == "")
            {
                return Problem("Cnpj not found");
            }

          if (_context.AirCraft == null)
          {
              return Problem("Entity set '_5by5_AirCraftAPIContext.AirCraft'  is null.");
            }
            airCraft.CnpjCompany = _serviceCnpj.CnpjMask(airCraft.CnpjCompany);
            _context.AirCraft.Add(airCraft);
            try
            {
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
            return CreatedAtAction("GetAirCraft", new { id = airCraft.Rab }, airCraft);
        }

        // DELETE: api/AirCraft/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirCraft(string id)
        {
            if (_context.AirCraft == null)
            {
                return NotFound();
            }
            var airCraft = await _context.AirCraft.FindAsync(id);
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
