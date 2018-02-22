using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace depot {

    public interface IPartSupplierService {
        IQueryable<object> ListFor (int partId);
        Task<PartsSupplier> GetCompanyOf (int userId);
        Task<IList<object>> ListOffers (int UserId);
        Task UploadOffers (IFormFileCollection files, string identifier, int userId);
        Task<bool> RemoveOffers (string groupIdentifier);
    }

    public class PartSupplierService : IPartSupplierService {
        public readonly IPartSupplierRepository _partSupplierRepository;
        public readonly ISupplierOfferFilesRepository _supplierOfferFilesRepository;
        public readonly IHostingEnvironment _hostingEnvironment;

        public PartSupplierService (IPartSupplierRepository partSupplierRepository, ISupplierOfferFilesRepository supplierOfferFilesRepository,
            IHostingEnvironment hostingEnvironment) {
            this._hostingEnvironment = hostingEnvironment;
            this._partSupplierRepository = partSupplierRepository;
            this._supplierOfferFilesRepository = supplierOfferFilesRepository;
        }

        public IQueryable<object> ListFor (int partId) => this._partSupplierRepository.ListFor (partId);

        public async Task<PartsSupplier> GetCompanyOf (int userId) {
            return await this._partSupplierRepository.GetCompanyOf (userId);
        }
        public async Task<bool> RemoveOffers (string groupIdentifier) {
            await this._supplierOfferFilesRepository.Remove (groupIdentifier);
            return true;
        }

        public async Task<IList<object>> ListOffers (int userId) {
            var company = await this.GetSupplierCompany (userId);
            var items = this._supplierOfferFilesRepository.AllFor (company);

            return items.ToList ();
        }

        private async Task<PartsSupplier> GetSupplierCompany (int UserId) {
            return await this.GetCompanyOf (userId: UserId);
        }

        public string FolderPath => this._hostingEnvironment.WebRootPath + $"/../files/offers_of_suppliers/";
        public async Task UploadOffers (IFormFileCollection files, string identifier, int userId) {

            var supplier = (await GetSupplierCompany (userId));

            // full path to file in temp location
            var filePath = $"{FolderPath}/{supplier.SearchName}/{identifier}/";

            if (!Directory.Exists (filePath))
                Directory.CreateDirectory (filePath);

            foreach (var file in files) {
                if (file.Length > 0) {
                    using (var stream = new FileStream ($"{filePath}/{file.FileName}", FileMode.Create)) {
                        await file.CopyToAsync (stream);
                    }
                }
            }

            var entities = files.Select (x => new SupplierOfferFile {
                Name = x.FileName,
                    Status = OfferFileStatusEnum.Downloaded,
                    UserId = userId,
                    PartsSupplier = supplier,
                    GroupIdentifier = identifier
            });

            await this._supplierOfferFilesRepository.AddRange (entities);
        }

        public static DateTime UnixTimeStampToDateTime (double unixTimeStamp) {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime (1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds (unixTimeStamp).ToLocalTime ();
            return dtDateTime;
        }

    }

}