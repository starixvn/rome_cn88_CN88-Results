namespace Alpha.Results.Site.DataAccesses.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Criteria;
    using DataAccesses;
    using Fanex.Result.Core.Sportsbook.DTO;

    /// <summary>
    /// The class SportsBookResultDataAccess.
    /// </summary>
    public class SportsbookResultDataAccess : ISportsbookResultDataAccess
    {
        /// <summary>
        /// Get SPORTBOOK result.
        /// </summary>
        /// <param name="criteria">The criterion.</param>
        /// <returns>The SPORTBOOK data.</returns>
        public IList<Sportsbook> GetSportbookResults(GetSportsbookResultCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var results = new List<Sportsbook>();
            var gameDetails = new List<GameDetail>();
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        results.Add(BindSportbook(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            gameDetails.Add(new GameDetail
                            {
                                MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                                ResourceId = reader["ResourceId"] != DBNull.Value ? reader["ResourceId"].ToString() : string.Empty,
                                HomeValue = reader["HomeValue"] != DBNull.Value ? reader["HomeValue"].ToString() : string.Empty,
                                AwayValue = reader["AwayValue"] != DBNull.Value ? reader["AwayValue"].ToString() : string.Empty,
                                SettlementStatusID = reader["SettlementStatusID"] != DBNull.Value ? int.Parse(reader["SettlementStatusID"].ToString()) : 0
                            });
                        }
                    }
                }
            }

            foreach (var result in results)
            {
                result.GameDetails = gameDetails
                    .Where(i => i.MatchId == result.MatchId)
                    .ToList();
            }

            return results;
        }

        /// <summary>
        /// Bind data from IDATAREADER to DTO.
        /// </summary>
        /// <param name="reader">The IDATAREADER.</param>
        /// <returns>The SPORTBOOK DTO.</returns>
        private Sportsbook BindSportbook(IDataReader reader)
        {
            var item = new Sportsbook
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                BetStatus =
                    reader["betstatus"] != DBNull.Value ? reader["betstatus"].ToString() : string.Empty,
                LeagueId = reader["leagueid"] != DBNull.Value ? int.Parse(reader["leagueid"].ToString()) : 0,
                MatchId = reader["matchid"] != DBNull.Value ? int.Parse(reader["matchid"].ToString()) : 0,
                EventStatus = reader["eventstatus"] != DBNull.Value ? reader["eventstatus"].ToString() : string.Empty,
                FinalAwayScore =
                    reader["finalawayscore"] != DBNull.Value
                        ? int.Parse(reader["finalawayscore"].ToString())
                        : new int?(),
                HtAwayScore =
                    reader["htawayscore"] != DBNull.Value ? int.Parse(reader["htawayscore"].ToString()) : new int?(),
                Color = ColumnExisted(reader, "lcolor") && reader["lcolor"] != DBNull.Value ? reader["lcolor"].ToString() : string.Empty,
                FinalHomeScore =
                    reader["finalhomescore"] != DBNull.Value
                        ? int.Parse(reader["finalhomescore"].ToString())
                        : new int?(),
                HtHomeScore =
                    reader["hthomescore"] != DBNull.Value
                        ? int.Parse(reader["hthomescore"].ToString())
                        : new int?(),
                EventDate = reader["eventdate"] != DBNull.Value ? DateTime.Parse(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? bool.Parse(reader["IsNeutral"].ToString()) : false,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader["kickofftime"].ToString()) : DateTime.MinValue,
                MatchCode = reader["matchcode"] != DBNull.Value ? reader["matchcode"].ToString() : string.Empty,
                GoalRecord = reader["goalrecord"] != DBNull.Value ? reader["goalrecord"].ToString() : string.Empty,
                IsHTCal = reader["IsHTCal"] != DBNull.Value ? Convert.ToBoolean(reader["IsHTCal"]) : false,
                Calculate = reader["Calculate"] != DBNull.Value ? Convert.ToBoolean(reader["Calculate"]) : false,
                RefNo = string.Empty,
                GameDetails = new List<GameDetail>(),
                Bettype = GetBetTypeFromToken(reader),
                BetID = GetBetIDFromToken(reader),
                SettlementStatus = reader["SettlementStatus"] != DBNull.Value ? Convert.ToByte(reader["SettlementStatus"]) : default(byte)
            };

            return item;
        }

        private int GetBetTypeFromToken(IDataReader reader)
        {
            const string BettypeColumn = "bettype";
            if (ColumnExisted(reader, BettypeColumn) && reader[BettypeColumn] != DBNull.Value)
            {
                return int.Parse(reader[BettypeColumn].ToString());
            }

            return 0;
        }

        private string GetBetIDFromToken(IDataReader reader)
        {
            const string BetcheckColumn = "betcheck";
            if (ColumnExisted(reader, BetcheckColumn) && reader[BetcheckColumn] != DBNull.Value)
            {
                return reader[BetcheckColumn].ToString();
            }

            return string.Empty;
        }

        public List<Sportsbook> GetScoreInformation(GetSportsbookResultDetailsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var result = new List<Sportsbook>();

            using (var objDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        result.Add(BindSportsbookDetails(reader, criteria));

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                result[0].GameDetails.Add(BindGameDetail(reader, criteria.MatchId));
                            }
                        }

                        if (!result[0].GameDetails.Any())
                        {
                            result[0].GameDetails.Add(new GameDetail());
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Bind game detail value.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>
        /// The game detail value.
        /// </returns>
        private GameDetail BindGameDetail(IDataReader reader, int matchId)
        {
            return new GameDetail
            {
                MatchId = matchId,
                ResourceId = reader["ResourceId"] != DBNull.Value ? reader["ResourceId"].ToString() : string.Empty,
                HomeValue = reader["HomeValue"] != DBNull.Value ? reader["HomeValue"].ToString() : string.Empty,
                AwayValue = reader["AwayValue"] != DBNull.Value ? reader["AwayValue"].ToString() : string.Empty,
                ResultStatusID = reader["ResultStatusID"] != DBNull.Value ? int.Parse(reader["ResultStatusID"].ToString()) : 0,
                SettlementStatusID = reader["SettlementStatusID"] != DBNull.Value ? int.Parse(reader["SettlementStatusID"].ToString()) : 0,
                OtherInfo = reader["OtherInfo"] != DBNull.Value ? reader["OtherInfo"].ToString() : string.Empty
            };
        }

        /// <summary>
        /// Binds the SPORTSBOOK.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The object of SPORTSBOOK.
        /// </returns>
        private Sportsbook BindSportsbookDetails(IDataReader reader, GetSportsbookResultDetailsCriteria criteria)
        {
            criteria.SportType = reader["sporttype"] != DBNull.Value ? Convert.ToInt32(reader["sporttype"]) : 0;

            var sportsbook = new Sportsbook
            {
                HomeId = reader["homeID"] != DBNull.Value ? Convert.ToInt32(reader["homeID"]) : 0,
                AwayId = reader["awayID"] != DBNull.Value ? Convert.ToInt32(reader["awayID"]) : 0,
                HtHomeScore = reader["hthomescore"] != DBNull.Value ? Convert.ToInt32(reader["hthomescore"]) : new int?(),
                FinalHomeScore = reader["finalhomescore"] != DBNull.Value ? Convert.ToInt32(reader["finalhomescore"]) : new int?(),
                HtAwayScore = reader["htawayscore"] != DBNull.Value ? Convert.ToInt32(reader["htawayscore"]) : new int?(),
                FinalAwayScore = reader["finalawayscore"] != DBNull.Value ? Convert.ToInt32(reader["finalawayscore"]) : new int?(),
                EventStatus = reader["eventstatus"] != DBNull.Value ? Convert.ToString(reader["eventstatus"]) : string.Empty,
                KickOffTime = reader["kickofftime"] != DBNull.Value ? Convert.ToDateTime(reader["kickofftime"].ToString()) : DateTime.MinValue,
                EventDate = reader["eventdate"] != DBNull.Value ? Convert.ToDateTime(reader["eventdate"].ToString()) : DateTime.MinValue,
                IsNeutral = reader["IsNeutral"] != DBNull.Value ? Convert.ToBoolean(reader["IsNeutral"]) : false,
                BetStatus = reader["betstatus"] != DBNull.Value ? Convert.ToString(reader["betstatus"]) : string.Empty,
                LeagueId = reader["LeagueID"] != DBNull.Value ? Convert.ToInt32(reader["LeagueID"]) : 0,
                MatchId = criteria.MatchId,
                Color = string.Empty,
                MatchCode = reader["matchcode"] != DBNull.Value ? Convert.ToString(reader["matchcode"]) : string.Empty,
                GoalRecord = reader["goalrecord"] != DBNull.Value ? reader["goalrecord"].ToString() : string.Empty,
                IsHTCal = reader["IsHTCal"] != DBNull.Value ? Convert.ToBoolean(reader["IsHTCal"]) : false,
                Calculate = reader["Calculate"] != DBNull.Value ? Convert.ToBoolean(reader["Calculate"]) : false,
                RefNo = string.Empty,
                BetID = criteria.BetId.ToString(),
                Bettype = criteria.BetType,
                GameDetails = new List<GameDetail>(),
                Handicap2 = criteria.Handicap2,
                SettlementStatus = reader["SettlementStatus"] != DBNull.Value ? Convert.ToByte(reader["SettlementStatus"]) : default(byte)
            };

            if (ColumnExisted(reader, "livehomescore") && ColumnExisted(reader, "liveawayscore"))
            {
                sportsbook.LiveHomeScore = reader["livehomescore"] != DBNull.Value ? Convert.ToInt32(reader["livehomescore"]) : new int?();
                sportsbook.LiveAwayScore = reader["liveawayscore"] != DBNull.Value ? Convert.ToInt32(reader["liveawayscore"]) : new int?();
            }

            return sportsbook;
        }

        private bool ColumnExisted(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
