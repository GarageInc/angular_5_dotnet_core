using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace depot {

    public interface IModerationService {
        IQueryable<object> NeedModeration (ModerationFilterModel filter);
        Task CheckModeration ();
        Task Delete (int id);
        Task Update (ItemForModeration model);
        Task SaveFileFor (string producerCode, int producerId, IFormFile file);
        Task CreateCatalogItem (ItemForModeration model);
        ItemForModeration GetModerationItem (int id);
        IQueryable<object> SuppliersMatchForAsync (string producerCode);
        Task Restore (int id);
    }

    public class ModerationService : IModerationService {
        public readonly IProducerCodeService _producerCodeService;
        public readonly IModerationRepository _moderationRepository;

        public readonly ISupplierPriceItemRepository _supplierPriceItemRepository;
        public readonly ICatalogItemRepository _catalogItemRepository;
        public readonly IHostingEnvironment _hostingEnvironment;

        public readonly IPartProducerRepository _partProducerRepository;

        public ModerationService (IProducerCodeService producerCodeService,
            IPartProducerRepository partProducerRepository,
            IModerationRepository moderationRepository,
            ISupplierPriceItemRepository supplierPriceItemRepository,
            ICatalogItemRepository catalogItemRepository,
            IHostingEnvironment hostingEnvironment) {
            this._catalogItemRepository = catalogItemRepository;
            this._producerCodeService = producerCodeService;
            this._moderationRepository = moderationRepository;
            this._supplierPriceItemRepository = supplierPriceItemRepository;
            this._hostingEnvironment = hostingEnvironment;
            this._partProducerRepository = partProducerRepository;
        }

        public ItemForModeration GetModerationItem (int id) {
            return this._moderationRepository.GetModerationItem (id);
        }

        public async Task CreateCatalogItem (ItemForModeration model) {
            await this._catalogItemRepository.CreateCatalogItem (model);
        }

        public async Task SaveFileFor (string producerCode, int producerId, IFormFile formFile) {
            var producer = this._partProducerRepository.GetById (producerId);

            // full path to file in temp location
            var filePath = this._hostingEnvironment.WebRootPath + $"/../files/producers/items/{producerId}";

            if (!Directory.Exists (filePath))
                Directory.CreateDirectory (filePath);

            filePath = $"{filePath}/{producerCode.Trim()}.jpg";

            if (formFile.Length > 0) {
                using (var stream = new FileStream (filePath, FileMode.Create)) {
                    await formFile.CopyToAsync (stream);
                }
            }

        }

        public IQueryable<object> NeedModeration (ModerationFilterModel model) {
            var query = this._moderationRepository.NeedModeration (true, model != null ? model.Status : PartStatusEnum.OnModeration);

            if (model != null) {
                query = query.Where (y => string.IsNullOrEmpty (model.EnName) || EF.Functions.Like (y.EnName, $"%{model.EnName}%"));
                query = query.Where (y => string.IsNullOrEmpty (model.RuName) || EF.Functions.Like (y.RuName, $"%{model.RuName}%"));
                query = query.Where (y => string.IsNullOrEmpty (model.ProducerCode) || EF.Functions.Like (y.ProducerCode, $"%{model.ProducerCode}%"));
                query = query.Where (y => string.IsNullOrEmpty (model.ProducerName) || EF.Functions.Like (y.ProducerName, $"%{model.ProducerName}%"));
                query = query.Where (y => string.IsNullOrEmpty (model.SupplierName) || EF.Functions.Like (y.SupplierName, $"%{model.SupplierName}%"));

                query = query.Skip (model.Offset).Take (model.Count);
            }

            return query;
        }

        public async Task CheckModeration () {
            var allPriceItems = this._supplierPriceItemRepository.GetModerationDto ();
            var allCatalogItems = this._catalogItemRepository.GetModerationDto ();

            var except = allPriceItems.Except (allCatalogItems, new ModerationDtoComparer (_producerCodeService)).Select (x => x.Id);

            await _supplierPriceItemRepository.SetForModeration (except);
        }

        public async Task Delete (int id) {
            await this._supplierPriceItemRepository.SetDeleted (id, true);
        }

        public async Task Restore (int id) {
            await this._supplierPriceItemRepository.SetDeleted (id, false);
        }
        public async Task Update (ItemForModeration model) {
            await this._supplierPriceItemRepository.Update (model);
            await this._catalogItemRepository.Update (model);
        }

        public IQueryable<object> SuppliersMatchForAsync (string producerCode) {
            return this._supplierPriceItemRepository.GetMatchFor (producerCode);
        }
    }

}