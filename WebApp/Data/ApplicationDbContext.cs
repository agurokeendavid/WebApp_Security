using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Uppercase table names
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Convert table name to uppercase
                entity.SetTableName(entity.GetTableName().ToUpper());

                // Uppercase column names
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToUpper());

                    if (property.ClrType == typeof(bool) || property.ClrType == typeof(bool?))
                    {
                        property.SetColumnType("NUMBER(1)");
                    }
                }

                // Uppercase key column names (optional but recommended)
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToUpper());
                }

                // Uppercase foreign key column names (optional)
                foreach (var fk in entity.GetForeignKeys())
                {
                    fk.SetConstraintName(fk.GetConstraintName().ToUpper());
                }

                // Uppercase index column names (optional)
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToUpper());
                }
            }
        }
    }
}
