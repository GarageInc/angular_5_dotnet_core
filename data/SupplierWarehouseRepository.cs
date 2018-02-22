namespace depot {

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ISupplierWarehouseRepository {
        Task AddRange (IEnumerable<SupplierWarehouse> warehouses);
    }

    public class SupplierWarehouseRepository : ISupplierWarehouseRepository {

        private DepotContext _dbContext;

        public SupplierWarehouseRepository (DepotContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task AddRange (IEnumerable<SupplierWarehouse> warehouses) {
            await this._dbContext.AddRangeAsync (warehouses.Where (warehouse => !string.IsNullOrEmpty (warehouse.IdForProducer)));
            await this._dbContext.SaveChangesAsync ();
        }
    }
}