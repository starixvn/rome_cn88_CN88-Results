namespace Alpha.Results.Site.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using Criteria;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Result.Core.MetricGaming.DTO;

    public class MetricGamingResultDataAccess : IMetricGamingResultDataAccess
    {
        public IList<MetricGaming> GetResults(GetMetricGamingResultCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<MetricGaming>();

            using (var objDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new MetricGaming
                        {
                            LeagueId = Converter.ToInt(reader["Leagueid"]),
                            MatchId = Converter.ToInt(reader["MatchID"]),
                            GameId = Converter.ToInt(reader["GameID"]),
                            BetTypeTime = Converter.ToString2(reader["Custom1"]),
                            SettlementStatusId = Converter.ToInt(reader["SettlementStatusID"]),
                            GameResult = Converter.ToString2(reader["GameResult"]),
                            OtherInfo = Converter.ToString2(reader["OtherInfo"]),
                            HomeId = Converter.ToString2(reader["HomeId"]),
                            AwayId = Converter.ToString2(reader["awayid"])
                        });
                    }
                }
            }

            return result;
        }
    }
}
