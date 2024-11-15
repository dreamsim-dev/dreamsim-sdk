namespace Dreamsim.Publishing
{
public struct ImpressionData
{
    public readonly string AuctionId;
    public readonly string AdUnit;
    public readonly string Country;
    public readonly string AB;
    public readonly string SegmentName;
    public readonly string Placement;
    public readonly string AdNetwork;
    public readonly string InstanceName;
    public readonly string InstanceId;
    public readonly double? Revenue;
    public readonly string Precision;
    public readonly double? LifetimeRevenue;
    public readonly string EncryptedCPM;
    public readonly int? ConversionValue;
    public readonly string AllData;

    public ImpressionData(string auctionId,
        string adUnit,
        string country,
        string ab,
        string segmentName,
        string placement,
        string adNetwork,
        string instanceName,
        string instanceId,
        double? revenue,
        string precision,
        double? lifetimeRevenue,
        string encryptedCPM,
        int? conversionValue,
        string allData)
    {
        AuctionId = auctionId;
        AdUnit = adUnit;
        Country = country;
        AB = ab;
        SegmentName = segmentName;
        Placement = placement;
        AdNetwork = adNetwork;
        InstanceName = instanceName;
        InstanceId = instanceId;
        Revenue = revenue;
        Precision = precision;
        LifetimeRevenue = lifetimeRevenue;
        EncryptedCPM = encryptedCPM;
        ConversionValue = conversionValue;
        AllData = allData;
    }
}
}