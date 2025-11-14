namespace NotaFacil.InventoryService.Models
{
    public class Product
    {
        public int Id { get; set; }            // Identificador
        public string Name { get; set; } = ""; // Nome do produto
        public double Price { get; set; }      // Preço
        public int Quantity { get; set; }      // Estoque
    }
}
