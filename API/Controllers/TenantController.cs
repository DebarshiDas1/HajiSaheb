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
    /// <summary>
    /// Controller responsible for managing tenant-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting tenant information.
    /// </remarks>
    [Route("api/tenant")]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private readonly HajiSahebContext _context;

        public TenantController(HajiSahebContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new tenant to the database</summary>
        /// <param name="model">The tenant data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Tenant",Entitlements.Create)]
        public IActionResult Post([FromBody] Tenant model)
        {
            _context.Tenant.Add(model);
            this._context.SaveChanges();
            return Ok(new { model.Id });
        }

        /// <summary>Retrieves a list of tenants based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of tenants</returns>
        [HttpGet]
        [UserAuthorize("Tenant",Entitlements.Read)]
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

            var query = _context.Tenant.AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Tenant>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific tenant by its primary key</summary>
        /// <param name="entityId">The primary key of the tenant</param>
        /// <returns>The tenant data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Tenant",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.Tenant.FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific tenant by its primary key</summary>
        /// <param name="entityId">The primary key of the tenant</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Tenant",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.Tenant.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.Tenant.Remove(entityData);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific tenant by its primary key</summary>
        /// <param name="entityId">The primary key of the tenant</param>
        /// <param name="updatedEntity">The tenant data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Tenant",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] Tenant updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.Tenant.Update(updatedEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific tenant by its primary key</summary>
        /// <param name="entityId">The primary key of the tenant</param>
        /// <param name="updatedEntity">The tenant data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("Tenant",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<Tenant> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var existingEntity = this._context.Tenant.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
                return NotFound();
            updatedEntity.ApplyTo(existingEntity, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            this._context.Tenant.Update(existingEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }
    }
}