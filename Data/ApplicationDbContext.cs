using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Itransition_Course_Project.Models;

namespace Itransition_Course_Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enable pg_trgm extension for trigram-based full-text search
            builder.HasPostgresExtension("pg_trgm");

            // --- Item configuration ---
            builder.Entity<Item>()
                .HasIndex(i => new { i.InventoryId, i.CustomId })
                .IsUnique();

            builder.Entity<Item>()
                .HasOne(i => i.Inventory)
                .WithMany(inv => inv.Items)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Item>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Item>()
                .Property(i => i.RowVersion)
                .IsRowVersion();

            // --- Inventory configuration ---
            builder.Entity<Inventory>()
                .Property(i => i.RowVersion)
                .IsRowVersion();

            builder.Entity<Inventory>()
                .HasOne(i => i.Creator)
                .WithMany()
                .HasForeignKey(i => i.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Comment configuration ---
            builder.Entity<Comment>()
                .HasOne(c => c.Inventory)
                .WithMany()
                .HasForeignKey(c => c.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .Property(c => c.RowVersion)
                .IsRowVersion();

            // --- Trigram indexes for full-text search (no full table scans) ---
            builder.Entity<Inventory>()
                .HasIndex(i => i.Title)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");

            builder.Entity<Inventory>()
                .HasIndex(i => i.Description)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");

            builder.Entity<Inventory>()
                .HasIndex(i => i.Category)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");

            builder.Entity<Item>()
                .HasIndex(i => i.CustomString1_Value)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");

            builder.Entity<Item>()
                .HasIndex(i => i.CustomString2_Value)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");

            builder.Entity<Item>()
                .HasIndex(i => i.CustomString3_Value)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");

            builder.Entity<Item>()
                .HasIndex(i => i.CustomId)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
        }
    }
}