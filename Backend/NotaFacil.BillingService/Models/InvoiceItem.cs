using System.ComponentModel.DataAnnotations;

namespace NotaFacil.BillingService.Models;

public class InvoiceItem
{
    [Key]
    public int Id { get; set; }

    public int InvoiceId { get; set; }
    public Invoice? Invoice { get; set; } 

    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal Total => Quantity * UnitPrice;
}
