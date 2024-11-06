using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiValidation.DTOs;
using WebApiValidation.Models;
public class ApplicationDbcontext : IdentityDbContext<User>
{   
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {
            
        }
        public DbSet<Studentrec> Studentslist { get; set; }
        public DbSet<Course> Courserecord { get; set; }
        public DbSet<StudentCor> StudentCourses { get; set; }
        public DbSet<TeacherRegister> Teachers { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<TeacherCourse> TeacherCourse { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ScheduleClass> ScheduleClass { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
        // Ensure Identity tables have primary keys
        builder.Entity<IdentityUserLogin<string>>()
            .HasKey(l => new { l.LoginProvider, l.ProviderKey });
        builder.Entity<IdentityUserRole<string>>()
            .HasKey(r => new { r.UserId, r.RoleId });
        builder.Entity<IdentityUserToken<string>>()
            .HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

        builder.Entity<Admin>(entity =>
        {
            entity.HasKey(k => k.AdminId);
            entity.Property(k => k.AdminId).ValueGeneratedOnAdd();
            entity.Property(k => k.Name).IsRequired().HasMaxLength(50);
            entity.Property(k => k.Contactno).IsRequired().HasMaxLength(12);
            entity.Property(k => k.Email).IsRequired().HasMaxLength(50);
            entity.Property(k => k.Password).IsRequired().HasMaxLength(50);
        });
        builder.Entity<Studentrec>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Contactno).HasMaxLength(20);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Password).HasMaxLength(50);
            entity.HasOne(c => c.Class)
                  .WithMany(d => d.Students)
                  .HasForeignKey(c => c.ClassId);
        });
        builder.Entity<Class>(entity => {
            entity.HasKey(s => s.ClassId);
            entity.Property(k => k.ClassId).ValueGeneratedOnAdd();
            entity.Property(c => c.ClassName).IsRequired().HasMaxLength(50);
            entity.HasMany(c => c.Students)
                  .WithOne(s => s.Class)
                  .HasForeignKey(s => s.ClassId);
        });
        builder.Entity<TeacherRegister>(entity =>
        {
            entity.HasKey(k => k.TeacherId);
            entity.Property(k => k.TeacherId).ValueGeneratedOnAdd();
            entity.Property(k => k.Name).IsRequired().HasMaxLength(50);
            entity.Property(k => k.Contactno).IsRequired().HasMaxLength(12);
            entity.Property(k => k.Email).IsRequired().HasMaxLength(50);
            entity.Property(k => k.Password).IsRequired().HasMaxLength(50);
        });
        builder.Entity<StudentCor>(entity =>
            {
                entity.HasOne(d => d.Course)
                .WithMany(p => p.StudentCourses)
                .HasForeignKey(e => e.Course_Id)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
        builder.Entity<ScheduleClass>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(f => f.Days).IsRequired().HasMaxLength(50);
            entity.Property(f => f.DurationTime).IsRequired().HasMaxLength(50);
            entity.Property(f => f.StartDate).IsRequired().HasMaxLength(30);
            entity.Property(f => f.EndDate).IsRequired().HasMaxLength(30);
            entity.Property(f => f.Room).IsRequired().HasMaxLength(20);
            entity.HasOne(c => c.Class)
                  .WithMany(d => d.ScheduleClass)
                  .HasForeignKey(c => c.ClassId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(c => c.Course)
                  .WithMany(d => d.ScheduleClass)
                  .HasForeignKey(c => c.Course_Id)
                  .OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(c => c.Teacher)
                   .WithMany(d => d.ScheduleClass)
                   .HasForeignKey(c => c.TeacherId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        });
        }
}
