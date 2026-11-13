namespace AutoServiceManager.Models;

public class RepairOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public Guid VehicleId { get; set; }
    public string ClientName { get; set; } = "";
    public string VehicleName { get; set; } = "";
    public string ProblemDescription { get; set; } = "";
    public string WorkDescription { get; set; } = "";
    public decimal Cost { get; set; }
    public string Status { get; set; } = "Новый";
    public DateTime CreatedAt { get; set; } = DateTime.Today;
    public string CreatedAtText => CreatedAt.ToString("dd.MM.yyyy");
    public string CostText => Cost.ToString("N2") + " ₽";
}
