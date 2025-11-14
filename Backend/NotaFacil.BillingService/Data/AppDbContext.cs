using Microsoft.EntityFrameworkCore;
using NotaFacil.BillingService.Models;

namespace NotaFacil.BillingService.Data;

public class AppDbContext : DbContext
{
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceItem> InvoiceItems => Set<InvoiceItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}
