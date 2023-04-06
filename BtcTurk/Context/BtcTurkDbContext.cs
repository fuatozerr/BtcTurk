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

        public DbSet<Instruction> Instructions { get; set; }
        public override int SaveChanges()
        {
            OnBeforeSave();
            return base.SaveChanges();
        }
        private void OnBeforeSave()
        {
            //update olmadıgı için sadece ekleme kısmına yazdım.
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
