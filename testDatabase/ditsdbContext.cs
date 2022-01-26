using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace testDatabase
{
    public partial class ditsdbContext : DbContext
    {
        public ditsdbContext()
        {
        }

        public ditsdbContext(DbContextOptions<ditsdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<IncidentHistory> IncidentHistories { get; set; }
        public virtual DbSet<IncidentStatus> IncidentStatuses { get; set; }
        public virtual DbSet<Line> Lines { get; set; }
        public virtual DbSet<Station> Stations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=PELMESHKI_PC\\SQLEXPRESS;Initial Catalog=ditsdb;Integrated Security=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("departments");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(255)
                    .HasColumnName("department_name");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employees");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .HasColumnName("lastname");

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(255)
                    .HasColumnName("patronymic");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_employees_departments");
            });

            modelBuilder.Entity<Incident>(entity =>
            {
                entity.ToTable("incidents");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CloseDate)
                    .HasColumnType("date")
                    .HasColumnName("close_date");

                entity.Property(e => e.Comment).HasColumnName("comment");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.OpenDate)
                    .HasColumnType("date")
                    .HasColumnName("open_date");

                entity.Property(e => e.ResponderId).HasColumnName("responder_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Incidents)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__icindents__emplo__3D5E1FD2");

                entity.HasOne(d => d.Responder)
                    .WithMany(p => p.IncidentResponders)
                    .HasForeignKey(d => d.ResponderId)
                    .HasConstraintName("FK_incidents_incident_status1");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.IncidentStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_incidents_incident_status");
            });

            modelBuilder.Entity<IncidentHistory>(entity =>
            {
                entity.ToTable("incident_history");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.IncidentId).HasColumnName("incident_id");

                entity.Property(e => e.OpenDate)
                    .HasColumnType("date")
                    .HasColumnName("open_date");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<IncidentStatus>(entity =>
            {
                entity.ToTable("incident_status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext")
                    .HasColumnName("description");
            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.ToTable("line");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LineName)
                    .HasMaxLength(255)
                    .HasColumnName("line_name");
            });

            modelBuilder.Entity<Station>(entity =>
            {
                entity.ToTable("stations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.LineId).HasColumnName("line_id");

                entity.Property(e => e.StationName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("station_name");

                entity.HasOne(d => d.Line)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.LineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__stations__line_i__6754599E");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
