namespace Dreamsim.Publishing
{
    public class ImpressionData
    {
        public readonly string auctionId;
        public readonly string adUnit;
        public readonly string country;
        public readonly string ab;
        public readonly string segmentName;
        public readonly string placement;
        public readonly string adNetwork;
        public readonly string instanceName;
        public readonly string instanceId;
        public readonly double? revenue;
        public readonly string precision;
        public readonly double? lifetimeRevenue;
        public readonly string encryptedCPM;
        public readonly int? conversionValue;
        public readonly string allData;

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
            this.auctionId = auctionId;
            this.adUnit = adUnit;
            this.country = country;
            this.ab = ab;
            this.segmentName = segmentName;
            this.placement = placement;
            this.adNetwork = adNetwork;
            this.instanceName = instanceName;
            this.instanceId = instanceId;
            this.revenue = revenue;
            this.precision = precision;
            this.lifetimeRevenue = lifetimeRevenue;
            this.encryptedCPM = encryptedCPM;
            this.conversionValue = conversionValue;
            this.allData = allData;
        }
    }
}