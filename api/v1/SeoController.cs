using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace depot {

    [Route ("api/v1/seo")]
    public class SeoController : Controller {

        private readonly ISeoRepository _seoRepository;
        private readonly ISeoService _seoService;

        public SeoController (ISeoRepository seoRepository, ISeoService seoService) {
            this._seoRepository = seoRepository;
            this._seoService = seoService;
        }

        [HttpGet]
        [Route ("get")]
        public async Task<IActionResult> GetSeo (int id) {
            var seoParemeter = await this._seoRepository.Get (id);
            return Ok (seoParemeter);
        }

        [HttpGet]
        [Route ("get-robots")]
        [Authorize (AuthenticationSchemes = "Bearer")]
        public IActionResult GetRobots () {
            var robots = this._seoService.GetRobots ();
            return Ok (robots);
        }

        [HttpPost]
        [Route ("update-robots")]
        [Authorize (AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateRobots ([FromBody] RobotsTxt model) {
            this._seoService.UpdateRobots (model);
            return Ok (true);
        }

        [HttpPost]
        [Route ("update-search-seo")]
        [Authorize (AuthenticationSchemes = "Bearer")]
        public IActionResult UpdateSearchSeo ([FromBody] GlobalSeo seo) {
            this._seoRepository.UpdateSearchSeo (seo);
            return Ok (true);
        }

        [Route ("save-part-seo")]
        [HttpPost]
        [Authorize (AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateSearchSeo ([FromBody] SeoParameterSavingModel model) {
            await this._seoRepository.SaveCatalogItemSeo (model.model, model.catalogItemId);
            return Ok (true);
        }
    }

    public class SeoParameterSavingModel {
        public SeoParameter model { get; set; }
        public int catalogItemId { get; set; }
    }
}