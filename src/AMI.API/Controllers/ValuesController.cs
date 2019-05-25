using System.Collections.Generic;
using AMI.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to values.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route("values")]
    [ApiController]
    public class ValuesController : BaseController
    {
        /// <summary>
        /// Gets a list of values.
        /// </summary>
        /// <returns>A list of values.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Gets value with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The value with the specified identifier.</returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            throw new NotFoundException("Value", id);
        }

        /// <summary>
        /// Posts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// Puts the value with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// Deletes the value with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
