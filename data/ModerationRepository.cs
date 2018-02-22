namespace depot {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.EntityFrameworkCore;

    public interface IModerationRepository {
        IQueryable<ItemForModeration> NeedModeration (bool checkStatus = true, PartStatusEnum status = PartStatusEnum.OnModeration);

        ItemForModeration GetModerationItem (int id);
    }

    public class ModerationRepository : IModerationRepository {

        private DepotContext _dbContext;
        private readonly IProducerCodeService _producerCodeService;

        public ModerationRepository (DepotContext dbContext, IProducerCodeService producerCodeService) {
            this._producerCodeService = producerCodeService;
            this._dbContext = dbContext;
        }

        public IQueryable<ItemForModeration> NeedModeration (bool checkStatus = true, PartStatusEnum status = PartStatusEnum.OnModeration) {
            var query =
            from x in this._dbContext.SupplierPriceItems
            join y in this._dbContext.ProducerCatalogItems on new {
            x.ProducerCodeTrimmed,
            x.ProducerId
            }
            equals new {
            y.ProducerCodeTrimmed,
            y.ProducerId
            }
            into ps
            from p in ps.DefaultIfEmpty ()
            select new ItemForModeration {
            Id = x.ID,
            SupplierId = x.PartsSupplierId,
            SupplierName = x.PartsSupplier.Name,
            ProducerName = x.Producer.Name,
            ProducerCodeTrimmed = x.ProducerCodeTrimmed,
            ProducerCode = x.ProducerCode,
            ProducerId = x.ProducerId,

            SeoParameterId = p.SeoParameterId,

            IsDeleted = x.IsDeleted,
            Status = x.Status,

            RuName = p.RuName,
            EnName = p.EnName,

            HaveSupplier = x.PartsSupplier != null,
            CatalogItemId = p.ID
            };

            if (checkStatus) {
                query = query.Where (x => x.Status == status);
            }

            return query;
        }
        public ItemForModeration GetModerationItem (int id) {
            return this.NeedModeration (false).FirstOrDefault (x => x.Id == id);
        }

    }

    public class ItemForModeration {
        public int Id { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProducerName { get; set; }
        public string ProducerCodeTrimmed { get; set; }
        public string ProducerCode { get; set; }
        public int? ProducerId { get; set; }
        public bool IsDeleted { get; set; }
        public PartStatusEnum Status { get; set; }
        public string RuName { get; set; }
        public string EnName { get; set; }
        public bool HaveSupplier { get; set; }
        public int? CatalogItemId { get; set; }
        public int? SeoParameterId { get; set; }
    }
}