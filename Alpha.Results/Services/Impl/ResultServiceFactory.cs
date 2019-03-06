namespace Alpha.Results.Site.Services.Impl
{
    using System.Diagnostics.CodeAnalysis;
    using Constants;
    using Products;

    public class ResultServiceFactory : IResultServiceFactory
    {
        public const int MetricGaming60SecondOfKick = 801;
        public const int MetricGamingEndBetType = 899;

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        public IProductResultService CreateProductResultService(int sportId, int betTypeId)
        {
            if (betTypeId == BetTypeIdConstants.MixParlay
                || betTypeId == BetTypeIdConstants.CombineMixParlay
                || betTypeId == BetTypeIdConstants.MixParlayBetRadarVirtualSports)
            {
                return new ParlayResultService();
            }

            switch (sportId)
            {
                case SportTypeIdConstants.Soccer:
                    if (MetricGaming60SecondOfKick <= betTypeId
                        && betTypeId <= MetricGamingEndBetType)
                    {
                        return new MetricGamingResultService();
                    }

                    return new SportsbookResultService();                    

                case SportTypeIdConstants.NumberGame:                
                    return new NumberGameResultService();
                case SportTypeIdConstants.Happy5:
                    return new Happy5ResultService();
                case SportTypeIdConstants.Lucky3:
                    return new Lucky3ResultService();                    

                case SportTypeIdConstants.AllBet:
                    return new AllBetCasinoResultService();

                case SportTypeIdConstants.VirtualHorsesRacing:
                case SportTypeIdConstants.VirtualGreyhounds:
                case SportTypeIdConstants.VirtualMotors:
                case SportTypeIdConstants.VirtualCycling:
                case SportTypeIdConstants.VirtualSpeedway:
                    return new VirtualRacingResultService();

                case SportTypeIdConstants.GoldDeluxe:
                    return new GoldDeluxeResultService();

                default:
                    return new SportsbookResultService();
            }
        }
    }
}
