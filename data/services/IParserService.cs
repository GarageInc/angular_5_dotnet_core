using System.Collections.Generic;
using System.Threading.Tasks;

namespace depot {
    public interface IParserService {
        Task ImportParameters (string SERVICE_SEARCH_NAME);
        Task ImportOffers (string SERVICE_SEARCH_NAME, IPartSupplierRepository _partSupplierRepository, ISupplierOfferFilesRepository _supplierOfferFilesRepository);

        Task Process (IEnumerable<SupplierOfferFile> files, PartsSupplier supplier);
        Task Run (string SERVICE_SEARCH_NAME);
    }

}