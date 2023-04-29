using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Models;

public partial class MyDatabaseContext : DbContext
{
    public MyDatabaseContext()
    {
    }

    public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Costumer> Costumers { get; set; }

    public virtual DbSet<Master> Masters { get; set; }

    public virtual DbSet<MasterSchedule> MasterSchedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-78FNHK5T\\SQLEXPRESS;Database=my_database;Trusted_Connection=True;Trust Server Certificate=true;User=LAPTOP-78FNHK5T\\virus;Password=qw79");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("admins", tb => tb.HasTrigger("check_duplicate"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Salt)
                .HasMaxLength(50)
                .HasColumnName("salt");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("appointments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CostumerId).HasColumnName("costumer_id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Costumer).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CostumerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appointments_costumers");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appointments_services");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_appointments_users");
        });

        modelBuilder.Entity<Costumer>(entity =>
        {
            entity.ToTable("costumers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasColumnName("language");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Costumers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_costumers_users");
        });

        modelBuilder.Entity<Master>(entity =>
        {
            entity.ToTable("masters");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone).HasColumnName("phone")
            .HasMaxLength(50);
        });

        modelBuilder.Entity<MasterSchedule>(entity =>
        {
            entity.ToTable("master_schedule");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DayOfWeek)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("day_of_week");
            entity.Property(e => e.MasterId).HasColumnName("master_id");

            entity.HasOne(d => d.Master).WithMany(p => p.MasterSchedules)
                .HasForeignKey(d => d.MasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_master_schedule_master");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("services");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.MasterId).HasColumnName("master_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Master).WithMany(p => p.Services)
                .HasForeignKey(d => d.MasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_services_masters");

            entity.HasOne(d => d.User).WithMany(p => p.Services)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_services_users");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.ToTable("statistic");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CostumerId).HasColumnName("costumer_id");
            entity.Property(e => e.Complete).HasColumnName("complete");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Costumer).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.CostumerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_statistics_costumers");

            entity.HasOne(d => d.User).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_statistics_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    internal Task GeneratePasswordResetTokenAsync(Admin user) {
        throw new NotImplementedException();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
