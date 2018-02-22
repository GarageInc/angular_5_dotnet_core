using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace depot {
    // 
    [Table ("SupplierPriceItem")]
    public class SupplierPriceItem : BaseModelAbstract {

        public string ProducerName { get; set; }

        public decimal Price { get; set; }

        public decimal PriceUsd { get; set; }
        public decimal PriceEu { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        // because it can be filled, but ProducerCatalogItem can't exist before it
        public string ProducerCode { get; set; }

        public int? ProducerId { get; set; }

        public PartProducer Producer { get; set; }

        public int? PartsSupplierId { get; set; }

        public PartsSupplier PartsSupplier { get; set; }

        public bool IsAvailable { get; set; }

        public PartStatusEnum Status { get; set; }
        public string ProducerCodeTrimmed { get; set; }

        public int LastUploadedUpdated { get; set; }

        internal void UpdateBy (SupplierPriceItem newItem) {
            if (newItem != null) {
                this.ProducerName = newItem.ProducerName;
                this.Status = newItem.Status;
                this.ProducerCodeTrimmed = newItem.ProducerCodeTrimmed;
                this.Name = newItem.Name;
                this.Count = newItem.Count;
                this.Price = newItem.Price;
                this.PriceEu = newItem.PriceEu;
                this.PriceUsd = newItem.PriceUsd;
                this.IsDeleted = newItem.IsAvailable;

                this.SetUploadedAt ();
            }
        }

        internal void UpdateBy (ItemForModeration item) {
            if (item != null) {

                this.PartsSupplierId = item.SupplierId;
                this.ProducerId = item.ProducerId;

                this.ProducerCode = item.ProducerCode;
                this.ProducerCodeTrimmed = (item.ProducerCodeTrimmed);

                this.Status = item.Status;

            }
        }

        public void SetUploadedAt () {
            this.LastUploadedUpdated = GetTimeStamp;
        }
    }

}