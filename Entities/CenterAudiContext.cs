using AMOGUSIK.Entities;
using Microsoft.EntityFrameworkCore;

public partial class CenterAudiContext : DbContext
{
    public CenterAudiContext()
    {
    }

    public CenterAudiContext(DbContextOptions<CenterAudiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Roles> Roles { get; set; }
    public virtual DbSet<Customers> Customers { get; set; }
    // Добавьте остальные DbSet для ваших таблиц

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Подключение к базе данных
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-IPI4ON8\\SQLExpress;" +
                "Initial Catalog=CenterAudi;Integrated Security=True;MultipleActiveResultSets=True;" +
                "TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.RoleID);
            entity.HasMany(r => r.Customers)
                  .WithOne(c => c.Role)
                  .HasForeignKey(c => c.RoleID)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Customers_Roles");
        });



        modelBuilder.Entity<Customers>(entity =>
        {
            entity.HasKey(e => e.CustomerID);
            entity.Property(e => e.CustomerID).ValueGeneratedNever();
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.RoleID).HasColumnName("RoleID");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.RoleID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Roles");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
