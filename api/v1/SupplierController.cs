using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace depot {

    [Route ("api/v1/suppliers")]
    [Authorize (AuthenticationSchemes = "Bearer")]
    public class SupplierController : Controller {

        private readonly IPartSupplierService _partSupplierService;

        public SupplierController (IPartSupplierService partSupplierService) {
            this._partSupplierService = partSupplierService;
        }

        [HttpGet]
        [Route ("list-for")]
        public IActionResult List (int partId) {
            var list = this._partSupplierService.ListFor (partId);
            return Ok (list);
        }

    }
}