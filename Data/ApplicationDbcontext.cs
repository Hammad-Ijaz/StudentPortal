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
     	public DbSet<Session> Sessions { get; set; }
     	public DbSet<Challan> Challans { get; set; }
    	public DbSet<Installment> Installments {  get; set; }
        public DbSet<FinanceDetails> FinanceDetailss {  get; set; }

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
            entity.HasKey(s => s.StudentId);
            entity.Property(x => x.StudentId).ValueGeneratedOnAdd();
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

		builder.Entity<Challan>(entity =>
		{
			entity.HasKey(k => k.ChallanId);
			entity.Property(k => k.ChallanId).ValueGeneratedOnAdd();
			entity.Property(k => k.ChallanVoucher).IsRequired().HasMaxLength(50);
			entity.Property(k => k.ToatalFees).IsRequired().HasColumnType("decimal(18, 2)");
		});
		builder.Entity<FinanceDetails>(entity =>
        {
            entity.HasKey(k => k.FinanceId);
            entity.Property(k => k.FinanceId).ValueGeneratedOnAdd();
            entity.Property(x => x.Installments);
            entity.Property(x => x.PaymentDate);
            entity.Property(x => x.RemainingAmount).HasColumnType("decimal(18, 2)");
        });
        builder.Entity<Installment>(entity =>
        {
            entity.HasKey(k => k.InstallmentId);
            entity.Property(d => d.InstallmentId).ValueGeneratedOnAdd();
            entity.Property(d => d.PaymentDate);
            entity.Property(d => d.Paid).HasColumnType("decimal(18, 2)");
            entity.Property(d => d.Unpaid).HasColumnType("decimal(18, 2)");
            entity.Property(d => d.Status).HasMaxLength(30);
        });

        builder.Entity<Session>(entity =>
        {
            entity.HasKey(k => k.SessionId);
            entity.Property(d => d.SessionId).ValueGeneratedOnAdd();
            entity.Property(d => d.SessionStart).IsRequired();
            entity.Property(d => d.SessionEnd).IsRequired();
            entity.Property(d => d.SessionName).IsRequired();

            entity.HasMany(d => d.FinanceDetails)
                  .WithOne(d => d.Session)
                  .HasForeignKey(d => d.SessionId); 
        });
    }
}
