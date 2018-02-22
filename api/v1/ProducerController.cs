using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace depot {

    [Route ("api/v1/producers")]
    [Authorize (AuthenticationSchemes = "Bearer")]
    public class ProducerController : Controller {

        private readonly IPartProducerService _partProducerService;

        public ProducerController (IPartProducerService partProducerService) {
            this._partProducerService = partProducerService;
        }

        [HttpGet]
        [Route ("list")]
        public IActionResult List (int id) {
            var list = this._partProducerService.List ();
            return Ok (list);
        }

    }
}