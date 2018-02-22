using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace depot {

    public interface IStatisticService {
        Task IncrementFor (string producerCode, string producerName);
        IQueryable<object> GetStatisticForCatalog (StatisticFilter filter);
    }

    public class StatisticService : IStatisticService {

        public readonly ICatalogItemRepository _catalogItemRepository;
        public readonly IProducerCodeService _producerCodeService;
        public readonly ICatalogItemStatisticRepository _catalogItemStatisticRepository;

        public StatisticService (ICatalogItemRepository catalogItemRepository, IProducerCodeService producerCodeService, ICatalogItemStatisticRepository catalogItemStatisticRepository) {
            this._catalogItemRepository = catalogItemRepository;
            this._producerCodeService = producerCodeService;
            this._catalogItemStatisticRepository = catalogItemStatisticRepository;
        }

        public async Task IncrementFor (string producerCode, string producerName) {
            var code = _producerCodeService.TrimCode (producerCode);

            var items = this._catalogItemRepository.GetMatch (producerCode, producerName)
                .Where (x => x.ProducerCodeTrimmed == code).ToList ();

            if (items.Any ()) {
                var list = new List<CatalogItemStatistic> ();
                foreach (var item in items) {
                    list.Add (new CatalogItemStatistic {
                        CatalogItem = item,
                    });
                }

                await this._catalogItemStatisticRepository.AddRange (list);
            }

        }
        public IQueryable<object> GetStatisticForCatalog (StatisticFilter filter) {
            return this._catalogItemStatisticRepository.GetStatisticForCatalog (filter);
        }
    }

}