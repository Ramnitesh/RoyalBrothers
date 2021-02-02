using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Lib.Databse.Models
{
    public partial class rbrbacdbContext : DbContext
    {
        private readonly string _aopDBConnectionString;
        public rbrbacdbContext()
        {
        }
        public rbrbacdbContext(string connectionString) : base()
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

        public rbrbacdbContext(DbContextOptions<rbrbacdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblActionType> TblActionType { get; set; }
        public virtual DbSet<TblErrorLogs> TblErrorLogs { get; set; }
        public virtual DbSet<TblRoleMaster> TblRoleMaster { get; set; }
        public virtual DbSet<TblRoleSourceAction> TblRoleSourceAction { get; set; }
        public virtual DbSet<TblSource> TblSource { get; set; }
        public virtual DbSet<TblUserMaster> TblUserMaster { get; set; }
        public virtual DbSet<TblUserRole> TblUserRole { get; set; }
        public virtual DbSet<TblUserSession> TblUserSession { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblActionType>(entity =>
            {
                entity.HasKey(e => e.AcitonTypeId)
                    .HasName("PK__Tbl_Acti__6F0C01D695E75AF2");

                entity.ToTable("Tbl_ActionType", "ReadOnly");

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblErrorLogs>(entity =>
            {
                entity.HasKey(e => e.LogSequenceId);

                entity.ToTable("Tbl_ErrorLogs", "LOGS");

                entity.Property(e => e.LogSequenceId).HasColumnName("LogSequenceID");

                entity.Property(e => e.ErrorDescription).IsUnicode(false);

                entity.Property(e => e.ErrorSourceDetails).IsUnicode(false);

                entity.Property(e => e.ErrorType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LoggedDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblRoleMaster>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__Tbl_Role__8AFACE3A7995EA98");

                entity.ToTable("Tbl_RoleMaster", "ReadOnly");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRoleSourceAction>(entity =>
            {
                entity.HasKey(e => e.UserSourceActionId)
                    .HasName("PK__Tbl_Role__317F4786C1FFF660");

                entity.ToTable("Tbl_RoleSourceAction", "ReadOnly");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.AcitonType)
                    .WithMany(p => p.TblRoleSourceAction)
                    .HasForeignKey(d => d.AcitonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_RoleS__Acito__6C190EBB");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblRoleSourceAction)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_RoleS__RoleI__6A30C649");

                entity.HasOne(d => d.Source)
                    .WithMany(p => p.TblRoleSourceAction)
                    .HasForeignKey(d => d.SourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_RoleS__Sourc__6B24EA82");
            });

            modelBuilder.Entity<TblSource>(entity =>
            {
                entity.HasKey(e => e.SourceId)
                    .HasName("PK__Tbl_Sour__16E0191910359C88");

                entity.ToTable("Tbl_Source", "ReadOnly");

                entity.Property(e => e.SourceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Tbl_User__1788CC4CDF4CBC15");

                entity.ToTable("Tbl_User_Master", "Security");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalMailId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrivacyCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUserRole>(entity =>
            {
                entity.HasKey(e => e.UroleId)
                    .HasName("PK__Tbl_User__8F3F1B25FAF039B0");

                entity.ToTable("Tbl_UserRole", "USERS");

                entity.Property(e => e.UroleId).HasColumnName("URoleID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblUserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_UserR__RoleI__628FA481");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tbl_UserR__UserI__6383C8BA");
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
