namespace depot {

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public interface IPartSupplierRepository {
        Task<PartsSupplier> CreateSupplierIfNotExist (PartsSupplier model);

        PartsSupplier GetByName (string name);
        IQueryable<object> ListFor (int partId);
        Task<PartsSupplier> GetCompanyOf (int userId);
        IQueryable<PartsSupplier> GetAll ();
    }

    public class PartSupplierRepository : IPartSupplierRepository {

        private DepotContext _dbContext;

        public PartSupplierRepository (DepotContext dbContext) {
            this._dbContext = dbContext;
        }

        public IQueryable<PartsSupplier> GetAll () => this._dbContext.PartsSuppliers.Where (x => !x.IsDeleted);
        public async Task<PartsSupplier> GetCompanyOf (int userId) {
            var user = await this._dbContext.Users.Include (u => u.PartsSupplier).FirstAsync (x => x.Id == userId);

            return user.PartsSupplier;
        }

        public IQueryable<object> ListFor (int partId) {
            return this._dbContext.SupplierPriceItems.Include (x => x.PartsSupplier)
                .Where (x => x.ID == partId)
                .Select (x => new {
                    Id = x.PartsSupplier.ID,
                        Name = x.PartsSupplier.Name,
                        PartName = x.Name
                });
        }

        public async Task<PartsSupplier> CreateSupplierIfNotExist (PartsSupplier model) {
            var findByName = this._dbContext
                .PartsSuppliers
                .Include (x => x.SalePoints)
                .ThenInclude (x => x.TimeWorks)
                .FirstOrDefault (x => x.Name == model.Name);
            if (findByName == null) {
                findByName = model;

                await this._dbContext.AddAsync (findByName);
            } else {
                foreach (var sp in model.SalePoints) {
                    var oldSalePoint = findByName.SalePoints.FirstOrDefault (x => x.Name == sp.Name);

                    if (oldSalePoint == null) {
                        findByName.SalePoints.Add (sp);
                    } else {
                        oldSalePoint.UpdateTimeWorks (sp);
                        this._dbContext.UpdateRange (oldSalePoint.TimeWorks);
                    }
                }

                this._dbContext.UpdateRange (findByName.SalePoints);
                this._dbContext.Update (findByName);
            }

            await this._dbContext.SaveChangesAsync ();

            return findByName;
        }

        public PartsSupplier GetByName (string name) {
            var findByName = this._dbContext.PartsSuppliers.Where (x => x.SearchName == name)
                .Include (x => x.Logo)

                .Include (x => x.Warehouses)
                .ThenInclude (x => x.Contacts)
                .ThenInclude (x => x.Address)

                .Include (x => x.Warehouses)
                .ThenInclude (x => x.Address)

                .Include (x => x.Contacts)

                .Include (x => x.SalePoints)
                .ThenInclude (s => s.Address)

                .Include (x => x.SalePoints)
                .ThenInclude (s => s.TimeWorks)

                .Include (x => x.SeoParameter)
                .FirstOrDefault ();

            if (findByName != null) {
                foreach (var warehouse in findByName.Warehouses) {
                    warehouse.Supplier = null;
                }
            }

            return findByName;
        }
    }
}