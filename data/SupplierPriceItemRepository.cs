namespace depot {
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System;
    using Microsoft.EntityFrameworkCore;

    public interface ISupplierPriceItemRepository {
        Task<bool> AddRangeForSupplier (IEnumerable<SupplierPriceItem> items, PartsSupplier supplier);
        IEnumerable<object> GetByProducerCode (string producerCode, string producerName);
        Task TrimProducerCodes ();

        Task SetForModeration (IEnumerable<int> ids);

        IEnumerable<ModerationDto> GetModerationDto ();
        Task SetDeleted (int id, bool deleted);

        Task Update (ItemForModeration dto);
        IQueryable<object> GetMatchFor (string producerCode);
    }

    public class SupplierPriceItemRepository : ISupplierPriceItemRepository {

        private DepotContext _dbContext;
        private readonly IProducerCodeService _producerCodeService;

        public SupplierPriceItemRepository (DepotContext dbContext, IProducerCodeService producerCodeService) {
            this._dbContext = dbContext;
            this._producerCodeService = producerCodeService;
        }
        public async Task SetDeleted (int id, bool deleted) {
            var item = this._dbContext.SupplierPriceItems.Find (id);

            item.IsDeleted = deleted;

            this._dbContext.SupplierPriceItems.Update (item);
            await this._dbContext.SaveChangesAsync ();
        }
        public IQueryable<object> GetMatchFor (string producerCode) {
            var trimmed = _producerCodeService.TrimCode (producerCode);

            return _dbContext.SupplierPriceItems.Include (x => x.PartsSupplier)
                .Where (x => x.ProducerCodeTrimmed == producerCode)
                .Select (x => new {
                    Name = x.Name,
                        SupplierName = x.PartsSupplier.Name,
                        SupplierId = x.PartsSupplierId
                });
        }

        public async Task Update (ItemForModeration dto) {
            var item = this._dbContext.SupplierPriceItems.Find (dto.Id);

            dto.ProducerCodeTrimmed = _producerCodeService.TrimCode (dto.ProducerCode);

            item.UpdateBy (dto);

            this._dbContext.SupplierPriceItems.Update (item);
            await this._dbContext.SaveChangesAsync ();
        }

        public IEnumerable<ModerationDto> GetModerationDto () => this._dbContext.SupplierPriceItems.Select (x => new ModerationDto {
            Id = x.ID,
                ProducerCode = x.ProducerCodeTrimmed,
                ProducerId = x.ProducerId,
                ProducerCodeTrimmed = x.ProducerCodeTrimmed
        }).ToArray ();

        private void ResetModerationForAll () {
            _dbContext.Database.ExecuteSqlCommand ($"update depot.SupplierPriceItem set status = {(int)PartStatusEnum.Active};");
        }

        public async Task SetForModeration (IEnumerable<int> ids) {
            this.ResetModerationForAll ();

            var priceItems = _dbContext.SupplierPriceItems.Where (x => ids.ToArray ().Contains (x.ID) && x.Status != PartStatusEnum.OnModeration).ToList ();

            foreach (var item in priceItems) {
                item.Status = PartStatusEnum.OnModeration;
            }

            _dbContext.SupplierPriceItems.UpdateRange (priceItems);

            await _dbContext.SaveChangesAsync ();
        }

        public async Task<IEnumerable<SupplierPriceItem>> GetItemsForUpdate (int supplierId, List<string> articles, IEnumerable<SupplierPriceItem> source) {
            var producerGroups = await this.GetProducerGroups (supplierId);

            var list = new List<SupplierPriceItem> ();

            foreach (var group in producerGroups) {
                var dict = this.GetFormedDictionaryFrom (group);

                foreach (var article in articles) {
                    if (dict.ContainsKey (article)) {
                        list.Add (dict[article]);
                    }
                }
            }

            producerGroups = source.GroupBy (x => x.ProducerId);
            foreach (var group in producerGroups) {
                var dict = this.GetFormedDictionaryFrom (group);

                foreach (var listItem in list) {
                    if (dict.ContainsKey (listItem.ProducerCodeTrimmed)) {
                        listItem.UpdateBy (dict[listItem.ProducerCodeTrimmed]);
                        listItem.IsDeleted = false;
                    }
                }
            }

            return list;
        }

        public async Task<IEnumerable<SupplierPriceItem>> GetItemsForDelete (int supplierId, List<string> articles) {
            var producerGroups = await this.GetProducerGroups (supplierId);

            var list = new List<SupplierPriceItem> ();

            var dict = articles.ToDictionary (x => x, y => y);
            foreach (var group in producerGroups) {
                foreach (var groupItem in group) {
                    if (!dict.ContainsKey (groupItem.ProducerCodeTrimmed)) {
                        list.Add (groupItem);
                    }
                }
            }

            return list;
        }

        public IEnumerable<SupplierPriceItem> GetItemsForInsert (IEnumerable<SupplierPriceItem> source, IEnumerable<SupplierPriceItem> oldItemsForUpdate) {
            var articlesForUpdate = oldItemsForUpdate.Select (x => x.ProducerCodeTrimmed).ToDictionary (x => x, y => y);

            var result = source
                .Where (x => !articlesForUpdate.ContainsKey (x.ProducerCodeTrimmed));

            return result;
        }

        public async Task<IEnumerable<IGrouping<int?, SupplierPriceItem>> > GetProducerGroups (int supplierId) {
            var result = await this._dbContext.SupplierPriceItems.Where (x => x.PartsSupplierId == supplierId)
                .GroupBy (x => x.ProducerId)
                .ToListAsync ();
            return result;
        }
        public Dictionary<string, SupplierPriceItem> GetFormedDictionaryFrom (IGrouping<int?, SupplierPriceItem> group) {
            var dict = new Dictionary<string, SupplierPriceItem> ();

            foreach (var groupItem in group) {
                if (!dict.ContainsKey (groupItem.ProducerCodeTrimmed)) {
                    dict[groupItem.ProducerCodeTrimmed] = groupItem;
                }
            }

            return dict;

        }

        public async Task<bool> AddRangeForSupplier (IEnumerable<SupplierPriceItem> items, PartsSupplier supplier) {

            items = items.Where (x => !string.IsNullOrEmpty (x.ProducerCode));

            SetTrimmedForItems (items);

            items = items.Where (x => !string.IsNullOrEmpty (x.ProducerCodeTrimmed)).GroupBy (x => x.ProducerCodeTrimmed).Select (x => x.FirstOrDefault ());

            var articlesTrimmed = items.Select (x => x.ProducerCodeTrimmed).ToList ();

            // UPDATE
            var oldItemsForUpdate = await this.GetItemsForUpdate (supplier.ID, articlesTrimmed, items);
            if (oldItemsForUpdate.Any ()) {
                this._dbContext.SupplierPriceItems.UpdateRange (oldItemsForUpdate);
                await this._dbContext.SaveChangesAsync ();
            }

            // REMOVE
            var oldItemsForDelete = await this.GetItemsForDelete (supplier.ID, articlesTrimmed);

            if (oldItemsForDelete.Any ()) {
                foreach (var item in oldItemsForDelete) {
                    item.IsDeleted = true;
                }
                if (oldItemsForDelete.Any (x => x.ID == 62341)) {
                    var a = 1;
                }
                this._dbContext.SupplierPriceItems.UpdateRange (oldItemsForDelete);
                await this._dbContext.SaveChangesAsync ();
            }

            // INSERT
            var insertItems = this.GetItemsForInsert (items, oldItemsForUpdate)
                .Where (x => x.Producer != null && x.PartsSupplier != null);

            if (insertItems.Any ()) {
                foreach (var item in insertItems) {
                    item.SetUploadedAt ();

                    item.PartsSupplierId = item.PartsSupplier.ID;
                    item.ProducerId = item.Producer.ID;
                }

                await this._dbContext.SupplierPriceItems.AddRangeAsync (insertItems);
                await this._dbContext.SaveChangesAsync ();
            }

            return true;
        }

        public async Task TrimProducerCodes () {
            var items = await this._dbContext.SupplierPriceItems.ToListAsync ();

            // _dbContext.Database.ExecuteSqlCommand ("SET unique_checks=0;");

            SetTrimmedForItems (items);
            this._dbContext.UpdateRange (items);
            await this._dbContext.SaveChangesAsync ();

            // _dbContext.Database.ExecuteSqlCommand ("SET unique_checks=1;");
        }

        public IEnumerable<object> GetByProducerCode (string producerCode, string producerName) {

            var trimmed = _producerCodeService.TrimCode (producerCode);

            var items = this._dbContext.SupplierPriceItems
                .Where (x => x.ProducerCodeTrimmed == trimmed &&
                    (string.IsNullOrEmpty (producerName) || EF.Functions.Like (x.ProducerName, $"{producerName}%")) && !x.IsDeleted)
                .Where (x => x.Status != PartStatusEnum.Blocked);

            items.Include (x => x.PartsSupplier)
                .ThenInclude (x => x.Logo);

            items.Include (x => x.PartsSupplier)
                .ThenInclude (x => x.Contacts);

            items.Include (x => x.PartsSupplier)
                .ThenInclude (x => x.SalePoints);
            items.Include (x => x.PartsSupplier)
                .ThenInclude (x => x.SalePoints)
                .ThenInclude (x => x.Address);

            return items.Select (x => new {
                producer = new {
                        producerId = x.ProducerId,
                    },
                    name = x.Name,
                    producerCode = x.ProducerCodeTrimmed,
                    supplier = new {
                        searchName = x.PartsSupplier.SearchName,
                            logoName = x.PartsSupplier.Logo.Name,
                            deliveryMethods = x.PartsSupplier.SalePoints.Select (sp => sp.DeliveryMethod),
                            addresses = x.PartsSupplier.SalePoints.Select (sp => new {
                                address = sp.Address,
                                    phone = sp.Phone
                            }),
                            prices = new {
                                priceRu = x.Price,
                                    priceUsd = x.PriceUsd,
                                    priceEu = x.PriceEu
                            }

                    },
                    count = x.Count,
                    updatedAt = x.UpdatedAt,
                    createdAt = x.CreatedAt,
                    freshOfPrice = x.LastUploadedUpdated
            });
        }
        private void SetTrimmedForItems (IEnumerable<SupplierPriceItem> items) {
            foreach (var item in items) {
                item.ProducerCodeTrimmed = this._producerCodeService.TrimCode (item.ProducerCode);
            }
        }
    }
}