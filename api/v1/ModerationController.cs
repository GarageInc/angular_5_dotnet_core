using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace depot {

    [Route ("api/v1/moderation")]
    [Authorize (AuthenticationSchemes = "Bearer")]
    public class ModerationController : Controller {

        private readonly IModerationService _moderationService;
        public ModerationController (IModerationService moderationService) {
            this._moderationService = moderationService;
        }

        [HttpPost]
        [Route ("upload-catalog-item-image")]
        public async Task<IActionResult> UploadCatalogItemImage ([FromQuery] string producerCode, [FromQuery] int producerId) {

            FileValidator.Validate (Request.Form.Files, new [] { "jpg" });

            var file = Request.Form.Files.FirstOrDefault ();
            await this._moderationService.SaveFileFor (producerCode, producerId, file);
            return Ok (true);
        }

        [HttpGet]
        [Route ("need-moderation-count")]
        public IActionResult GetModerationCount () {
            var Moderation = this._moderationService.NeedModeration (null).Count ();
            return Ok (Moderation);
        }

        [Route ("need-moderation")]
        public IActionResult GetModeration ([FromBody] ModerationFilterModel model) {
            var Moderation = this._moderationService.NeedModeration (model);
            return Ok (Moderation);
        }

        [HttpGet]
        [Route ("need-moderation-item")]
        public IActionResult GetModerationItem (int id) {
            var Moderation = this._moderationService.GetModerationItem (id);
            return Ok (Moderation);
        }

        [HttpGet]
        [Route ("delete")]
        public async Task<IActionResult> Delete (int id) {
            await this._moderationService.Delete (id);
            return Ok (true);
        }

        [HttpGet]
        [Route ("restore")]
        public async Task<IActionResult> Restore (int id) {
            await this._moderationService.Restore (id);
            return Ok (true);
        }

        [Route ("update")]
        public async Task<IActionResult> Update ([FromBody] ItemForModeration model) {
            await this._moderationService.Update (model);
            return Ok (true);
        }

        [Route ("create-catalog-item")]
        public async Task<IActionResult> CreateCatalogItem ([FromBody] ItemForModeration model) {
            await this._moderationService.CreateCatalogItem (model);
            return Ok (true);
        }

        [HttpGet]
        [Route ("suppliers-match-for")]
        public IActionResult SuppliersMatchFor (string producerCode) {
            var match = this._moderationService.SuppliersMatchForAsync (producerCode);
            return Ok (match);
        }

    }
}