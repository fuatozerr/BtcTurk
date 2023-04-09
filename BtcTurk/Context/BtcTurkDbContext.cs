using BtcTurk.Models;
using Microsoft.EntityFrameworkCore;

namespace BtcTurk.Context
{
    public class BtcTurkDbContext : DbContext
    {
        public BtcTurkDbContext()
        {

        }
        public BtcTurkDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connStr = "Data Source=localhost;Initial Catalog=BtcTurk;Persist Security Info=True;Trust Server Certificate=true;User ID=sa;Password=Fuatko123";
                optionsBuilder.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Instruction> Instructions { get; set; }
        public override int SaveChanges()
        {
            OnBeforeSave();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSave();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void OnBeforeSave()
        {
            //talimat eklendiğinde tarih eklesin ve aktif olsun
            var addedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added)
                .Select(y => (BaseEntity)y.Entity);
            PrepareAddedEntities(addedEntities);
        }
        private void PrepareAddedEntities(IEnumerable<BaseEntity> entities)
        {
            foreach (var item in entities)
            {
                item.CreateDate = DateTime.Now;
                item.IsActive = true;  //yeni bir kayıt eklenirse işlemi aktif olarak olmalı
            }
        }
    }
}
