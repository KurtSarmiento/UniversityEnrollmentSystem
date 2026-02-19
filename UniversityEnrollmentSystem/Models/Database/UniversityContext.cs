using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniversityEnrollmentSystem.Models.Database;

public partial class UniversityContext : DbContext
{
    public UniversityContext()
    {
    }

    public UniversityContext(DbContextOptions<UniversityContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseOffering> CourseOfferings { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<EnrollmentHistory> EnrollmentHistories { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=EA611-12;Database=University;User ID=User;Password=123456;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.CourseCode, "IX_Courses").IsUnique();

            entity.Property(e => e.CourseCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title1).IsUnicode(false);
        });

        modelBuilder.Entity<CourseOffering>(entity =>
        {
            entity.HasOne(d => d.Course).WithMany(p => p.CourseOfferings)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseOfferings_Courses");

            entity.HasOne(d => d.Instructor).WithMany(p => p.CourseOfferings)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseOfferings_Instructors");

            entity.HasOne(d => d.Semester).WithMany(p => p.CourseOfferings)
                .HasForeignKey(d => d.SemesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CourseOfferings_Semesters");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.DateEnrolled).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FinalGrade).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.CourseOffering).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseOfferingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_CourseOfferings");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollments_Students");
        });

        modelBuilder.Entity<EnrollmentHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__Enrollme__4D7B4ABD21196F3A");

            entity.ToTable("EnrollmentHistory");

            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.ActionType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FinalGrade).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.Property(e => e.Name).IsUnicode(false);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.StudentNumber, "IX_Students").IsUnique();

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
