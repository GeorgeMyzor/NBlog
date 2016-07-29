using ORM.Entities;

namespace ORM
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class EntityModel : DbContext
    {
        public EntityModel()
            : base("name=EntityModel")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<EntityModel, Migrations.Configuration>("EntityModel"));
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
    }
}