using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

namespace depot {

    public class GazServiceParserService : AbstractParserService {
        private readonly IPartSupplierRepository _partSupplierRepository;
        private readonly ISupplierWarehouseRepository _supplierWarehouseRepository;
        private readonly IPartProducerRepository _partProducerRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISupplierOfferFilesRepository _supplierOfferFilesRepository;
        private readonly ISupplierPriceItemRepository _supplierPriceItemRepository;

        public GazServiceParserService (
            IPartSupplierRepository partSupplierRepository,
            ISupplierWarehouseRepository supplierWarehouseRepository,
            IPartProducerRepository partProducerRepository,
            ISupplierPriceItemRepository supplierPriceItemRepository,
            ISupplierOfferFilesRepository supplierOfferFilesRepository,
            IHostingEnvironment hostingEnvironment) {

            this._supplierOfferFilesRepository = supplierOfferFilesRepository;
            this._hostingEnvironment = hostingEnvironment;
            this._supplierPriceItemRepository = supplierPriceItemRepository;
            this._supplierWarehouseRepository = supplierWarehouseRepository;
            this._partSupplierRepository = partSupplierRepository;
            this._partProducerRepository = partProducerRepository;
        }

        public override async Task Run (string SERVICE_SEARCH_NAME) {
            await this.ImportOffers (SERVICE_SEARCH_NAME, this._partSupplierRepository, this._supplierOfferFilesRepository);
        }

        public override async Task Process (IEnumerable<SupplierOfferFile> files, PartsSupplier supplier) {
            var file = files.First ();

            var fileModel = new FileModel (supplier.SearchName, "All", file.Name);
            var priceList = new Collection<SupplierPriceItem> ();
            FileInfo fileInfo = new FileInfo (GetPathToFile (_hostingEnvironment, file, supplier.SearchName));;

            using (ExcelPackage package = new ExcelPackage (fileInfo)) {
                if (package.Workbook != null && package.Workbook.Worksheets != null) {
                    foreach (var worksheet in package.Workbook.Worksheets) {
                        if (worksheet != null) {
                            int rowCount = worksheet.Dimension != null ? worksheet.Dimension.Rows : 0;
                            int ColCount = worksheet.Dimension != null ? worksheet.Dimension.Columns : 0;

                            for (int row = 5; row <= rowCount; row++) {
                                var priceItem = new SupplierPriceItem ();
                                priceItem.PartsSupplier = supplier;
                                priceItem.ProducerName = fileModel.ProducerName;

                                for (int col = 2; col <= ColCount; col++) {
                                    var value = worksheet.Cells[row, col]?.Value?.ToString ().Trim ();
                                    if (value != null) {

                                        switch (col) {
                                            case 2:
                                                {
                                                    priceItem.ProducerCode = value;
                                                    break;
                                                }
                                            case 3:
                                                {

                                                    priceItem.ProducerName = value;
                                                    break;
                                                }
                                            case 4:
                                                {
                                                    priceItem.Name = value;
                                                    break;
                                                }
                                            case 6:
                                                {
                                                    decimal result = 0;
                                                    decimal.TryParse (value, out result);
                                                    priceItem.PriceEu = result;
                                                    break;
                                                }
                                            case 8:
                                                {
                                                    decimal result = 0;
                                                    decimal.TryParse (value, out result);
                                                    priceItem.Price = result;
                                                    break;
                                                }
                                            case 10:
                                                {
                                                    var result = 0;
                                                    int.TryParse (value, out result);
                                                    priceItem.Count = result;
                                                    break;
                                                }
                                        }
                                    }
                                }

                                priceList.Add (priceItem);
                            }
                        }
                    }
                }

                var groupedByProducer = priceList.GroupBy (x => x.ProducerName);

                foreach (var group in groupedByProducer) {
                    var producer = await this._partProducerRepository.CreateProducerIfNotExist (group.Key);

                    foreach (var priceListItem in group) {
                        priceListItem.Producer = producer;
                    }
                }
            }

            await this._supplierPriceItemRepository.AddRangeForSupplier (priceList, supplier);
        }

        /**
                public override async Task<string> ImportOffers () {
                    var files = new List<FileModel> {
                        new FileModel (SERVICE_SEARCH_NAME, "Protherm", "Protherm.xlsx"),
                        new FileModel (SERVICE_SEARCH_NAME, "Vaillant", "Vaillant.xlsx"),
                        new FileModel (SERVICE_SEARCH_NAME, "Ariston", "Ariston.xlsx"),
                        new FileModel (SERVICE_SEARCH_NAME, "Baxi", "Baxi.xlsx"),
                    };

                    var supplier = _partSupplierRepository.GetByName (SERVICE_SEARCH_NAME);

                    try {

                        string sWebRootFolder = _hostingEnvironment.WebRootPath + "./../files/offers_of_suppliers/" + SERVICE_SEARCH_NAME + "/";
                        foreach (var fileModel in files) {
                            var priceList = new Collection<SupplierPriceItem> ();
                            var fileName = fileModel.FileName;

                            FileInfo file = new FileInfo (Path.Combine (sWebRootFolder, fileName));;

                            var producer = await this._partProducerRepository.CreateProducerIfNotExist (fileModel.ProducerName);

                            using (ExcelPackage package = new ExcelPackage (file)) {
                                if (package.Workbook != null && package.Workbook.Worksheets != null) {
                                    foreach (var worksheet in package.Workbook.Worksheets) {
                                        if (worksheet != null) {
                                            int rowCount = worksheet.Dimension != null ? worksheet.Dimension.Rows : 0;
                                            int ColCount = worksheet.Dimension != null ? worksheet.Dimension.Columns : 0;

                                            for (int row = 1; row <= rowCount; row++) {
                                                //if (!string.IsNullOrEmpty (worksheet.Cells[row, 1].Value.ToString ())) {
                                                var priceItem = new SupplierPriceItem ();
                                                priceItem.PartsSupplier = supplier;
                                                priceItem.Producer = producer;
                                                priceItem.ProducerName = fileModel.ProducerName;

                                                for (int col = 1; col <= ColCount; col++) {
                                                    var value = worksheet.Cells[row, col]?.Value?.ToString ().Trim ();
                                                    if (value != null) {

                                                        switch (col) {
                                                            case 1:
                                                                {
                                                                    priceItem.ProducerCode = value;
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    priceItem.Name = value;
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    decimal result = 0;
                                                                    decimal.TryParse (value, out result);
                                                                    priceItem.PriceEu = result;
                                                                    break;
                                                                }
                                                            case 4:
                                                                {
                                                                    var result = 0;
                                                                    int.TryParse (value, out result);
                                                                    priceItem.Count = result;
                                                                    break;
                                                                }
                                                        }
                                                    }
                                                }

                                                priceList.Add (priceItem);
                                                //}

                                            }
                                        }
                                    }
                                }
                            }

                            await this._supplierPriceItemRepository.AddRangeForSupplier (priceList, supplier);
                        }
                    } catch (Exception ex) {
                        return (ex.Message + "\n" + ex.InnerException.Message + "\n" + ex.InnerException.StackTrace);
                    }

                    return string.Empty;

                }
         */
        public override async Task ImportParameters (string SERVICE_SEARCH_NAME) {
            var energobyt = new PartsSupplier {
                Name = "«ГазСервис» ИП Попов И.В.",
                SearchName = SERVICE_SEARCH_NAME,
                Email = "r47478@mail.ru",
                Contacts = new List<SupplierContact> {
                new SupplierContact {
                Phone = "74739621917"
                },
                new SupplierContact {
                Phone = "47478"
                },
                new SupplierContact {
                Phone = "79204394447"
                },
                new SupplierContact {
                Phone = "79529555478"
                },
                },
                SalePoints = new List<SalePoint> {
                new SalePoint {
                Name = "«ГазСервис» ИП Попов И.В.",
                Phone = "74739621917",
                Address = new Address {
                Locality = "г.Россошь",
                Location = "Воронежская область, ул.Пролетарская, д.33"
                },
                TimeWorks = new List<TimeWork> {
                new TimeWork {
                Description = "Пн-Пт: с 8.30 до 17.30"
                },
                new TimeWork {
                Description = "Сб-Вс: с 9.00 до 14.00"
                }
                },
                DeliveryMethod = "Cамовывоз"
                }
                },
                Logo = new ModelFile {
                Name = "logo.png"
                }
            };

            energobyt = await _partSupplierRepository.CreateSupplierIfNotExist (energobyt);
        }
    }

}