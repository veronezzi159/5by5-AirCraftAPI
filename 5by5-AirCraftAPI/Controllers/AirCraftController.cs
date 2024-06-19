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
        private readonly ServiceRAB _serviceRAB;

        public AirCraftController(_5by5_AirCraftAPIContext context, ServiceCnpj cnpj,ServiceDataFormat dataFormat)
        {
            _context = context;
            _serviceCnpj = cnpj;
            _serviceDataFormat = dataFormat;
            _serviceRAB = new ServiceRAB();
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
            id = id.ToUpper(); 

            if (id != airCraft.Rab)
            {
                return BadRequest("O ID fornecido não corresponde ao Rab da aeronave.");
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
                    return NotFound("Aeronave não encontrada.");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Aeronave atualizada com sucesso.");
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
          if (airCraft.CnpjCompany.Length == 14)
            {
                airCraft.CnpjCompany = _serviceCnpj.CnpjMask(airCraft.CnpjCompany);
            }
            _context.AirCraft.Add(airCraft);
            if (_context.AirCraft == null)
            {
                return Problem("Entity set '_5by5_AirCraftAPIContext.AirCraft' is null.");
            }

            try
            {
                airCraft.Rab = _serviceRAB.ValidateRAB(airCraft.Rab);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            if (AirCraftIsRemoved(airCraft.Rab))
            {
                return Conflict("Uma Aeronave com este RAB está removida. Não foi possível cadastrar!");
            }

            if (AirCraftExists(airCraft.Rab))
            {
                return Conflict("Uma Aeronave com este RAB já existe.");
            }

            try
            {
                _context.AirCraft.Add(airCraft);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw; 
            }

            return CreatedAtAction("GetAirCraft", new { id = airCraft.Rab }, airCraft);
        }



        // DELETE: api/AirCraft/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirCraft(string id)
        {
            id = id.ToUpper();

            var airCraft = await _context.AirCraft.FindAsync(id);

            if (AirCraftIsRemoved(id))
            {
                return Conflict("A Aeronave já está removida.");
            }

            else if (airCraft == null)
            {
                return Conflict("Aeronave não encontrada no cadastro.");
            }
            

            try
            {
                var copyAirCraft = CopyAirCraft(airCraft);

                _context.AirCraft.Remove(airCraft);

                _context.Removed.Add(copyAirCraft);

                await _context.SaveChangesAsync();

                
                if (!AirCraftExists(id))
                {
                    return Ok("Aeronave removida com sucesso.");
                }

                return NoContent(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir aeronave: {ex.Message}");
            }
        }

        private Removed CopyAirCraft(AirCraft airCraft)
        {
            var removed = new Removed
            {
                Rab = airCraft.Rab,
                Capacity = airCraft.Capacity,
                DTRegistry = airCraft.DTRegistry,
                DTLastFlight = airCraft.DTLastFlight,
                CnpjCompany = airCraft.CnpjCompany
            };
            return removed;
        }


        public bool AirCraftExists(string id)
        {
            return _context.AirCraft.Any(e => e.Rab == id);
        }

       public bool AirCraftIsRemoved(string id)
        {
            return _context.Removed.Any(e => e.Rab == id);
        }   


    }
}


