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

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Item> Items { get; set; }
    }

    public class Customer
    {
        public string Id { get; set; }
        public CreditCard CreditCard { get; set; }
        public ICollection<Order> OrderHistory { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public Address BillingAddress { get; set; }
        public ICollection<OrderLine> Details { get; set; }
    }

    public class OrderLine
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public int Ammount { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
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
