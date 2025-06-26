using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace axAssetControl.Entidades;

public partial class AxAssetControlDbContext : DbContext
{
    public AxAssetControlDbContext()
    {
    }

    public AxAssetControlDbContext(DbContextOptions<AxAssetControlDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Active> Actives { get; set; }

    public virtual DbSet<ActiveType> ActiveTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<ControlRecord> ControlRecords { get; set; }

    public virtual DbSet<DetailControl> DetailControls { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<Subsector> Subsectors { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPasswordReset> UserPasswordResets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PCI;Database=AxAssetControlDB;User Id=sa;Password=987123;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Active>(entity =>
        {
            entity.ToTable("active");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("brand");
            entity.Property(e => e.IdActiveType).HasColumnName("id_active_type");
            entity.Property(e => e.IdSubsector).HasColumnName("id_subsector");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_company");
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("model");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.SeriaNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("seria_number");
            entity.Property(e => e.TagRfid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tag_rfid");

            entity.Property(e => e.Version)
            .HasColumnName("version")
            .HasDefaultValue(1);

            entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasDefaultValue(true);

            entity.HasOne(d => d.IdActiveTypeNavigation).WithMany(p => p.Actives)
                .HasForeignKey(d => d.IdActiveType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_active_active_type");

            entity.HasOne(d => d.IdSubsectorNavigation).WithMany(p => p.Actives)
                .HasForeignKey(d => d.IdSubsector)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_active_subsector");

            entity.HasOne(d => d.Company).WithMany(p => p.Actives)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_active_company");
        });

        modelBuilder.Entity<ActiveType>(entity =>
        {
            entity.ToTable("active_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("company");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
        });

        modelBuilder.Entity<ControlRecord>(entity =>
        {
            entity.ToTable("control_record");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd() // ✅ Esto le dice a EF que el valor se genera automáticamente (IDENTITY).
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.IdSubsector).HasColumnName("id_subsector");

            entity.Property(e => e.IdCompany).HasColumnName("id_company");

            entity.HasOne(d => d.IdCompanyNavigation)
                .WithMany(p => p.ControlRecords)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull) // o .Cascade si preferís
                .HasConstraintName("FK_control_record_company");

            entity.HasOne(d => d.IdSubsectorNavigation).WithMany(p => p.ControlRecords)
                .HasForeignKey(d => d.IdSubsector)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_control_record_subsector");
        });

        modelBuilder.Entity<DetailControl>(entity =>
        {
            entity.ToTable("detail_control");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdActivo).HasColumnName("id_activo");
            entity.Property(e => e.IdAuditor).HasColumnName("id_auditor");
            entity.Property(e => e.IdControl).HasColumnName("id_control");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.IdActivoNavigation).WithMany(p => p.DetailControls)
                .HasForeignKey(d => d.IdActivo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_detail_control_active");

            entity.HasOne(d => d.IdAuditorNavigation).WithMany(p => p.DetailControls)
                .HasForeignKey(d => d.IdAuditor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_detail_control_Users");

            entity.HasOne(d => d.IdControlNavigation).WithMany(p => p.DetailControls)
                .HasForeignKey(d => d.IdControl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_detail_control_control_record"); 
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("location");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.IdCompany).HasColumnName("id_company");

            entity.Property(e => e.Version)
            .HasColumnName("version")
            .HasDefaultValue(1);

            entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasDefaultValue(true);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Locations)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_location_company");
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.ToTable("sector");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdLocation).HasColumnName("id_location");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_company");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.TagRfid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tag_rfid");

            entity.Property(e => e.Version)
            .HasColumnName("version")
            .HasDefaultValue(1);

            entity.Property(e => e.Status)
            .HasColumnName("status")
            .HasDefaultValue(true);

            entity.HasOne(d => d.IdLocationNavigation).WithMany(p => p.Sectors)
                .HasForeignKey(d => d.IdLocation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sector_location");

            entity.HasOne(s => s.Company)
                .WithMany(e => e.Sectors)
                .HasForeignKey(s => s.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_sector_company");
        });

            modelBuilder.Entity<Subsector>(entity =>
            {
            entity.ToTable("subsector");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdSector).HasColumnName("id_sector");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_company");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.TagRfid)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tag_rfid");

                entity.Property(e => e.Version)
                .HasColumnName("version")
                .HasDefaultValue(1);

                entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasDefaultValue(true);

                entity.HasOne(d => d.IdSectorNavigation).WithMany(p => p.Subsectors)
                .HasForeignKey(d => d.IdSector)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_subsector_sector");

            entity.HasOne(d => d.Company).WithMany(p => p.Subsectors)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_subsector_company");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdCompany).HasColumnName("id_company");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("status");

            entity.HasOne(d => d.IdCompanyNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdCompany)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_company");
        });

        modelBuilder.Entity<UserPasswordReset>(entity =>
        {
            entity.ToTable("userPasswordReset");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("token");

            entity.Property(e => e.Expiracion)
                .IsRequired()
                .HasColumnName("expiracion");

            entity.Property(e => e.Used).HasColumnName("used");


            entity.HasOne(d => d.User)
                .WithMany() // o WithMany(p => p.PasswordResetTokens) si tenés colección
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_userPasswordReset_User");



        });

        OnModelCreatingPartial(modelBuilder);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<Location>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        foreach (var entry in ChangeTracker.Entries<Sector>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        foreach (var entry in ChangeTracker.Entries<Subsector>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        foreach (var entry in ChangeTracker.Entries<Active>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Location>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        foreach (var entry in ChangeTracker.Entries<Sector>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        foreach (var entry in ChangeTracker.Entries<Subsector>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        foreach (var entry in ChangeTracker.Entries<Active>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version++;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
