using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {

    [Table ("SeoParameter")]
    public class SeoParameter : BaseModelUserRelationAbstract {
        public string Name { get; set; }
        public string Description { get; set; }
    }

}