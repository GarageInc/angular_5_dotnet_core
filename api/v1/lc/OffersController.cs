using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace depot {

    [Route ("api/v1/lc/offers")]
    [Authorize (AuthenticationSchemes = "Bearer")]
    public class OffersController : Controller {
        private readonly IPartSupplierService _partSupplierService;

        private readonly UserManager<User> _userManager;
        public int UserId => int.Parse (this._userManager.GetUserId (this.User));
        public OffersController (IPartSupplierService partSupplierService, UserManager<User> userManager) {
            this._partSupplierService = partSupplierService;
            this._userManager = userManager;
        }

        [Route ("upload")]
        [HttpPost]
        public async Task<IActionResult> UploadOffers ([FromQuery] string identifier) {
            var files = Request.Form.Files;

            FileValidator.Validate (files, new [] { "xml", "xls", "xlsx" });

            await this._partSupplierService.UploadOffers (files, identifier, UserId);
            return Ok (true);
        }

        [HttpGet]
        [Route ("remove")]
        public async Task<IActionResult> Remove (string groupIdentifier) {
            return Ok (await _partSupplierService.RemoveOffers (groupIdentifier));
        }

        [HttpGet]
        [Route ("list")]
        public async Task<IActionResult> ListOffers () {
            return Ok (await _partSupplierService.ListOffers (UserId));
        }
    }
}