namespace ArtilleryGame;

public class Observer
{
    public double Distance { get; set; }
    public double Bearing { get; set; }

    public Observer(double distance, double bearing)
    {
        Distance = distance;
        Bearing = bearing;
    }
}
