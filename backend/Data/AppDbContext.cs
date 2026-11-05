using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<NetworkSite> NetworkSites => Set<NetworkSite>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NetworkSite>(entity =>
        {
            entity.ToTable("network_sites");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(x => x.Name).HasColumnName("name").IsRequired();
            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("users", table => table.HasCheckConstraint(
                "users_role_check", "role IN ('Dispatcher', 'Electrician')"));
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(x => x.Login).HasColumnName("login").IsRequired();
            entity.Property(x => x.Role).HasColumnName("role").IsRequired();
            entity.HasIndex(x => x.Login).IsUnique();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("cable_tickets", table =>
            {
                table.HasCheckConstraint("cable_tickets_title_check", "char_length(title) BETWEEN 5 AND 80");
                table.HasCheckConstraint("cable_tickets_description_check", "char_length(description) BETWEEN 10 AND 500");
                table.HasCheckConstraint("cable_tickets_status_check", "status IN ('New', 'InProgress', 'Closed', 'Cancelled')");
            });
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(x => x.Number).HasColumnName("number").IsRequired();
            entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(80).IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").HasMaxLength(500).IsRequired();
            entity.Property(x => x.SiteId).HasColumnName("site_id");
            entity.Property(x => x.Status).HasColumnName("status").IsRequired();
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone").HasDefaultValueSql("now()");
            entity.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(x => x.AssigneeUserId).HasColumnName("assignee_user_id");
            entity.HasIndex(x => x.Number).IsUnique();
            entity.HasIndex(x => x.Status).HasDatabaseName("cable_tickets_status_idx");
            entity.HasIndex(x => x.SiteId).HasDatabaseName("cable_tickets_site_id_idx");
            entity.HasOne<NetworkSite>().WithMany().HasForeignKey(x => x.SiteId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<AppUser>().WithMany().HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<AppUser>().WithMany().HasForeignKey(x => x.AssigneeUserId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
