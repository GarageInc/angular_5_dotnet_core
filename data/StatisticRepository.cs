namespace depot {

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IStatisticRepository {
        object GetFull ();
    }

    public class StatisticRepository : IStatisticRepository {

        private DepotContext _dbContext;
        public StatisticRepository (DepotContext dbContext) {
            this._dbContext = dbContext;
        }

        public object GetFull () {

            return new {
                counters = new {
                    users = this._dbContext.Users.Count (),
                    suppliers = this._dbContext.PartsSuppliers.Count (),
                    salePoints = this._dbContext.SalePoints.Count (),
                    producers = this._dbContext.PartProducers.Count (),
                    parts = new {
                    common = this._dbContext.SupplierPriceItems.Count (),
                    active = this._dbContext.SupplierPriceItems.Count (x => x.Status == PartStatusEnum.Active),
                    blocked = this._dbContext.SupplierPriceItems.Count (x => x.Status == PartStatusEnum.Blocked),
                    onModeration = this._dbContext.SupplierPriceItems.Count (x => x.Status == PartStatusEnum.OnModeration),
                    }
                    }
            };
        }
    }
}