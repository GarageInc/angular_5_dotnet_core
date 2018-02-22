namespace depot {
    public class FileModel {
        public FileModel (string supplierName, string ProducerName, string FileName) {
            this.ProducerName = ProducerName;
            this.FileName = FileName;
            this.SupplierName = supplierName;
        }

        public FileModel (string ProducerName, string FileName) {
            this.ProducerName = ProducerName;
            this.FileName = FileName;
        }
        public string SupplierName { get; set; }

        public string FileName { get; set; }
        public string ProducerName { get; set; }
    }

}