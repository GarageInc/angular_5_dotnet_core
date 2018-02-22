namespace depot {

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IPartProducerRepository {
        Task<PartProducer> CreateProducerIfNotExist (string producerName);

        IList<PartProducer> GetAll ();
        IQueryable<object> List ();
        PartProducer GetById (int producerId);
    }

    public class PartProducerRepository : IPartProducerRepository {

        private DepotContext _dbContext;

        public PartProducerRepository (DepotContext dbContext) {
            this._dbContext = dbContext;
        }

        public PartProducer GetById (int producerId) {
            return this._dbContext.PartProducers.FirstOrDefault (x => x.ID == producerId);
        }

        public IQueryable<object> List () => this._dbContext.PartProducers.Select (x => new {
            Id = x.ID,
                Name = x.Name
        });

        public IList<PartProducer> GetAll () => this._dbContext.PartProducers.ToList ();
        public async Task<PartProducer> CreateProducerIfNotExist (string producerName) {
            var findByName = this._dbContext.PartProducers.FirstOrDefault (x => x.Name == producerName);

            if (findByName == null) {
                findByName = new PartProducer {
                Name = producerName,
                };

                await this._dbContext.AddAsync (findByName);
                await this._dbContext.SaveChangesAsync ();
            }

            return findByName;
        }
    }
}