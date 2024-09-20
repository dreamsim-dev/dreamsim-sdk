namespace Dreamsim.Publishing
{
public readonly struct AdInfo
{
    public readonly string Network;
    public readonly double Revenue;
    public readonly string Placement;
    public readonly string Unit;

    public AdInfo(string network, double revenue, string placement, string unit)
    {
        Network = network;
        Revenue = revenue;
        Placement = placement;
        Unit = unit;
    }
}
}