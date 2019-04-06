using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FreddyBaseApi.Core.Repositories.Base.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FreddyBaseApi.Core.Model.Extensions;

namespace FreddyBaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController<T> : ControllerBase where T : class
    {
        /// <summary>
        /// Repository acces property
        /// </summary>
        protected IRepository<T> Repository { get; set; }

        /// <summary>
        /// Get entity list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult<IEnumerable<T>> GetAll() => Repository.GetAll().ToList();

        /// <summary>
        /// Get specific entity by Id
        /// </summary>
        /// <param name="id">Entity primary key id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public virtual ActionResult<T> GetById(object id)
        {
            var entity = Repository.Find(id);

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        /// <summary>
        ///  Add new entity
        /// </summary>
        /// <param name="entity">Entity instance to add</param>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public virtual ActionResult<T> Post([FromBody] T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Repository.Add(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.GetEntityKeyValue() }, entity);
        }

        /// <summary>
        /// Update info to existing entity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public virtual IActionResult Put(int id, [FromBody] T entity)
        {
            var originalEntity = Repository.Find(id);
            if (originalEntity == null)
                return NotFound();

            entity.SetEntityKeyValue(id);
            Repository.Update(entity);

            return NoContent();
        }

        /// <summary>
        /// Delete existing entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public virtual IActionResult Delete(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
            {
                return NotFound();
            }

            Repository.Remove(id);
            return NoContent();
        }
    }
}