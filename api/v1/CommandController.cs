using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Google.Apis.Discovery;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace depot {

    [Route ("api/v1/[controller]")]
    public class CommandController : Controller {

        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly IPartsRepository _partsRepository;
        private readonly ISupplierPriceItemRepository _supplierPriceItemRepository;
        private readonly IPartSupplierRepository _partSupplierRepository;
        private readonly IPartProducerRepository _partProducerRepository;

        private readonly IParserService _enbsvParserService;
        private readonly IParserService _gazServiceParserService;

        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly IGoogleDriveService _googleDriveService;

        private readonly ISchedulerWrapper _scheduler;
        private readonly IModerationService _moderationService;
        public CommandController (
            EnbsvParserService enbsvParserService,
            GazServiceParserService gazServiceParserService,
            IPartsRepository partsRepository,
            IPartProducerRepository partProducerRepository,
            ICatalogItemRepository catalogItemRepository,
            IPartSupplierRepository partSupplierRepository,
            ISupplierPriceItemRepository supplierPriceItemRepository,
            IModerationService moderationService,
            IHostingEnvironment hostingEnvironment,
            ISchedulerWrapper scheduler,
            IGoogleDriveService googleDriveService) {

            this._scheduler = scheduler;
            this._googleDriveService = googleDriveService;
            this._moderationService = moderationService;
            this._gazServiceParserService = gazServiceParserService;
            this._enbsvParserService = enbsvParserService;
            this._partSupplierRepository = partSupplierRepository;
            this._partsRepository = partsRepository;
            this._partProducerRepository = partProducerRepository;
            this._hostingEnvironment = hostingEnvironment;
            this._catalogItemRepository = catalogItemRepository;
            this._supplierPriceItemRepository = supplierPriceItemRepository;
        }

        // GET: api/values
        [HttpGet]
        [Route (template: "test")]
        public IActionResult Get () {
            return Ok (_partsRepository.All);;
        }

        [HttpGet]
        [Route (template: "gd-list")]
        public IActionResult GetList () {
            var result = this._googleDriveService.ListOfFiles ();
            return Ok (result);
        }

        // GET: api/values
        [HttpGet]
        [Route (template: "trim-producer-codes")]
        public async Task<IActionResult> TrimProducerCodes () {
            await this._supplierPriceItemRepository.TrimProducerCodes ();
            await this._catalogItemRepository.TrimProducerCodes ();

            return Ok (true);;
        }

        [HttpGet]
        [Route (template: "check-for-moderation")]
        public async Task<IActionResult> CheckForModeration () {
            await _moderationService.CheckModeration ();
            return Ok (true);
        }

        [HttpGet]
        [Route (template: "import-gaz-service-offers")]
        public async Task<IActionResult> ImportGazServiceOffers () {
            await this._gazServiceParserService.Run ("gaz-service");
            return Ok (true);
        }

        [HttpGet]
        [Route ("import-gaz-service-parameters")]
        public async Task<IActionResult> ImportGazServiceParameters () {
            await this._gazServiceParserService.ImportParameters ("gaz-service");
            return Ok (true);
        }

        [HttpGet]
        [Route (template: "import-enbsv-parameters")]
        public async Task<IActionResult> ImportEnbsvParameters () {
            await this._enbsvParserService.ImportParameters ("energobyt-service");
            return Ok (true);
        }

        [HttpGet]
        [Route (template: "run-scheduler")]
        public async Task<IActionResult> RunScheduler () {
            await this._scheduler.Run ();
            return Ok (true);
        }

        [HttpGet]
        [Route (template: "import-enbsv-offers")]
        public async Task<IActionResult> ImportEnbsvOffers () {
            await this._enbsvParserService.Run ("energobyt-service");
            return Ok (true);
        }

        [HttpGet]

        [Route ("download-catalogs")]
        public async Task<IActionResult> DownloadCatalogsAsync () {
            var files = new List<FileModel> {
                new FileModel ("Bosch", "01_Прайс_лист_Бош_Термотехника_08.xlsx"),
                new FileModel ("Beretta", "02 BERETTA прайс.xlsx"),
                new FileModel ("Wolf", "03_2017_03_20_WOLF_прайс_лист_на.xlsx"),
                new FileModel ("Viessmann", "04 Viessmann от 01.03.2017.xlsx"),
                new FileModel ("Buderus", "05 BUDERUS запчасти.xlsx"),
                new FileModel ("Navien", "06 01042017 Navien.xlsx"),
                new FileModel ("Thermona", "07 THERMONA прайс запчасти.xlsx"),
                new FileModel ("Giersch", "08 GIERSCH прайс запчасти.xlsx"),
                new FileModel ("ACV", "09 ACV прайс запчасти.xlsx"),
                new FileModel ("ACV", "10 ACV цены запчасти Alfa Comfort.xlsx"),
                new FileModel ("Rinnai", "15__АСЦ_прайс_запчасти_Rinnai_RUS.xlsx"),
                new FileModel ("Baxi", "baxi.xlsx"),
                new FileModel ("Protherm", "Protherm.xlsx"),
                new FileModel ("Vaillant", "vaillant.xlsx")
            };

            try {
                string sWebRootFolder = _hostingEnvironment.WebRootPath + "./../files/catalogs/";
                foreach (var fileModel in files) {
                    var catalogs = new Collection<ProducerCatalogItem> ();
                    var fileName = fileModel.FileName;

                    FileInfo file = new FileInfo (Path.Combine (sWebRootFolder, fileName));

                    var producer = await _partProducerRepository.CreateProducerIfNotExist (fileModel.ProducerName);

                    using (ExcelPackage package = new ExcelPackage (file)) {
                        if (package.Workbook != null && package.Workbook.Worksheets != null) {
                            foreach (var worksheet in package.Workbook.Worksheets) {
                                if (worksheet != null) {
                                    int rowCount = worksheet.Dimension != null ? worksheet.Dimension.Rows : 0;
                                    int ColCount = worksheet.Dimension != null ? worksheet.Dimension.Columns : 0;

                                    for (int row = 2; row <= rowCount; row++) {
                                        //if (!string.IsNullOrEmpty (worksheet.Cells[row, 1].Value.ToString ())) {
                                        var catalogItem = new ProducerCatalogItem ();
                                        catalogItem.Producer = producer;

                                        for (int col = 1; col <= ColCount; col++) {
                                            switch (col) {
                                                case 1:
                                                    {
                                                        catalogItem.ProducerCode = worksheet.Cells[row, col]?.Value?.ToString ().Trim ();
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        catalogItem.RuName = worksheet.Cells[row, col]?.Value?.ToString ().Trim ();
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        catalogItem.EnName = worksheet.Cells[row, col]?.Value?.ToString ().Trim ();
                                                        break;
                                                    }
                                            }
                                        }
                                        catalogs.Add (catalogItem);
                                        //}

                                    }
                                }
                            }
                        }
                    }

                    await this._catalogItemRepository.AddRangeForProducer (catalogs, producer);
                }
            } catch (Exception ex) {
                return Ok (ex.Message + "\n" + ex.StackTrace);
            }

            return Ok (true);
        }

        [HttpGet]

        [Route ("download-items-viessmann")]
        public async Task<IActionResult> DownloadItemsViessmann () {

            var files = new List<FileModel> {
                new FileModel ("Viessmann", "viessmann/Viessmann.xlsx"),
            };

            try {
                string sWebRootFolder = _hostingEnvironment.WebRootPath + "./../files/offers_of_suppliers/";
                foreach (var fileModel in files) {
                    var prices = new Collection<SupplierPriceItem> ();
                    var fileName = fileModel.FileName;

                    FileInfo file = new FileInfo (Path.Combine (sWebRootFolder, fileName));

                    var supplierModel = new PartsSupplier () {
                        Name = fileModel.ProducerName,
                        SearchName = fileModel.ProducerName,
                        Logo = new ModelFile {
                        Name = "logo.png"
                        }
                    };
                    var supplier = await _partSupplierRepository.CreateSupplierIfNotExist (supplierModel);

                    using (ExcelPackage package = new ExcelPackage (file)) {
                        if (package.Workbook != null && package.Workbook.Worksheets != null) {
                            foreach (var worksheet in package.Workbook.Worksheets) {
                                if (worksheet != null) {
                                    int rowCount = worksheet.Dimension != null ? worksheet.Dimension.Rows : 0;
                                    int ColCount = worksheet.Dimension != null ? worksheet.Dimension.Columns : 0;

                                    // return Ok ($"{package.Workbook.Worksheets.Count} - {rowCount} - {ColCount}");
                                    for (int row = 2; row <= rowCount; row++) {
                                        //if (!string.IsNullOrEmpty (worksheet.Cells[row, 1].Value.ToString ())) {
                                        var price = new SupplierPriceItem ();
                                        price.PartsSupplier = supplier;

                                        for (int col = 1; col <= ColCount; col++) {
                                            var value = worksheet.Cells[row, col]?.Value?.ToString ().Trim ();;
                                            switch (col) {
                                                case 1:
                                                    {
                                                        price.ProducerName = value;
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        price.ProducerCode = value;
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        price.Name = value;
                                                        break;
                                                    }
                                                case 4:
                                                    {
                                                        int count = 0;
                                                        int.TryParse (value, result : out count);
                                                        price.Count = count;
                                                        break;
                                                    }
                                                case 5:
                                                    {
                                                        decimal priceValie = 0;
                                                        decimal.TryParse (value, result : out priceValie);
                                                        price.Price = priceValie;
                                                        break;
                                                    }
                                                case 6:
                                                    {
                                                        price.Description = value;
                                                        break;
                                                    }
                                            }
                                        }
                                        prices.Add (price);
                                        //}

                                    }
                                }
                            }
                        }
                    }

                    var groups = prices.GroupBy (x => x.ProducerName);
                    foreach (IGrouping<string, SupplierPriceItem> group in groups) {

                        var producerName = group.Key;
                        if (!string.IsNullOrEmpty (producerName)) {
                            var producer = await this._partProducerRepository.CreateProducerIfNotExist (producerName);

                            foreach (var priceItem in group) {
                                priceItem.Producer = producer;
                            }

                            await this._supplierPriceItemRepository.AddRangeForSupplier (group, supplier);
                        }
                    }
                }
            } catch (Exception ex) {
                return Ok (ex.Message + "\n" + ex.StackTrace);
            }

            return Ok (true);
        }

    }

}