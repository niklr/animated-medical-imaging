using System.Net;
using System.Threading.Tasks;
using AMI.Core.Entities.Shared.Commands;
using Microsoft.AspNetCore.Mvc;
using Models = AMI.Core.Entities.Models;

namespace AMI.API.Controllers
{
    /// <summary>
    /// The endpoints related to tasks.
    /// </summary>
    /// <seealso cref="BaseController" />
    [ApiController]
    [Route("tasks")]
    public class TasksController : BaseController
    {
    }
}
