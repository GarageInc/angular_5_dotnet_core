namespace depot {
    public class ModerationFilterModel {

        public string ProducerName { get; set; }
        public string ProducerCode { get; set; }

        public string SupplierName { get; set; }
        public string RuName { get; set; }
        public string EnName { get; set; }

        public int Offset { get; set; }
        public int Count { get; set; }

        public PartStatusEnum Status { get; set; }
    }
}