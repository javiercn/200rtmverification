using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EntityFrameworkVerificationApp.Data
{
    public class BillingContext : DbContext
    {
        public BillingContext(DbContextOptions<BillingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>(customer =>
            {
                customer.OwnsOne(c => c.CreditCard).ToTable("CreditCards");
                customer.HasMany(c => c.Invoices).WithOne();
            });

            modelBuilder.Entity<Invoice>(invoice =>
            {
                invoice.OwnsOne(i => i.BillingAddress);
                invoice.HasMany(i => i.Details).WithOne();
            });

            modelBuilder.Entity<InvoiceLine>(invoiceLine =>
            {
                invoiceLine.HasOne(line => line.Item).WithOne().IsRequired(false);
            });

            modelBuilder.Entity<Item>(item => item.HasIndex(i => i.Code).IsUnique());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceLine> InvoiceLine { get; set; }
        public DbSet<Item> Items { get; set; }
    }

    public class Customer
    {
        public string Id { get; set; }
        public CreditCard CreditCard { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }

    public class Invoice
    {
        public int Id { get; set; }
        public Address BillingAddress { get; set; }
        public ICollection<InvoiceLine> Details { get; set; }
    }

    public class InvoiceLine
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public int Ammount { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public decimal Price { get; set; }
    }

    public class CreditCard
    {
        public string FullName { get; set; }
        public long Number { get; set; }
        public int SecurityCode { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
