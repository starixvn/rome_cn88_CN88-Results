namespace Alpha.Results.Site.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Criteria;
    using DataAccesses;
    using Fanex.Result.Core.Lucky3.DTO;

    public class Lucky3ResultDataAccess : ILucky3ResultDataAccess
    {
        public IList<Lucky3> GetResults(GetNumberGameResultCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<Lucky3>();
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        BindLucky3(result, reader);
                    }
                }
            }

            return result;
        }

        public IList<Lucky3> GetResultDetails(GetNumberGameResultDetailsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<Lucky3>();
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        BindLucky3(result, reader);
                    }
                }
            }

            return result;
        }

        private static void BindLucky3(List<Lucky3> result, IDataReader reader)
        {
            var matchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0;
            var existMatch = result.SingleOrDefault(i => i.MatchId == matchId);
            if (existMatch != null)
            {
                existMatch.Seq.Add(reader["seq"] != DBNull.Value ? int.Parse(reader["seq"].ToString()) : 0);
                existMatch.Ball.Add(reader["number"] != DBNull.Value ? int.Parse(reader["number"].ToString()) : 0);
            }
            else
            {
                result.Add(new Lucky3
                {
                    LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                    MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                    EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                    EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                    KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                    MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty,
                    FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? int.Parse(reader["finalhomescore"].ToString()) : 0,
                    LiveHomeScore = reader["livehomescore"] != DBNull.Value ? int.Parse(reader["livehomescore"].ToString()) : 0,
                    LiveAwayScore = reader["liveawayscore"] != DBNull.Value ? int.Parse(reader["liveawayscore"].ToString()) : 0,
                    Seq = new List<int> { reader["seq"] != DBNull.Value ? int.Parse(reader["seq"].ToString()) : 0 },
                    Ball = new List<int> { reader["number"] != DBNull.Value ? int.Parse(reader["number"].ToString()) : 0 }
                });
            }
        }
    }
}
