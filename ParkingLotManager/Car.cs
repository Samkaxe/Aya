namespace ParkingLotManager;

public class Car
{
    public string NumberPlate { get; set; }
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; } // Nullable for cars currently parked
    
    public double HoursParked => ExitTime.HasValue 
        ? (ExitTime.Value - EntryTime).TotalHours 
        : 0;
}