using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace depot {
    public abstract class AbstractParserService : IParserService {
        public abstract Task ImportParameters (string SERVICE_SEARCH_NAME);
        public abstract Task Run (string SERVICE_SEARCH_NAME);

        public abstract Task Process (IEnumerable<SupplierOfferFile> files, PartsSupplier supplier);

        public string GetPathToFile (IHostingEnvironment hostingEnvironment, SupplierOfferFile file, string SERVICE_SEARCH_NAME) {
            return Path.Combine (hostingEnvironment.WebRootPath + "./../files/offers_of_suppliers/" + SERVICE_SEARCH_NAME + "/" + file.GroupIdentifier + "/", file.Name);
        }

        public string GetLatestFilesFolder (IHostingEnvironment _hostingEnvironment) {
            var path = _hostingEnvironment.WebRootPath + "./../files/offers_of_suppliers/" + "energobyt/";

            var directory = new DirectoryInfo (path);
            var myFile = (from f in directory.GetDirectories () orderby f.LastWriteTime descending select f).First ();

            return myFile.FullName;
        }

        public Dictionary<string, PartProducer> GetProducersDictionary (IPartProducerRepository _partProducerRepository) {

            // Load producers
            var producers = _partProducerRepository.GetAll ();
            var producersDictionary = new Dictionary<string, PartProducer> ();

            foreach (var producer in producers) {
                producersDictionary.Add (producer.Name, producer);
            }

            return producersDictionary;
        }
        public string CheckProducerNameErrors (string producerName) {
            if (producerName.Contains ("Beretta/Riello")) {
                return "Beretta";
            }

            return producerName;
        }

        public async virtual Task ImportOffers (string SERVICE_SEARCH_NAME, IPartSupplierRepository _partSupplierRepository, ISupplierOfferFilesRepository _supplierOfferFilesRepository) {

            var supplier = _partSupplierRepository.GetByName (SERVICE_SEARCH_NAME);
            var files = _supplierOfferFilesRepository.NeedProcessingFor (supplier).ToList ();

            foreach (var file in files) {
                try {
                    await this.Process (new List<SupplierOfferFile> { file }, supplier);

                    file.Status = OfferFileStatusEnum.Processed;
                } catch (Exception ex) {
                    file.Status = OfferFileStatusEnum.Error;
                    file.ErrorMessage = ex.Message + ex.StackTrace;
                }

            }

            await _supplierOfferFilesRepository.SaveRange (files);
        }

    }

}