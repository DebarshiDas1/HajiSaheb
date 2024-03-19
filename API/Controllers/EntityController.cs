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
    /// Controller responsible for managing entity-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting entity information.
    /// </remarks>
    [Route("api/entity")]
    [Authorize]
    public class EntityController : ControllerBase
    {
        private readonly HajiSahebContext _context;

        public EntityController(HajiSahebContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new entity to the database</summary>
        /// <param name="model">The entity data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Entity",Entitlements.Create)]
        public IActionResult Post([FromBody] Entity model)
        {
            _context.Entity.Add(model);
            this._context.SaveChanges();
            return Ok(new { model.Id });
        }

        /// <summary>Retrieves a list of entitys based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of entitys</returns>
        [HttpGet]
        [UserAuthorize("Entity",Entitlements.Read)]
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

            var query = _context.Entity.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<Entity>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific entity by its primary key</summary>
        /// <param name="entityId">The primary key of the entity</param>
        /// <returns>The entity data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Entity",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.Entity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific entity by its primary key</summary>
        /// <param name="entityId">The primary key of the entity</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Entity",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.Entity.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.Entity.Remove(entityData);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific entity by its primary key</summary>
        /// <param name="entityId">The primary key of the entity</param>
        /// <param name="updatedEntity">The entity data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Entity",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] Entity updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.Entity.Update(updatedEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific entity by its primary key</summary>
        /// <param name="entityId">The primary key of the entity</param>
        /// <param name="updatedEntity">The entity data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("Entity",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<Entity> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var existingEntity = this._context.Entity.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
                return NotFound();
            updatedEntity.ApplyTo(existingEntity, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            this._context.Entity.Update(existingEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }
    }
}