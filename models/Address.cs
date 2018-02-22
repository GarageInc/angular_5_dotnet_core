using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {
    [Table ("Address")] // Адресс
    public class Address : BaseModelUserRelationAbstract {
        public string Locality { get; set; }
        public string Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public abstract class ContactAbstract : BaseModelUserRelationAbstract {

        public string Phone { get; set; }
        public Address Address { get; set; }
    }

    [Table ("Contact")]
    public class Contact : ContactAbstract {

    }
}