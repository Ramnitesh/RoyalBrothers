using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Lib.Databse.Models
{
    public partial class ramnitesh_aonepageContext : DbContext
    {
        private readonly string _aopDBConnectionString;
        public ramnitesh_aonepageContext()
        {
        }
        public ramnitesh_aonepageContext(string connectionString) : base()
        {
            _aopDBConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_aopDBConnectionString);
            }
        }

        public ramnitesh_aonepageContext(DbContextOptions<ramnitesh_aonepageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblRoleMaster> TblRoleMasters { get; set; }
        public virtual DbSet<TblUserMaster> TblUserMasters { get; set; }
        public virtual DbSet<TblUserRole> TblUserRoles { get; set; }
        public virtual DbSet<TblUserSession> TblUserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblRoleMaster>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__Tbl_Role__8AFACE3A96F21252");

                entity.ToTable("Tbl_RoleMaster", "ReadOnly");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Tbl_User__1788CC4CB968ED6F");

                entity.ToTable("Tbl_User_Master", "Security");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalMailId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrivacyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUserRole>(entity =>
            {
                entity.HasKey(e => e.UroleId)
                    .HasName("PK__Tbl_User__8F3F1B25C184A99C");

                entity.ToTable("Tbl_UserRole", "USERS");

                entity.Property(e => e.UroleId).HasColumnName("URoleID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_UserR__RoleI__2E1BDC42");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_UserR__UserI__2F10007B");
            });

            modelBuilder.Entity<TblUserSession>(entity =>
            {
                entity.HasKey(e => e.SessionGuid);

                entity.ToTable("Tbl_User_Session", "LOGS");

                entity.Property(e => e.SessionGuid)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.EndedAt).HasColumnType("datetime");

                entity.Property(e => e.RefreshToken)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefreshedAt).HasColumnType("datetime");

                entity.Property(e => e.StartedAt).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
