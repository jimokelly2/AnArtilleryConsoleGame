namespace ArtilleryGame;

public class Target
{
    public double Distance { get; set; }
    public double Bearing { get; set; }
    public bool IsDestroyed { get; set; }

    public Target(double distance, double bearing)
    {
        Distance = distance;
        Bearing = bearing;
        IsDestroyed = false;
    }
}
