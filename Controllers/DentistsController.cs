using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using POC01.Model;
using System.Threading.Tasks;
using System.Linq;

namespace POC01.Controllers
{
    //[Authorize]
    [Route("api/{tenantId}/[controller]")]
    [Produces("application/json")]
    public class DentistsController : Controller
    {
        private IDentistRepository _repository;

        public DentistsController(IDentistRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Dentist))]
        [ProducesResponseType(404)]
        public IActionResult GetSinlge(string tenantId, int id)
        {
            Dentist d = _repository.GetSingle(tenantId, id);

            if(d != null)     
            {
                return Ok(d);
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
            //var pagination = Request.Headers["Pagination"];
            var d = _repository.GetAll(tenantId).Select(x => new { x.Id, x.Name, x.Specialty });
                      
            if (d.Count() >= 1)
            {
                return Ok(d);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/includePatients")]
        [ProducesResponseType(200, Type = typeof(List<Dentist>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSingleIncludePatientsAsync(string tenantId, int id)
        {
            var d = await _repository.GetPatientsAsync(tenantId, id);

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
    }
}