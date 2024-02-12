﻿using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMOGUSIK.Entities
{
    public partial class AudiCenterusContext : DbContext
    {
        public AudiCenterusContext()
        {
        }

        public AudiCenterusContext(DbContextOptions<AudiCenterusContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-IPI4ON8\\SQLExpress;" +
                "Initial Catalog=AudiCenterus;Integrated Security=True;MultipleActiveResultSets=True;" +
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

                // Добавление связи с Employees
                entity.HasMany(r => r.Employees)
                      .WithOne(e => e.Role)
                      .HasForeignKey(e => e.RoleID)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_Employees_Roles");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerID);
                entity.Property(e => e.CustomerID).ValueGeneratedNever();
                entity.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Username).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.RoleID).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.RoleID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_Roles");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeID);
                entity.Property(e => e.EmployeeID).ValueGeneratedNever();
                entity.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Position).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.RoleID).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.RoleID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
