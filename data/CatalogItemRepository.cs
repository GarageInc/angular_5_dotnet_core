namespace depot {
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;

    public interface ICatalogItemRepository {
        Task<bool> AddRangeForProducer (IEnumerable<ProducerCatalogItem> items, PartProducer producer);
        IQueryable<object> GetByProducerCode (string producerCode, string producerName);
        IQueryable<ProducerCatalogItem> GetMatch (string producerCode, string producerName);

        Task TrimProducerCodes ();

        IEnumerable<ModerationDto> GetModerationDto ();

        Task CreateCatalogItem (ItemForModeration item);
        Task Update (ItemForModeration model);
        Task UpdateRange (List<ProducerCatalogItem> items);
    }

    public class CatalogItemRepository : ICatalogItemRepository {

        private DepotContext _dbContext;
        private readonly IProducerCodeService _producerCodeService;
        public CatalogItemRepository (DepotContext dbContext, IProducerCodeService producerCodeService) {
            this._dbContext = dbContext;
            this._producerCodeService = producerCodeService;
        }

        public async Task UpdateRange (List<ProducerCatalogItem> items) {
            this._dbContext.UpdateRange (items);
            await this._dbContext.SaveChangesAsync ();
        }
        public async Task CreateCatalogItem (ItemForModeration item) {
            var newCatalogItem = new ProducerCatalogItem {
                EnName = item.EnName,
                RuName = item.RuName,
                ProducerId = item.ProducerId,
                ProducerCode = item.ProducerCode,
                ProducerCodeTrimmed = _producerCodeService.TrimCode (item.ProducerCode),
                SeoParameterId = item.SeoParameterId
            };

            await _dbContext.AddAsync (newCatalogItem);

            await _dbContext.SaveChangesAsync ();
        }

        public async Task Update (ItemForModeration dto) {
            if (dto.CatalogItemId > 0) {

                var item = this._dbContext.ProducerCatalogItems.Find (dto.CatalogItemId);

                dto.ProducerCodeTrimmed = _producerCodeService.TrimCode (dto.ProducerCode);

                item.UpdateBy (dto);

                this._dbContext.ProducerCatalogItems.Update (item);
                await this._dbContext.SaveChangesAsync ();
            }
        }

        public IEnumerable<ModerationDto> GetModerationDto () => this._dbContext.ProducerCatalogItems.Select (x => new ModerationDto {
            ProducerCode = x.ProducerCodeTrimmed,
                ProducerId = x.ProducerId,
                ProducerCodeTrimmed = x.ProducerCodeTrimmed
        }).ToArray ();

        public async Task<bool> AddRangeForProducer (IEnumerable<ProducerCatalogItem> items, PartProducer producer) {

            items = items.Where (x => !string.IsNullOrEmpty (x.ProducerCode));
            this.SetTrimmedForItems (items);

            items = items.Where (x => !string.IsNullOrEmpty (x.ProducerCodeTrimmed))
                .GroupBy (x => x.ProducerCodeTrimmed).Select (x => x.FirstOrDefault ())
                .Where (x => x != null && x.ProducerCode != null && x.Producer != null).ToList ();

            var articlesTrimmed = items.Select (x => x.ProducerCode).ToList ();

            var oldItems = this._dbContext.ProducerCatalogItems.Where (x => x.ProducerId == producer.ID && articlesTrimmed.Contains (x.ProducerCodeTrimmed));

            this._dbContext.ProducerCatalogItems.RemoveRange (oldItems);
            await this._dbContext.SaveChangesAsync ();

            await this._dbContext.ProducerCatalogItems.AddRangeAsync (items);
            await this._dbContext.SaveChangesAsync ();

            return true;
        }

        public async Task TrimProducerCodes () {
            var items = this._dbContext.ProducerCatalogItems;

            SetTrimmedForItems (items);

            this._dbContext.UpdateRange (items);
            await this._dbContext.SaveChangesAsync ();
        }

        public IQueryable<ProducerCatalogItem> GetMatch (string producerCode, string producerName) {
            var query = this._dbContext.ProducerCatalogItems
                .Include (x => x.Producer)
                .Where (x => EF.Functions.Like (x.ProducerCodeTrimmed, $"%{producerCode}%"));

            if (!(string.IsNullOrEmpty (producerName))) {
                query = query.Where (x => x.Producer.Name.StartsWith (producerName));
            }

            return query;
        }

        public IQueryable<object> GetByProducerCode (string producerCode, string producerName) {
            producerCode = this._producerCodeService.TrimCode (producerCode);

            var items = this.GetMatch (producerCode, producerName)
                .Select (x => new {
                    producer = x.Producer,
                        producerCode = x.ProducerCode,
                        producerCodeTrimmed = x.ProducerCodeTrimmed,
                        producerId = x.ProducerId,
                        ruName = x.RuName,
                        enName = x.EnName,
                        seoParameterId = x.SeoParameterId
                });

            return items;
        }

        private void SetTrimmedForItems (IEnumerable<ProducerCatalogItem> items) {
            foreach (var item in items) {
                item.ProducerCodeTrimmed = this._producerCodeService.TrimCode (item.ProducerCode);
            }
        }
    }
}