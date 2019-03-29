namespace Alpha.Results.Site.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria;
    using DataAccesses;
    using Fanex.Data;
    using Fanex.Result.Core.Racing.DTO;

    public class VirtualRacingResultDataAccess : IVirtualRacingResultDataAccess
    {
        /// <summary>
        /// Get racing result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The racing data.</returns>
        public IList<Racing> GetVirtualRacingResults(GetVirtualRacingResultCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<Racing>();
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindRacing(reader));
                    }
                }
            }

            return result;
        }

        public IList<Racing> GetVirtualRacingResultDetails(GetRacingResultDetailsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<Racing>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(BindRacing(reader));
                    }
                }
            }

            return result;
        }

        private static Racing BindRacing(IDataReader reader)
        {
            return new Racing
            {
                LeagueId = reader["meetingid"] != DBNull.Value ? int.Parse(reader["meetingid"].ToString()) : 0,
                EngineId = reader["HorseId"] != DBNull.Value ? reader["HorseId"].ToString() : string.Empty,
                EngineNo = reader["HorseNo"] != DBNull.Value ? reader["HorseNo"].ToString() : string.Empty,
                IsFavor = reader["IsFavor"] != DBNull.Value && reader["IsFavor"].ToString().Equals("1"),
                IsSCR = reader["IsSCR"] != DBNull.Value && reader["IsSCR"].ToString().Equals("1"),
                Place = reader["place"] != DBNull.Value ? int.Parse(reader["place"].ToString()) : 0,
                PlaceOdds = reader["placeodds"] != DBNull.Value ? decimal.Parse(reader["placeodds"].ToString()) : 0,
                WinOdds = reader["odds"] != DBNull.Value ? decimal.Parse(reader["odds"].ToString()) : 0,
                RaceName = reader["racename"] != DBNull.Value ? int.Parse(reader["racename"].ToString()) : 0,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                Favourite = reader["IsFavor"] != DBNull.Value ? reader["IsFavor"].ToString() : string.Empty,
                RowCount = reader["rowcount"] != DBNull.Value ? int.Parse(reader["rowcount"].ToString()) : 0,
                SCROfMatch = GetSRCValue(reader),
                TFOfMatch = reader["TF"] != DBNull.Value ? reader["TF"].ToString() : string.Empty
            };
        }
        
        private static string GetSRCValue(IDataReader reader)
        {
            if (reader["SCR"] != DBNull.Value)
            {
                string value = reader["SCR"].ToString();

                if (value.EndsWith(",", StringComparison.Ordinal))
                {
                    value = value.Remove(value.Length - 1);
                }

                return value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
