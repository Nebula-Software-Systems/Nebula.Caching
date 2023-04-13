using System.Collections.Concurrent;
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

        [Fact]
        public void Given_AMethodExecutedWithRedisCacheAttribute_When_CacheDoesNotExist_Then_SetCache()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            var key = "someKey";
            var value = "someValue";
            var expiration = TimeSpan.FromSeconds(25);
            mockedIDatabase.Setup(m => m.StringSet(key, value, It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>())).Returns(It.IsAny<bool>());
            mockedIDatabase.Setup(m => m.KeyExpire(key, expiration, It.IsAny<ExpireWhen>(), It.IsAny<CommandFlags>())).Returns(It.IsAny<bool>());
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);


            //Act
            redisCacheManager.Set(key, value, expiration);

            //Assert
            mockedIDatabase.Verify(m => m.StringSet(key, value, It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
            mockedIDatabase.Verify(m => m.KeyExpire(key, expiration, It.IsAny<ExpireWhen>(), It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task Given_AMethodExecutedWithRedisCacheAttribute_When_CacheDoesExist_Then_ReturnTrueBecauseCacheExistsAsync()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            mockedIDatabase.Setup(m => m.StringGetAsync("key", It.IsAny<CommandFlags>())).ReturnsAsync("value");
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);
            var expectedCacheExistence = true;

            //Act
            var cacheExists = await redisCacheManager.CacheExistsAsync("key");

            //Assert
            Assert.Equal(expectedCacheExistence, cacheExists);
            mockedIDatabase.Verify(m => m.StringGetAsync("key", It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task Given_AMethodExecutedWithRedisCacheAttribute_When_CacheDoesNotExist_Then_ReturnFalseBecauseCacheDoesNotExistAsync()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            mockedIDatabase.Setup(m => m.StringGetAsync("key", It.IsAny<CommandFlags>())).ReturnsAsync(RedisValue.Null);
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);
            var expectedCacheExistence = false;

            //Act
            var cacheExists = await redisCacheManager.CacheExistsAsync("key");

            //Assert
            Assert.Equal(expectedCacheExistence, cacheExists);
            mockedIDatabase.Verify(m => m.StringGetAsync("key", It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task Given_AMethodExecutedWithRedisCacheAttribute_When_CacheExistsAndWillBeRetrieved_Then_CacheValueWillBeReturnedAsync()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            mockedIDatabase.Setup(m => m.StringGetAsync("key", It.IsAny<CommandFlags>())).ReturnsAsync("value");
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);
            var expectedCacheValue = "value";

            //Act
            var cacheValue = await redisCacheManager.GetAsync("key");

            //Assert
            Assert.Equal(expectedCacheValue, cacheValue);
            mockedIDatabase.Verify(m => m.StringGetAsync("key", It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task Given_AMethodExecutedWithRedisCacheAttribute_When_CacheDoesNotExist_Then_SetCacheAsync()
        {
            //Arrange
            var mockedIDatabase = new Mock<IDatabase>();
            var key = "someKey";
            var value = "someValue";
            var expiration = TimeSpan.FromSeconds(25);
            mockedIDatabase.Setup(m => m.StringSetAsync(key, value, It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>())).ReturnsAsync(It.IsAny<bool>());
            mockedIDatabase.Setup(m => m.KeyExpireAsync(key, expiration, It.IsAny<ExpireWhen>(), It.IsAny<CommandFlags>())).ReturnsAsync(It.IsAny<bool>());
            var redisCacheManager = new RedisCacheManager(mockedIDatabase.Object);


            //Act
            redisCacheManager.SetAsync(key, value, expiration);

            //Assert
            mockedIDatabase.Verify(m => m.StringSetAsync(key, value, It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
            mockedIDatabase.Verify(m => m.KeyExpireAsync(key, expiration, It.IsAny<ExpireWhen>(), It.IsAny<CommandFlags>()), Times.Once);
        }
    }
}