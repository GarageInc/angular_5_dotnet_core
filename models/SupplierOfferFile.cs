using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {
    [Table ("SupplierOfferFile")]
    public class SupplierOfferFile : ModelFileAbstract {

        public int? UserId { get; set; }
        public User User { get; set; }

        public PartsSupplier PartsSupplier { get; set; }

        public string ErrorMessage { get; set; }

        public string GroupIdentifier { get; set; }

        public OfferFileStatusEnum Status { get; set; }
    }

    public enum OfferFileStatusEnum {
        Downloaded = 0,
        Processed = 1,
        Error = 2,
    }
}