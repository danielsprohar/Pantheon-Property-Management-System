using Microsoft.EntityFrameworkCore;
using Nubles.Core.Domain.Models;
using Nubles.Infrastructure.EntityConfigurations;

namespace Nubles.Infrastructure.Data
{
    public class NublesDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerRentalAgreement> CustomerRentalAgreements { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public DbSet<ParkingSpace> ParkingSpaces { get; set; }
        public DbSet<ParkingSpaceType> ParkingSpaceTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<RentalAgreement> RentalAgreements { get; set; }
        public DbSet<RentalAgreementType> RentalAgreementTypes { get; set; }

        public NublesDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerRentalAgreementConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new InvoicePaymentConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceStatusConfiguration());
            modelBuilder.ApplyConfiguration(new ParkingSpaceConfiguration());
            modelBuilder.ApplyConfiguration(new ParkingSpaceTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentMethodConfiguration());
            modelBuilder.ApplyConfiguration(new RentalAgreementConfiguration());
            modelBuilder.ApplyConfiguration(new RentalAgreementTypeConfiguration());
        }
    }
}