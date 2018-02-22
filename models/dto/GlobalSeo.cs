namespace depot {
    public class GlobalSeo {
        public TitleDescription Main { get; set; }
        public TitleDescription Suppliers { get; set; }
        public TitleDescription Parts { get; set; }
        public TitleDescription Clarify { get; set; }
        public TitleDescription About { get; set; }
    }

    public class TitleDescription {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}