using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {

    [Table ("PartsSupplier")]
    // Поставщик запчастей
    public class PartsSupplier : BaseModelAbstract {
        public string Name { get; set; }

        public string SearchName { get; set; }

        public string Email { get; set; }
        public ModelFile Logo { get; set; }
        public string Site { get; set; }

        public string INN { get; set; }

        public IEnumerable<SupplierWarehouse> Warehouses { get; set; }

        public IEnumerable<SupplierContact> Contacts { get; set; }
        public IEnumerable<User> Users { get; set; }

        public IList<SalePoint> SalePoints { get; set; }

        public int? SeoParameterId { get; set; }
        public SeoParameter SeoParameter { get; set; }

    }

    public class SupplierContact : Contact { }
}