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
    /// Controller responsible for managing userinrole-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting userinrole information.
    /// </remarks>
    [Route("api/userinrole")]
    [Authorize]
    public class UserInRoleController : ControllerBase
    {
        private readonly HajiSahebContext _context;

        public UserInRoleController(HajiSahebContext context)
        {
            _context = context;
        }

        /// <summary>Adds a new userinrole to the database</summary>
        /// <param name="model">The userinrole data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("UserInRole",Entitlements.Create)]
        public IActionResult Post([FromBody] UserInRole model)
        {
            _context.UserInRole.Add(model);
            this._context.SaveChanges();
            return Ok(new { model.Id });
        }

        /// <summary>Retrieves a list of userinroles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The filtered list of userinroles</returns>
        [HttpGet]
        [UserAuthorize("UserInRole",Entitlements.Read)]
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

            var query = _context.UserInRole.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<UserInRole>.ApplyFilter(query, filterCriteria);
            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return Ok(paginatedResult);
        }

        /// <summary>Retrieves a specific userinrole by its primary key</summary>
        /// <param name="entityId">The primary key of the userinrole</param>
        /// <returns>The userinrole data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("UserInRole",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.UserInRole.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific userinrole by its primary key</summary>
        /// <param name="entityId">The primary key of the userinrole</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("UserInRole",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.UserInRole.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.UserInRole.Remove(entityData);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific userinrole by its primary key</summary>
        /// <param name="entityId">The primary key of the userinrole</param>
        /// <param name="updatedEntity">The userinrole data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("UserInRole",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] UserInRole updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            this._context.UserInRole.Update(updatedEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }

        /// <summary>Updates a specific userinrole by its primary key</summary>
        /// <param name="entityId">The primary key of the userinrole</param>
        /// <param name="updatedEntity">The userinrole data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPatch]
        [UserAuthorize("UserInRole",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] JsonPatchDocument<UserInRole> updatedEntity)
        {
            if (updatedEntity == null)
                return BadRequest("Patch document is missing.");
            var existingEntity = this._context.UserInRole.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
                return NotFound();
            updatedEntity.ApplyTo(existingEntity, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            this._context.UserInRole.Update(existingEntity);
            var status = this._context.SaveChanges();
            return Ok(new { status });
        }
    }
}