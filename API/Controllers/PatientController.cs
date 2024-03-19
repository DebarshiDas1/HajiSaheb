using Microsoft.AspNetCore.Mvc;
using HajiSaheb.Models;
using HajiSaheb.Data;
using HajiSaheb.Filter;
using HajiSaheb.Entities;
using HajiSaheb.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace HajiSaheb.Controllers
{
    [Route("api/patient")]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly HajiSahebContext _context;

        public PatientController(HajiSahebContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new patient to the database</summary>
        /// <param name="model">The patient data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Patient",Entitlements.Create)]
        public IActionResult Post([FromBody] Patient model)
        {
            _context.Patient.Add(model);
            this._context.SaveChanges();
            return Ok(new { model.Id });
        }

        /// <summary>Retrieves a list of patients based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of patients</returns>
        [HttpGet]
        [UserAuthorize("Patient",Entitlements.Read)]
        public IActionResult Get([FromQuery] string filters, int pageNumber = 1, int pageSize = 10)
        {
            List<FilterCriteria> filterCriteria = null;
            if (pageSize < 1)
            {
                return BadRequest("Page size invalid.");
            }

            if (pageNumber < 1)
            {
                return BadRequest("Page mumber invalid.");
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.Patient.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Patient>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }
    }
}