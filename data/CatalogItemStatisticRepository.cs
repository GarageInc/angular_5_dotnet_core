using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace depot {

    public interface ICatalogItemStatisticRepository {
        Task AddRange (List<CatalogItemStatistic> items);
        IQueryable<object> GetStatisticForCatalog (StatisticFilter filter);
    }

    public class CatalogItemStatisticRepository : ICatalogItemStatisticRepository {

        private DepotContext _dbContext;

        public CatalogItemStatisticRepository (DepotContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task AddRange (List<CatalogItemStatistic> items) {
            await this._dbContext.AddRangeAsync (items);
            await this._dbContext.SaveChangesAsync ();
        }

        public IQueryable<object> GetStatisticForCatalog (StatisticFilter filter) {
            return this._dbContext.CatalogItemStatistic.Where (x => x.CreatedAt <= filter.To && x.CreatedAt >= filter.From)
                .Include (y => y.CatalogItem)
                .ThenInclude (z => z.Producer)
                .GroupBy (y => y.CatalogItem)
                .OrderByDescending (y => y.Count ())
                .Skip (filter.Offset)
                .Take (filter.Count)
                .Select (x => new {
                    ProducerCode = x.Key.ProducerCode,
                        ProducerName = x.Key.Producer.Name,
                        RequestsCount = x.Count (),
                        CreatedAt = x.FirstOrDefault ().CreatedAt,
                        CatalogItemEnName = x.Key.EnName,
                        CatalogItemRuName = x.Key.RuName
                });
        }
    }

}