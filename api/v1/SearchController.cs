using Microsoft.AspNetCore.Mvc;

namespace depot {

    [Route ("api/v1/search")]
    public class SearchController : Controller {

        [HttpGet]
        [Route ("in-producers")]
        public IActionResult SearchInProducers (string code, string producerName) {
            var fromProducers = _catalogItemRepository.GetByProducerCode (code, producerName);
            return Ok (fromProducers);
        }

        [HttpGet]
        [Route ("in-suppliers")]
        public IActionResult SearchInSuppliers (string code, string producerName) {
            var fromSuppliers = _supplierPriceItemRepository.GetByProducerCode (code, producerName);
            return Ok (fromSuppliers);
        }

        [HttpGet]
        [Route ("supplier")]
        public IActionResult SearchSupplier (string name) {
            var supplier = this._partSupplierRepository.GetByName (name);
            return Ok (supplier);
        }

        private ICatalogItemRepository _catalogItemRepository;
        private ISupplierPriceItemRepository _supplierPriceItemRepository;
        private IPartSupplierRepository _partSupplierRepository;

        public SearchController (ICatalogItemRepository catalogItemRepository,
            ISupplierPriceItemRepository supplierPriceItemRepository,
            IPartSupplierRepository partSupplierRepository) {

            this._partSupplierRepository = partSupplierRepository;
            this._catalogItemRepository = catalogItemRepository;
            this._supplierPriceItemRepository = supplierPriceItemRepository;
        }
    }
}