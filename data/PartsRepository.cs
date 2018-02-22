namespace depot {

    using System.Collections.Generic;

    public interface IPartsRepository {
        IEnumerable<string> All { get; }
    }

    public class PartsRepository : IPartsRepository {

        public IEnumerable<string> All => new List<string> { "Continuous", "Integration", "is", "Pretty", "Neat", "Eh?", "Yo" };

        public PartsRepository () {

        }
    }
}