using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {
    [Table ("ProducerCatalogItem")]
    // Элемент каталога производителя
    public class ProducerCatalogItem : BaseModelAbstract {

        public int? ProducerId { get; set; }
        public PartProducer Producer { get; set; }

        public string ProducerCode { get; set; }
        public string RuName { get; set; }
        public string EnName { get; set; }

        public int? SeoParameterId { get; set; }
        public SeoParameter SeoParameter { get; set; }
        public string ProducerCodeTrimmed { get; set; }

        public IList<CatalogItemStatistic> Statistics { get; set; }

        internal void UpdateBy (ItemForModeration dto) {
            this.RuName = dto.RuName;
            this.EnName = dto.EnName;
            this.ProducerCode = dto.ProducerCode;
            this.ProducerCodeTrimmed = dto.ProducerCodeTrimmed;
        }
    }

}