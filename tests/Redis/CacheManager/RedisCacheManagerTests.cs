using Moq;
using Nebula.Caching.Redis.CacheManager;
using StackExchange.Redis;
using Xunit;

namespace Nebula.Caching.tests.Redis.CacheManager
{
    public class RedisCacheManagerTests
    {
        [Fact]
        public void Given_AMethodExecutedWithRedisCacheAttribute_When_CacheDoesExist_Then_ReturnTrueBecauseCacheExists()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            mockedIDatabase.Setup(m => m.StringGet("key", It.IsAny<CommandFlags>())).Returns("value");
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);
            var expectedCacheExistence = true;

            //Act
            var cacheExists = redisCacheManager.CacheExists("key");

            //Assert
            Assert.Equal(expectedCacheExistence, cacheExists);
            mockedIDatabase.Verify(m => m.StringGet("key", It.IsAny<CommandFlags>()), Times.Once);
        }


        [Fact]
        public void Given_AMethodExecutedWithRedisCacheAttribute_When_CacheDoesNotExist_Then_ReturnFalseBecauseCacheDoesNotExist()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            mockedIDatabase.Setup(m => m.StringGet("key", It.IsAny<CommandFlags>())).Returns(RedisValue.Null);
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);
            var expectedCacheExistence = false;

            //Act
            var cacheExists = redisCacheManager.CacheExists("key");

            //Assert
            Assert.Equal(expectedCacheExistence, cacheExists);
            mockedIDatabase.Verify(m => m.StringGet("key", It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public void Given_AMethodExecutedWithRedisCacheAttribute_When_CacheExistsAndWillBeRetrieved_Then_CacheValueWillBeReturned()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            mockedIDatabase.Setup(m => m.StringGet("key", It.IsAny<CommandFlags>())).Returns("value");
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);
            var expectedCacheValue = "value";

            //Act
            var cacheValue = redisCacheManager.Get("key");

            //Assert
            Assert.Equal(expectedCacheValue, cacheValue);
            mockedIDatabase.Verify(m => m.StringGet("key", It.IsAny<CommandFlags>()), Times.Once);
        }

    }
}