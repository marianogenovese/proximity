public class Zone
{
    private readonly double distance;

    public Zone(double distance)
    {
        this.distance = distance;
    }

    public double Distance
    {
        get
        {
            return this.distance;
        }
    }
}

public class Beacon
{
    private readonly Guid id;

    public Beacon(Guid id)
    {
        this.id = id;
    }

    public Guid Id
    {
        get
        {
            return this.id;
        }
    }

}

public class Sector
{
    private readonly Guid id;
    private readonly Zone zone;
    private readonly Beacon[] beacons;

    public Sector(Guid id, Zone zone, Beacon[] beacons)
    {
        this.id = id;
        this.zone = zone;
        this.beacons = beacons;
    }

    public Guid Id
    {
        get
        {
            return this.id;
        }
    }

    public Zone Zone
    {
        get
        {
            return this.zone;
        }
    }

    public Beacons[] Beacons
    {
        get
        {
            return this.beacons;
        }
    }
}

public class Implementation
{
    private readonly Guid id;
    private readonly Sector[] sectors;
    
    public Implementation(Guid id, Sector[] sectors)
    {
        this.id = id;
        this.sectors = sectors;
    }

    public Guid Id
    {
        get
        {
            return this.id;
        }
    }

    public Sector[] Sectors
    {
        get
        {
            return this.sectors;
        }
    }
}