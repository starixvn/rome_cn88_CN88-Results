namespace Alpha.Results.Site.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Criteria;
    using DataAccesses;
    using Fanex.Result.Core.NumberGame.DTO;

    /// <summary>
    /// The class NumberGameResultDataAccess.
    /// </summary>
    public class NumberGameResultDataAccess : INumberGameResultDataAccess
    {
        /// <summary>
        /// Get number game result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The number game data.</returns>
        public IList<NumberGame> GetNumberGameResults(GetNumberGameResultCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<NumberGame>();
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        BindNumberGame(result, reader);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the number game popup result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of number games.</returns>
        public IList<NumberGame> GetNumberGameResultDetails(GetNumberGameResultDetailsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<NumberGame>();
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        BindNumberGame(result, reader);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="result">The result list.</param>
        /// <param name="reader">The IDATAREADER.</param>
        private static void BindNumberGame(List<NumberGame> result, IDataReader reader)
        {
            var matchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0;
            var existMatch = result.SingleOrDefault(i => i.MatchId == matchId);
            if (existMatch != null)
            {
                existMatch.Seg.Add(reader["seq"] != DBNull.Value ? int.Parse(reader["seq"].ToString()) : 0);
                existMatch.Ball.Add(reader["number"] != DBNull.Value ? int.Parse(reader["number"].ToString()) : 0);
            }
            else
            {
                result.Add(new NumberGame
                {
                    LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                    MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                    EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                    EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                    KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                    MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty,
                    Seg = new List<int> { reader["seq"] != DBNull.Value ? int.Parse(reader["seq"].ToString()) : 0 },
                    Ball = new List<int> { reader["number"] != DBNull.Value ? int.Parse(reader["number"].ToString()) : 0 }
                });
            }
        }
    }
}
