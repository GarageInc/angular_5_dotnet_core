using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {
    // Склад
    [Table ("SupplierWarehouse")]
    public class SupplierWarehouse : BaseModelUserRelationAbstract {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public string IdForProducer { get; set; }

        public string EmailAddress { get; set; }
        public Address Address { get; set; }

        public int SupplierId { get; set; }

        public PartsSupplier Supplier { get; set; }

        public IEnumerable<WarehouseContact> Contacts { get; set; }

    }

    public class WarehouseContact : Contact {

    }
}