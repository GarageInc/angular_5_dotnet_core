using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace depot {

    public class DepotContext : DbContext {
        public DepotContext (DbContextOptions<DepotContext> options) : base (options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SupplierPriceItem> SupplierPriceItems { get; set; }
        public DbSet<PartProducer> PartProducers { get; set; }
        public DbSet<PartsSupplier> PartsSuppliers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<SeoParameter> SeoParameters { get; set; }
        public DbSet<ProducerCatalogItem> ProducerCatalogItems { get; set; }
        public DbSet<ModelFile> ModelFiles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<SalePoint> SalePoints { get; set; }
        public DbSet<SupplierWarehouse> SupplierWarehouses { get; set; }

        public DbSet<TimeWork> TimeWorks { get; set; }
        public DbSet<ProducerToProducer> ProducerToProducers { get; set; }
        public DbSet<CatalogItemStatistic> CatalogItemStatistic { get; set; }

        public DbSet<SupplierOfferFile> SupplierOfferFile { get; set; }
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.EnableSensitiveDataLogging ();
            // ...

            base.OnConfiguring (optionsBuilder);
        }
        protected override void OnModelCreating (Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)     {   

            modelBuilder.Entity<User> ().ToTable ("User"); //.HasOne (x => x.PartsSupplier);
            modelBuilder.Entity<Role> ().ToTable ("Role");        
            modelBuilder.Entity<SupplierPriceItem> ().ToTable ("SupplierPriceItem").HasIndex (x => x.ProducerCodeTrimmed);        

            modelBuilder.Entity<SupplierPriceItem> ().HasAlternateKey (x => new {
                x.PartsSupplierId, x.ProducerId, x.ProducerCodeTrimmed
            }).HasName ("AK_SupplierPriceItem_PartsSupId_ProdId_ProdCodeTr");

            modelBuilder.Entity<PartProducer> ().ToTable ("PartProducer");        
            modelBuilder.Entity<PartsSupplier> ().ToTable ("PartsSupplier"); //.HasMany (x => x.Users);        

            modelBuilder.Entity<Address> ().ToTable ("Address");        

            modelBuilder.Entity<ProducerCatalogItem> ().ToTable ("ProducerCatalogItem").HasIndex (x => x.ProducerCodeTrimmed); 
            modelBuilder.Entity<ProducerCatalogItem> ().HasAlternateKey (x => new {
                x.ProducerId, x.ProducerCodeTrimmed
            });    

            modelBuilder.Entity<SeoParameter> ().ToTable ("SeoParameter");           
            modelBuilder.Entity<ModelFile> ().ToTable ("ModelFile");        

            modelBuilder.Entity<Contact> ().ToTable ("Contact");        
            modelBuilder.Entity<SupplierContact> ().ToTable ("SupplierContact");
            modelBuilder.Entity<WarehouseContact> ().ToTable ("WarehouseContact");

            modelBuilder.Entity<SalePoint> ().ToTable ("SalePoint");  
            modelBuilder.Entity<TimeWork> ().ToTable ("TimeWork");              

            modelBuilder.Entity<SupplierWarehouse> ().ToTable ("SupplierWarehouse").HasQueryFilter (p => !p.IsDeleted);;

            modelBuilder.Entity<ProducerToProducer> ().ToTable ("ProducerToProducer")
                .HasKey (t => new { t.FromId, t.ToId });

            modelBuilder.Entity<ProducerToProducer> ()
                .HasOne (sc => sc.From)
                .WithMany (s => s.SynonymsFrom)
                .HasForeignKey (sc => sc.ToId);

            modelBuilder.Entity<ProducerToProducer> ()
                .HasOne (sc => sc.To)
                .WithMany (c => c.SynonymsTo)
                .HasForeignKey (sc => sc.FromId);

            modelBuilder.Entity<CatalogItemStatistic> ().ToTable ("CatalogItemStatistic");

            modelBuilder.Entity<SupplierOfferFile> ().ToTable ("SupplierOfferFile");
        }

        public override int SaveChanges () {
            var changedEntities = ChangeTracker.Entries ();

            foreach (var changedEntity in changedEntities) {
                if (changedEntity.Entity is BaseModelAbstract) {
                    var entity = (BaseModelAbstract) changedEntity.Entity;

                    switch (changedEntity.State) {
                        case EntityState.Added:
                            entity.OnBeforeInsert ();
                            break;

                        case EntityState.Modified:
                            entity.OnBeforeUpdate ();
                            break;

                    }
                }
            }

            return base.SaveChanges ();
        }

        public async Task<int> SaveChangesAsync () {
            var changedEntities = ChangeTracker.Entries ();

            foreach (var changedEntity in changedEntities) {
                if (changedEntity.Entity is BaseModelAbstract) {
                    var entity = (BaseModelAbstract) changedEntity.Entity;

                    switch (changedEntity.State) {
                        case EntityState.Added:
                            entity.OnBeforeInsert ();
                            break;

                        case EntityState.Modified:
                            entity.OnBeforeUpdate ();
                            break;

                    }
                }
            }

            return await base.SaveChangesAsync ();
        }
    }
}