namespace ArtilleryGame;

public class Cannon
{
    public double Bearing { get; set; }
    public double Elevation { get; set; }
    public int Charge { get; set; }

    public Cannon()
    {
        Bearing = 0;  // 默认指向正北
        Elevation = 45;
        Charge = 1;
    }

    public void Turn(double angle)
    {
        Bearing = (Bearing + angle) % 360;
        if (Bearing < 0) Bearing += 360;
        Console.WriteLine(LanguageManager.GetString("cannon_turned", FormatBearing(Bearing)));
    }

    public void SetElevation(double elevation)
    {
        Elevation = Math.Max(1, Math.Min(89, elevation));
        Console.WriteLine(LanguageManager.GetString("cannon_elevation", Elevation));
    }

    public void SetCharge(int charge)
    {
        Charge = charge;
        Console.WriteLine(LanguageManager.GetString("cannon_charged", Charge));
    }

    public static string FormatBearing(double bearing)
    {
        // 将方位角标准化到 [0, 360)
        bearing = ((bearing % 360) + 360) % 360;

        string direction;

        if (bearing >= 337.5 || bearing < 22.5)
            direction = LanguageManager.GetString("dir_north");
        else if (bearing >= 22.5 && bearing < 67.5)
            direction = LanguageManager.GetString("dir_northeast");
        else if (bearing >= 67.5 && bearing < 112.5)
            direction = LanguageManager.GetString("dir_east");
        else if (bearing >= 112.5 && bearing < 157.5)
            direction = LanguageManager.GetString("dir_southeast");
        else if (bearing >= 157.5 && bearing < 202.5)
            direction = LanguageManager.GetString("dir_south");
        else if (bearing >= 202.5 && bearing < 247.5)
            direction = LanguageManager.GetString("dir_southwest");
        else if (bearing >= 247.5 && bearing < 292.5)
            direction = LanguageManager.GetString("dir_west");
        else
            direction = LanguageManager.GetString("dir_northwest");

        return $"{bearing:F1}° ({direction})";
    }
}