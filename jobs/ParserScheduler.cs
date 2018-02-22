using System.Threading.Tasks;

namespace depot {

    public interface ISchedulerWrapper {
        Task Run ();
    }

    public class SchedulerWrapper : ISchedulerWrapper {

        public readonly AbstractParserService _enbsvParser;
        public readonly AbstractParserService _gazServiceParser;

        public readonly IPartSupplierRepository _partSupplierRepo;

        public readonly ISupplierOfferFilesRepository _supplierOfferFilesRepo;

        public SchedulerWrapper (EnbsvParserService enbsvParser, GazServiceParserService gazServiceParser, IPartSupplierRepository partSupplierRepo, ISupplierOfferFilesRepository supplierOfferFilesRepo) {
            this._enbsvParser = enbsvParser;
            this._supplierOfferFilesRepo = supplierOfferFilesRepo;
            this._gazServiceParser = gazServiceParser;
            this._partSupplierRepo = partSupplierRepo;
        }

        public async Task Run () {

            var suppliers = this._partSupplierRepo.GetAll ();

            foreach (var supplier in suppliers) {
                switch (supplier.SearchName) {
                    case "energobyt-service":
                        {
                            await this._enbsvParser.ImportOffers (supplier.SearchName, _partSupplierRepo, _supplierOfferFilesRepo);
                            break;
                        }
                    default:
                        {
                            await this._gazServiceParser.ImportOffers (supplier.SearchName, _partSupplierRepo, _supplierOfferFilesRepo);
                            break;
                        }
                }
            }

        }
    }
}