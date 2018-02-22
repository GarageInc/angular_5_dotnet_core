using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;

namespace depot {

    public interface IPartProducerService {
        IQueryable<object> List ();
    }

    public class PartProducerService : IPartProducerService {
        public readonly IPartProducerRepository _partProducerRepository;

        public PartProducerService (IPartProducerRepository partProducerRepository) {
            this._partProducerRepository = partProducerRepository;
        }

        public IQueryable<object> List () => this._partProducerRepository.List ();

    }

}