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
    /// Controller responsible for managing role-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting role information.
    /// </remarks>
    [Route("api/role")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly HajiSahebContext _context;

        public RoleController(HajiSahebContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new role to the database</summary>
        /// <param name="model">The role data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Role",Entitlements.Create)]
        public IActionResult Post([FromBody] Role model)
        {
            _context.Role.Add(model);
            this._context.SaveChanges();
            return Ok(new { model.Id });
        }

        /// <summary>Retrieves a list of roles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of roles</returns>
        [HttpGet]
        [UserAuthorize("Role",Entitlements.Read)]
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

            var query = _context.Role.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Role>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific role by its primary key</summary>
        /// <param name="entityId">The primary key of the role</param>
        /// <returns>The role data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Role",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.Role.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific role by its primary key</summary>
        /// <param name="entityId">The primary key of the role</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Role",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.Role.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.Role.Remove(entityData);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific role by its primary key</summary>
        /// <param name="entityId">The primary key of the role</param>
        /// <param name="updatedEntity">The role data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Role",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] Role updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.Role.Update(updatedEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific role by its primary key</summary>
        /// <param name="entityId">The primary key of the role</param>
        /// <param name="updatedEntity">The role data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("Role",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<Role> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var existingEntity = this._context.Role.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
                return NotFound();
            updatedEntity.ApplyTo(existingEntity, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            this._context.Role.Update(existingEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }
    }
}