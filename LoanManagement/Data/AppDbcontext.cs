using Microsoft.EntityFrameworkCore;
using LoanManagement.Models;
using System.Reflection.Emit;

namespace LoanManagement.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        // DbSets for all models
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<LoanAdmin> LoanAdmins { get; set; }
        public DbSet<LoanOfficer> LoanOfficers { get; set; }
        public DbSet<LoanScheme> LoanSchemes { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Installment> Installments { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique(); // Unique email constraint
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Role).HasConversion<string>(); // Store enum as string
            });

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.Property(c => c.City).IsRequired().HasMaxLength(100);
                entity.Property(c => c.ContactNumber).IsRequired().HasMaxLength(10);

                // One-to-One relationship with User
                entity.HasOne(c => c.User)
                      .WithOne(u => u.Customer)
                      .HasForeignKey<Customer>(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LoanAdmin entity
            modelBuilder.Entity<LoanAdmin>(entity =>
            {
                entity.HasKey(la => la.AdminId);

                // One-to-One relationship with User
                entity.HasOne(la => la.User)
                      .WithOne(u => u.LoanAdmin)
                      .HasForeignKey<LoanAdmin>(la => la.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LoanOfficer entity
            modelBuilder.Entity<LoanOfficer>(entity =>
            {
                entity.HasKey(lo => lo.OfficerId);
                entity.Property(lo => lo.City).IsRequired().HasMaxLength(100);

                // One-to-One relationship with User
                entity.HasOne(lo => lo.User)
                      .WithOne(u => u.LoanOfficer)
                      .HasForeignKey<LoanOfficer>(lo => lo.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure LoanScheme entity
            modelBuilder.Entity<LoanScheme>(entity =>
            {
                entity.HasKey(ls => ls.SchemeId);
                entity.Property(ls => ls.SchemeName).IsRequired().HasMaxLength(200);
                entity.Property(ls => ls.InterestRate).HasColumnType("decimal(5,2)"); // e.g., 12.50%
                entity.Property(ls => ls.MaxAmount).HasColumnType("decimal(18,2)");
                entity.Property(ls => ls.Description).HasMaxLength(1000);
            });

            // Configure LoanApplication entity
            modelBuilder.Entity<LoanApplication>(entity =>
            {
                entity.HasKey(la => la.ApplicationId);
                entity.Property(la => la.Status).IsRequired().HasMaxLength(20);
                entity.Property(la => la.LoanAmount).HasColumnType("decimal(18,2)");
                entity.Property(la => la.DocumentUploaded).HasMaxLength(500);

                // Many-to-One relationship with Customer
                entity.HasOne(la => la.Customer)
                      .WithMany(c => c.LoanApplications)
                      .HasForeignKey(la => la.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Many-to-One relationship with LoanOfficer
                entity.HasOne(la => la.LoanOfficer)
                      .WithMany(lo => lo.AssignedApplications)
                      .HasForeignKey(la => la.AssignedOfficerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Many-to-One relationship with LoanScheme
                entity.HasOne(la => la.LoanScheme)
                      .WithMany(ls => ls.LoanApplications)
                      .HasForeignKey(la => la.SchemeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Loan entity
            modelBuilder.Entity<Loan>(entity =>
            {
                entity.HasKey(l => l.LoanId);
                entity.Property(l => l.LoanAmount).HasColumnType("decimal(18,2)");
                entity.Property(l => l.IsNPA).HasDefaultValue(false);

                // One-to-One relationship with LoanApplication
                entity.HasOne(l => l.LoanApplication)
                      .WithOne(la => la.Loan)
                      .HasForeignKey<Loan>(l => l.ApplicationId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Many-to-One relationship with Customer
                entity.HasOne(l => l.Customer)
                      .WithMany(c => c.Loans)
                      .HasForeignKey(l => l.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Installment entity
            modelBuilder.Entity<Installment>(entity =>
            {
                entity.HasKey(i => i.InstallmentId);
                entity.Property(i => i.AmountDue).HasColumnType("decimal(18,2)");
                entity.Property(i => i.AmountPaid).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(i => i.Status).IsRequired().HasMaxLength(20);

                // Many-to-One relationship with Loan
                entity.HasOne(i => i.Loan)
                      .WithMany(l => l.Installments)
                      .HasForeignKey(i => i.LoanId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Many-to-One relationship with Customer
                entity.HasOne(i => i.Customer)
                      .WithMany(c => c.Installments)
                      .HasForeignKey(i => i.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint: One installment number per loan
                entity.HasIndex(i => new { i.LoanId, i.InstallmentNumber })
                      .IsUnique();
            });

            // Configure Repayment entity
            modelBuilder.Entity<Repayment>(entity =>
            {
                entity.HasKey(r => r.RepaymentId);
                entity.Property(r => r.Amount).HasColumnType("decimal(18,2)");
                entity.Property(r => r.Method).HasMaxLength(50);
                entity.Property(r => r.TransactionId).HasMaxLength(100);

                // One-to-One relationship with Installment
                entity.HasOne(r => r.Installment)
                      .WithOne(i => i.Repayment)
                      .HasForeignKey<Repayment>(r => r.InstallmentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Many-to-One relationship with Customer
                entity.HasOne(r => r.Customer)
                      .WithMany(c => c.Repayments)
                      .HasForeignKey(r => r.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Document entity
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(d => d.DocumentId);
                entity.Property(d => d.FileName).IsRequired().HasMaxLength(255);
                entity.Property(d => d.FilePath).IsRequired().HasMaxLength(500);

                // Many-to-One relationship with LoanApplication
                entity.HasOne(d => d.LoanApplication)
                      .WithMany(la => la.Documents)
                      .HasForeignKey(d => d.ApplicationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Report entity
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.ReportId);
                entity.Property(r => r.ReportType).IsRequired().HasMaxLength(100);

                // Many-to-One relationship with LoanAdmin
                entity.HasOne(r => r.LoanAdmin)
                      .WithMany(la => la.Reports)
                      .HasForeignKey(r => r.AdminId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Check constraint: EndDate should be after StartDate
                entity.HasCheckConstraint("CK_Report_DateRange", "EndDate >= StartDate");
            });

            // Additional Configurations

            // Seed some default data (optional)
            SeedDefaultData(modelBuilder);

            // Add indexes for better performance
            modelBuilder.Entity<LoanApplication>()
                       .HasIndex(la => la.Status);

            modelBuilder.Entity<Loan>()
                       .HasIndex(l => l.IsNPA);

            modelBuilder.Entity<Installment>()
                       .HasIndex(i => i.Status);

            modelBuilder.Entity<Installment>()
                       .HasIndex(i => i.DueDate);
        }

        private void SeedDefaultData(ModelBuilder modelBuilder)
        {
            // Seed some default loan schemes
            modelBuilder.Entity<LoanScheme>().HasData(
                new LoanScheme
                {
                    SchemeId = 1,
                    SchemeName = "Home Loan",
                    InterestRate = 8.5m,
                    MaxAmount = 5000000m,
                    DurationsInMonths = 240,
                    Description = "Low interest rate home loan for residential properties"
                },
                new LoanScheme
                {
                    SchemeId = 2,
                    SchemeName = "Personal Loan",
                    InterestRate = 12.0m,
                    MaxAmount = 1000000m,
                    DurationsInMonths = 60,
                    Description = "Quick personal loan for immediate financial needs"
                },
                new LoanScheme
                {
                    SchemeId = 3,
                    SchemeName = "Car Loan",
                    InterestRate = 9.5m,
                    MaxAmount = 2000000m,
                    DurationsInMonths = 84,
                    Description = "Affordable car loan with competitive interest rates"
                },
                new LoanScheme
                {
                    SchemeId = 4,
                    SchemeName = "Education Loan",
                    InterestRate = 7.0m,
                    MaxAmount = 3000000m,
                    DurationsInMonths = 180,
                    Description = "Education loan for higher studies with flexible repayment"
                }
            );
        }

        // Override SaveChanges to add timestamps automatically
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in entries)
            {
                if (entry.Entity is LoanApplication app && app.ApplicationDate == default)
                {
                    app.ApplicationDate = DateTime.Now;
                }
                if (entry.Entity is Document doc && doc.UploadDate == default)
                {
                    doc.UploadDate = DateTime.Now;
                }
                if (entry.Entity is Report report && report.GeneratedDate == default)
                {
                    report.GeneratedDate = DateTime.Now;
                }
                if (entry.Entity is Repayment repayment && repayment.RepaymentDate == default)
                {
                    repayment.RepaymentDate = DateTime.Now;
                }
                if (entry.Entity is Loan loan && loan.IssueDate == default)
                {
                    loan.IssueDate = DateTime.Now;
                }
            }
        }
    }
}