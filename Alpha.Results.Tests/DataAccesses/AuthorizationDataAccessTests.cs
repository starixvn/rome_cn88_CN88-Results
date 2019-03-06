namespace Alpha.Results.Site.Tests.DataAccesses
{
    using System.Collections;
    using Fanex.Data;
    using NSubstitute;
    using NUnit.Framework;
    using Site.Criteria;
    using Site.DataAccesses;
    using Site.DataAccesses.Impl;

    [TestFixture]
    public class AuthorizationDataAccessTests
    {
        private const int ActorIdTest = 8;
        private const int CustIdTest = 1176092;
        private const int SubAccountIdTest = 0;
        private const int TransIdTest = 0;

        private IObjectDb objectDb;
        private IAuthorizationDataAccess dataAccess;

        [SetUp]
        public void SetUp()
        {
            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.CreateInstance = (settingKey) => objectDb;

            dataAccess = new AuthorizationDataAccess();
        }

        [Test]
        public void IsBelonging_ValidCriteria_ReturnsTrue()
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(ActorIdTest, CustIdTest, SubAccountIdTest, TransIdTest);
            var expectedData = new Hashtable
            {
                ["roleid"] = 1,
                ["isactive"] = true
            };

            objectDb.ExecuteNonQuery(criteria.GetSpParams()).Returns(0);
            objectDb.GetOuputParams().Returns(expectedData);

            // Act
            var actualData = dataAccess.IsBelonging(criteria);

            // Assert
            Assert.True(actualData.RoleId == 1 && actualData.IsActive);
        }

        [Test]
        public void IsBelonging_ValidCriteria_ReturnsFalse()
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(ActorIdTest, CustIdTest, SubAccountIdTest, TransIdTest);
            var expectedData = new Hashtable
            {
                ["roleid"] = 2,
                ["isactive"] = false
            };

            objectDb.ExecuteNonQuery(criteria.GetSpParams()).Returns(0);
            objectDb.GetOuputParams().Returns(expectedData);

            // Act
            var actualData = dataAccess.IsBelonging(criteria);

            // Assert
            Assert.True(actualData.RoleId == 2 && !actualData.IsActive);
        }

        [Test]
        public void IsBelonging_InvalidCriteria_ReturnsFalse()
        {
            // Arrange
            var criteria = new CheckBelongingCriteria(-1, -1, -1, TransIdTest);

            // Act
            var actualData = dataAccess.IsBelonging(criteria);

            // Assert
            Assert.True(actualData.RoleId == 0 && !actualData.IsActive);
        }
    }
}
