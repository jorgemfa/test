using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using POC01.Model;
using System.Linq;

namespace POC01.Controllers
{
    //[Authorize]
    [Route("api/{tenantId}/[controller]")]
    [Produces("application/json")]
    public class PatientsController : Controller
    {
        private IPatientRepository _repository;

        public PatientsController(IPatientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Patient))]
        [ProducesResponseType(404)]
        public IActionResult GetSinlge(string tenantId, int id)
        {
            Patient p = _repository.GetSingle(tenantId, id);

            if (p != null)
            {
                return Ok(p);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(List<Dentist>))]
        [ProducesResponseType(404)]
        public IActionResult GetAll(string tenantId)
        {
            var d = _repository.GetAll(tenantId).Where(x=>x.City.Equals("Santa Ana"));
            if (d.Count() >= 1)
            {
                return Ok(d);
            }
            else
            {
                return NotFound();
            }
        }
        
        [HttpGet("search/{query}")]
        [ProducesResponseType(200, Type = typeof(List<Dentist>))]
        [ProducesResponseType(404)]
        public IActionResult Search(string tenantId, string query)
        {
            var d = _repository.Search(tenantId, query);
            if (d.Count() >= 1)
            {
                return Ok(d);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("searchranked/{query}")]
        [ProducesResponseType(200, Type = typeof(List<Dentist>))]
        [ProducesResponseType(404)]
        public IActionResult SearchRanked(string tenantId, string query)
        {
            var d = _repository.SearchRanked(tenantId, query);
            if (d.Count() >= 1)
            {
                return Ok(d);
            }
            else
            {
                return NotFound();
            }
        }
    }
}