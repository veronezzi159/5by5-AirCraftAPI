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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _5by5_AirCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirCraftController : ControllerBase
    {
        private readonly _5by5_AirCraftAPIContext _context;
        private readonly ServiceCapacity _serviceCapacity;
        private readonly ServiceCnpj _serviceCnpj;
        private readonly ServiceDataFormat _serviceDataFormat;
        private readonly ServiceRAB _serviceRAB;

        public AirCraftController(_5by5_AirCraftAPIContext context, ServiceCnpj cnpj, ServiceDataFormat dataFormat, ServiceRAB rab, ServiceCapacity capacity)
        {
            _context = context;
            _serviceCnpj = cnpj;
            _serviceDataFormat = dataFormat;
            _serviceRAB = rab;
            _serviceCapacity = capacity;
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
            var BD = await _context.AirCraft.ToListAsync();
            foreach (var item in BD)
            {
                dto.Add(new AirCraftDTO { Rab = item.Rab, Capacity = item.Capacity, DTRegistry = _serviceDataFormat.MaskDate(item.DTRegistry), DTLastFlight = _serviceDataFormat.MaskDate(item.DTLastFlight), CnpjCompany = item.CnpjCompany });
            }
            return dto;
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
            rab = rab.ToUpper();

            if (rab != airCraft.Rab)
            {
                return BadRequest("O ID fornecido não corresponde ao Rab da aeronave.");
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

        [HttpPost] //aqui é o método que ele usa para criar uma nova aeronave e ele usa o método PostAirCraft para fazer isso
        public async Task<ActionResult<AirCraft>> PostAirCraft(AirCraftPost airCraftPost)
        {
            var airCraft = new AirCraft(airCraftPost);

            airCraft.CnpjCompany = _serviceCnpj.ValidationCnpj(airCraft.CnpjCompany);
            if (airCraft.CnpjCompany == String.Empty)
            {
                return Problem("Cnpj não encontrado");
            }
            
            //Esse If verifica a capacidade da aeronave, se a capacidade for menor ou igual a zero ou maior que 240, ele retorna um erro de conflito
            if (!_serviceCapacity.verifyCapacity(airCraft))
            {
                return Conflict("Capacidade invalida");
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
                return Problem("Uma Aeronave com este RAB está removida. Não foi possível cadastrar!");
            }

            if (AirCraftExists(airCraft.Rab))
            {
                return Problem("Uma Aeronave com este RAB já existe.");
            }

            try
            {
                _context.AirCraft.Add(airCraft);
                if (_context.AirCraft == null)
                {
                    return Problem("Entity set '_5by5_AirCraftAPIContext.AirCraft'  is null.");
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetAirCraft", new { rab = airCraft.Rab }, airCraft);
        }



        // DELETE: api/AirCraft/5
        [HttpDelete("{rab}")]
        public async Task<IActionResult> DeleteAirCraft(string rab)
        {
            rab = rab.ToUpper();

            var airCraft = await _context.AirCraft.FindAsync(rab);

            if (AirCraftIsRemoved(rab))
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


                if (!AirCraftExists(rab))
                {
                    return Ok("Aeronave removida com sucesso.");
                }

                return NoContent();
            }
            catch (DbUpdateConcurrencyException dbc)
            {
                return Problem($"Aeronave não removida,DbUpdateConcurrencyException \n {dbc.Message}");
            }
            catch (DbUpdateException dbe)
            {
                return Problem($"Aeronave não removida,DbUpdateExceptio \n {dbe.Message}");
            }
            catch (Exception e)
            {
                return Problem($"Aeronave não removida \n {e.Message}");
            }
        }

        [HttpPut("UpdateData/{rab}/{newDate}")]
        public async Task<IActionResult> UpdateData(string rab, string newDate)
        {
            var airCraft = await _context.AirCraft.FindAsync(rab);
            var date = DateTime.Parse(newDate);
            if (airCraft == null)
            {
                return NotFound();
            }
            if (date < airCraft.DTLastFlight)
            {
                return BadRequest("A data é anterior ao último voo");
            }

            airCraft.DTLastFlight = date;
            _context.Update(airCraft);

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                return Problem("Erro ao atualizar a data");
            }


            return NoContent();
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


