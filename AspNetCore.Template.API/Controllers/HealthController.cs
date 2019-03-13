using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Template.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HealthController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult<string> Health()
        {
            return _hostingEnvironment.EnvironmentName;
        }
    }
}
