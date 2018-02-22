using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {
    [Table ("CatalogItemStatistic")]
    public class CatalogItemStatistic : BaseModelAbstract {
        public int? CatalogItemId { get; set; }
        public ProducerCatalogItem CatalogItem { get; set; }
    }

}