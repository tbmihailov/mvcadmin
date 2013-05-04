using System.Data.Entity;

namespace MvcAdminResearch.Models
{
    public class NotesappContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<MvcAdminResearch.Models.NotesappContext>());

        public NotesappContext() : base("name=NotesappContext")
        {
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<NotesappContext>(new DropCreateDatabaseIfModelChanges<NotesappContext>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
