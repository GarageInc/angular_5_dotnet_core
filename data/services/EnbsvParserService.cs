using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;

namespace depot {
    public class EnbsvOffers {
        public string Id { get; set; }
        public string ProducerCode { get; set; }

        public decimal PriceRu { get; set; }
        public decimal PriceUsd { get; set; }
        public decimal PriceEu { get; set; }

        public int Count { get; set; }
    }

    public class EnbsvImport {
        public string Id { get; set; }
        public string ProducerName { get; set; }
        public string Name { get; set; }

        public string ProducerCode { get; set; }
    }

    public class EnbsvParserService : AbstractParserService {
        public override async Task Run (string SERVICE_SEARCH_NAME) {
            await this.ImportOffers (SERVICE_SEARCH_NAME, this._partSupplierRepository, this._supplierOfferFilesRepository);
        }

        public async override Task ImportOffers (string SERVICE_SEARCH_NAME, IPartSupplierRepository _partSupplierRepository, ISupplierOfferFilesRepository _supplierOfferFilesRepository) {

            var supplier = _partSupplierRepository.GetByName (SERVICE_SEARCH_NAME);
            var files = _supplierOfferFilesRepository.NeedProcessingFor (supplier).ToList ();

            var groupedFiles = files.GroupBy (x => x.GroupIdentifier);

            foreach (var group in groupedFiles) {

                var errorMessage = string.Empty;
                OfferFileStatusEnum status;

                try {
                    await this.Process (group, supplier);
                    status = OfferFileStatusEnum.Processed;
                } catch (Exception ex) {
                    status = OfferFileStatusEnum.Error;
                    errorMessage = ex.Message + ex.StackTrace;
                }

                foreach (var file in group) {
                    file.Status = status;
                    file.ErrorMessage = errorMessage;
                }

                await _supplierOfferFilesRepository.SaveRange (group);
            }
        }

        public override async Task Process (IEnumerable<SupplierOfferFile> files, PartsSupplier supplier) {
            // OFFERS #1

            var offerFile = files.First (x => x.Name == "offers0_1.xml");
            var filePath = GetPathToFile (_hostingEnvironment, offerFile, supplier.SearchName);

            var xmlDoc = new XmlDocument ();
            xmlDoc.Load (filePath);

            var offersTag = "Предложение";
            var offersNodes = xmlDoc.GetElementsByTagName (offersTag);
            var offers = new List<EnbsvOffers> ();
            foreach (XmlNode node in offersNodes) {
                var offer = new EnbsvOffers ();

                offer.Id = node["Ид"].InnerText;
                offer.ProducerCode = node["Артикул"].InnerText;
                foreach (XmlNode infoNode in node.ChildNodes) {
                    string value = infoNode.InnerText;

                    switch (infoNode.Name) {
                        case "Цены":
                            {
                                var priceNodes = infoNode.ChildNodes;

                                foreach (XmlNode priceNode in priceNodes) {
                                    var price = decimal.Parse (priceNode["ЦенаЗаЕдиницу"].InnerText);
                                    switch (priceNode["Валюта"].InnerText) {
                                        case "EUR":
                                            {
                                                offer.PriceEu = price;
                                                break;
                                            }
                                        case "USD":
                                            {
                                                offer.PriceUsd = price;
                                                break;
                                            }
                                        case "руб":
                                            {
                                                offer.PriceRu = price;
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                    }
                }

                var warehouseNodes = node.ChildNodes.Cast<XmlNode> ();
                foreach (var warehouseNode in warehouseNodes.Where (x => x.Name == "Склад")) {
                    var resultCount = 0;
                    var countAttr = warehouseNode.Attributes["КоличествоНаСкладе"];

                    int.TryParse (countAttr.Value, out resultCount);

                    offer.Count += resultCount;
                }

                offers.Add (offer);
            }

            // 
            var producersDictionary = GetProducersDictionary (_partProducerRepository);

            // OFFERS #2
            filePath = GetPathToFile (_hostingEnvironment, files.First (x => x.Name == "import0_1.xml"), supplier.SearchName);
            xmlDoc = new XmlDocument ();
            xmlDoc.Load (filePath);

            var importTag = "Товар";
            var importNodes = xmlDoc.GetElementsByTagName (importTag);
            var imports = new List<EnbsvImport> ();
            foreach (XmlNode node in importNodes) {
                var import = new EnbsvImport { };

                import.Id = node["Ид"].InnerText;
                import.ProducerCode = node["Артикул"].InnerText;

                try {
                    import.ProducerName = this.CheckProducerNameErrors (node["Изготовитель"]["Наименование"].InnerText);
                } catch (System.Exception e) {
                    var v = e.Message;
                }

                var requisiteNodes = node["ЗначенияРеквизитов"].ChildNodes;

                foreach (XmlNode requisiteNode in requisiteNodes) {
                    switch (requisiteNode["Наименование"].InnerText) {
                        case "Полное наименование":
                            {
                                import.Name = requisiteNode["Значение"].InnerText;
                                break;
                            }
                    }
                }

                if (!string.IsNullOrEmpty (import.ProducerName) &&
                    !producersDictionary.ContainsKey (import.ProducerName)) {
                    var newProducer = await this._partProducerRepository.CreateProducerIfNotExist (import.ProducerName);
                    producersDictionary.Add (newProducer.Name, newProducer);
                }

                imports.Add (import);
            }

            // Merge in new supplier items

            var offersDictionary = offers.ToDictionary (x => x.Id);

            var supplierItems = new List<SupplierPriceItem> ();
            foreach (var import in imports) {
                var supplierItem = new SupplierPriceItem ();

                if (offersDictionary.ContainsKey (import.Id)) {
                    var offer = offersDictionary[import.Id];

                    supplierItem.Price = offer.PriceRu;
                    supplierItem.PriceUsd = offer.PriceUsd;
                    supplierItem.PriceEu = offer.PriceEu;

                    supplierItem.Count = offer.Count;

                    supplierItem.ProducerCode = offer.ProducerCode;
                }

                supplierItem.PartsSupplier = supplier;
                supplierItem.Name = import.Name;

                if (!string.IsNullOrEmpty (import.ProducerName)) {
                    var producer = producersDictionary[import.ProducerName];
                    supplierItem.Producer = producer;
                    supplierItem.ProducerName = producer.Name;
                }

                if (string.IsNullOrEmpty (supplierItem.ProducerCode)) {
                    supplierItem.ProducerCode = import.ProducerCode;
                }

                supplierItems.Add (supplierItem);
            }

            await this._supplierPriceItemRepository.AddRangeForSupplier (supplierItems, supplier);
        }

        public override async Task ImportParameters (string SERVICE_SEARCH_NAME) {

            var energobyt = new PartsSupplier {
                Name = "Энергобыт Сервис, ООО",
                SearchName = SERVICE_SEARCH_NAME,
                Site = "enbsv.ru",
                Email = "info@enbsv.ru",
                Contacts = new List<SupplierContact> {
                new SupplierContact {
                Phone = "74953745351"
                },
                },
                SalePoints = new List<SalePoint> {
                new SalePoint {
                Name = "Главный офис",
                Phone = "74953745351",
                Address = new Address {
                Locality = "г.Химки",
                Location = "ул. Бабакина, 5а, оф. 104"
                },
                TimeWorks = new List<TimeWork> {
                new TimeWork {
                Description = "Пн-Пт: с 9-00 до 21-00"
                }
                },
                DeliveryMethod = "Cамовывоз, курьер, ночная доставка"
                }
                },
                Logo = new ModelFile {
                Name = "logo.png"
                }
            };

            energobyt = await _partSupplierRepository.CreateSupplierIfNotExist (energobyt);
        }

        public Address GetAddress (XmlNode node) {
            var address = new Address ();

            foreach (XmlNode addressItem in node) {
                var value = addressItem.InnerText;
                switch (addressItem.Name) {
                    case "АдресноеПоле":
                        {
                            address.Location = addressItem["Значение"].InnerText;
                            break;
                        }
                }
            }

            return address;
        }

        public IEnumerable<WarehouseContact> GetWarehouseContacts (XmlNode node) {
            var contacts = new List<WarehouseContact> ();

            foreach (XmlNode contactsNode in node.ChildNodes) {
                var contact = new WarehouseContact ();
                foreach (XmlNode contactNode in contactsNode.ChildNodes) {
                    var value = contactNode.InnerText;
                    switch (contactNode.Name) {
                        case "Значение":
                            {
                                contact.Address = new Address {
                                    Location = value
                                };
                                break;
                            }
                    }

                }
                contacts.Add (contact);
            }

            return contacts;
        }

        private readonly ISupplierOfferFilesRepository _supplierOfferFilesRepository;
        private readonly IPartSupplierRepository _partSupplierRepository;
        private readonly ISupplierWarehouseRepository _supplierWarehouseRepository;

        private readonly IPartProducerRepository _partProducerRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        private readonly ISupplierPriceItemRepository _supplierPriceItemRepository;
        public EnbsvParserService (
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
    }

}