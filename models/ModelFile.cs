using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {
    [Table ("ModelFile")]
    public class ModelFile : ModelFileAbstract { }

    public abstract class ModelFileAbstract : BaseModelAbstract {
        public string Name { get; set; }
    }

}