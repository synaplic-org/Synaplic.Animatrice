namespace Synaplic.Inventory.Transfer.SageModels;

public class SageStockDTO
{
    public string Reference { get; set; }
    public string Designation { get; set; }
    public string Lot { get; set; }
    public decimal QteStockDate { get; set; }
    public decimal QteMouvement { get; set; }
    public string CodeFamille { get; set; }
    
}