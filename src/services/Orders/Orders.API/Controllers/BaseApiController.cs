using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Orders.API.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class BaseApiController : ControllerBase
    {
        
    }
}