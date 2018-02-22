namespace depot {
    public class AppOptions {
        public AppOptions () {
            // Set default value.
        }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings {
        public string DefaultConnection { get; set; }
    }
}