using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace depot {

    [Route ("api/v1/statistic")]
    public class StatisticController : Controller {

        private readonly IStatisticRepository _StatisticRepository;
        private readonly IStatisticService _statisticService;
        public StatisticController (IStatisticRepository StatisticRepository, IStatisticService statisticService) {
            this._StatisticRepository = StatisticRepository;
            this._statisticService = statisticService;
        }

        [HttpGet]
        [Route ("full")]
        [Authorize (AuthenticationSchemes = "Bearer")]
        public IActionResult GetStatistic () {
            var statistic = this._StatisticRepository.GetFull ();
            return Ok (statistic);
        }

        [Route ("get-for-catalog")]
        [Authorize (AuthenticationSchemes = "Bearer")]
        public IActionResult GetStatisticForCatalog ([FromBody] StatisticFilter model) {
            var statistic = this._statisticService.GetStatisticForCatalog (model);
            return Ok (statistic);
        }

        [HttpPost]
        [Route ("check")]
        public async Task<IActionResult> Increment ([FromBody] PartStatisticeModel model) {
            await this._statisticService.IncrementFor (model.ProducerCode, model.ProducerName);
            return Ok (true);
        }
    }

    public class PartStatisticeModel {
        public string ProducerName { get; set; }
        public string ProducerCode { get; set; }
    }

    public class StatisticFilter {
        public int From { get; set; }
        public int To { get; set; }

        public int Offset { get; set; }
        public int Count { get; set; }
    }
}