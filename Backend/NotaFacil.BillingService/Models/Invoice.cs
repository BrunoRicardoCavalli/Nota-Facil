using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NotaFacil.BillingService.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public string Numero { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public List<InvoiceItem> Items { get; set; } = new();

        // Calcula o total da nota (caso use os itens)
        public decimal TotalAmount => Items.Sum(i => i.Total);
    }
}
