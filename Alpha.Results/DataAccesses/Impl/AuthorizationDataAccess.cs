namespace Alpha.Results.Site.DataAccesses.Impl
{
    using System;
    using Fanex.Data.Repository;
    using Models;

    public class AuthorizationDataAccess : IAuthorizationDataAccess
    {
        public BelongingInfo IsBelonging(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                return new BelongingInfo(0, false);
            }

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new BelongingInfo(
                    Convert.ToByte(outputParams["roleid"]),
                    Convert.ToBoolean(outputParams["isactive"]));
            }
        }
    }
}