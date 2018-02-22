using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;

namespace depot {

    public interface ISupplierOfferFilesRepository {
        IQueryable<SupplierOfferFile> NeedProcessingFor (PartsSupplier partsSupplier);
        IQueryable<object> AllFor (PartsSupplier partsSupplier);
        Task AddRange (IEnumerable<SupplierOfferFile> entities);
        Task SaveRange (IEnumerable<SupplierOfferFile> entities);

        Task<bool> Remove (string guid);
    }

    public class SupplierOfferFilesRepository : ISupplierOfferFilesRepository {
        private DepotContext _dbContext;

        public SupplierOfferFilesRepository (DepotContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task AddRange (IEnumerable<SupplierOfferFile> entities) {
            await this._dbContext.AddRangeAsync (entities);
            await this._dbContext.SaveChangesAsync ();
        }
        public async Task SaveRange (IEnumerable<SupplierOfferFile> entities) {
            this._dbContext.UpdateRange (entities);
            await this._dbContext.SaveChangesAsync ();
        }

        public async Task<bool> Remove (string guid) {
            var items = this._dbContext.SupplierOfferFile.Where (x => x.GroupIdentifier == guid).ToList ();

            items.ForEach (i => i.IsDeleted = true);

            await this.SaveRange (items);

            return true;
        }

        public IQueryable<object> AllFor (PartsSupplier partsSupplier) => this._dbContext.SupplierOfferFile
            .Where (x => !x.IsDeleted && x.PartsSupplier == partsSupplier)
            .OrderByDescending (x => x.GroupIdentifier)
            .GroupBy (x => x.GroupIdentifier).Select (i => new {
                groupIdentifier = i.Key,
                    files = i.Select (f => new {
                        Id = f.ID,
                            CreatedAt = f.CreatedAt,
                            Name = f.Name,
                            Status = f.Status,
                            ErrorMessage = f.ErrorMessage,
                            GroupIdentifier = f.GroupIdentifier
                    })
            });

        public IQueryable<SupplierOfferFile> NeedProcessingFor (PartsSupplier partsSupplier) => this._dbContext.SupplierOfferFile.Where (x => !x.IsDeleted && x.Status == OfferFileStatusEnum.Downloaded && x.PartsSupplier == partsSupplier);
    }

}