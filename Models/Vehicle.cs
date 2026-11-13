namespace AutoServiceManager.Models;

public class Vehicle
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public string RegistrationNumber { get; set; } = "";
    public int Year { get; set; }
    public string OwnerName { get; set; } = "";
    public string DisplayName => $"{Brand} {Model} ({RegistrationNumber})";
}
