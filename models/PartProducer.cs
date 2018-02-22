using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {

    [Table ("PartProducer")]
    public class PartProducer : BaseModelUserRelationAbstract {
        public PartProducer () { }

        public string Name { get; set; }
        public string Site { get; set; }
        public ModelFile Logo { get; set; }

        public IList<ProducerToProducer> SynonymsFrom { get; set; }
        public IList<ProducerToProducer> SynonymsTo { get; set; }

        public int? SeoParameterId { get; set; }
        public SeoParameter SeoParameter { get; set; }

    }

    [Table ("ProducerToProducer")]
    public class ProducerToProducer {
        public PartProducer From { get; set; }
        public int FromId { get; set; }

        public PartProducer To { get; set; }
        public int ToId { get; set; }
    }
}