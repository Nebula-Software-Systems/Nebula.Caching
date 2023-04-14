using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using Moq;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.KeyManager;
using Redis.Settings;
using Xunit;

namespace Nebula.Caching.tests.Common.Utils
{
    public class ContextUtilsTests
    {

        [Theory]
        [MemberData(nameof(ValidGenericParamNames))]
        public void Given_AParameterName_When_GenericParameterForConfigIsNeeded_Then_ReturnGenericParamAppropriateForConfig(string paramName, string expectedGenericConfigCacheParameter)
        {
            //Arrange
            var contextUtils = new ContextUtils(It.IsAny<IKeyManager>(), It.IsAny<IConfiguration>(), It.IsAny<BaseOptions>());

            //Act
            var generatedGenericConfigCacheParameter = contextUtils.GenerateGeneriConfigCacheParameter(paramName);

            //Assert
            Assert.Equal(expectedGenericConfigCacheParameter, generatedGenericConfigCacheParameter);
        }


        [Fact]
        public void Given_ACacheExpirationValue_When_CacheExpirationIsValid_Then_ReturnTrue()
        {
            //Arrange
            var contextUtils = new ContextUtils(It.IsAny<IKeyManager>(), It.IsAny<IConfiguration>(), It.IsAny<BaseOptions>());
            var validExpiration = TimeSpan.FromSeconds(30);

            //Act
            var isExpirationValid = contextUtils.IsCacheExpirationValid(validExpiration);

            //Assert
            Assert.True(isExpirationValid);
        }

        [Fact]
        public void Given_ACacheExpirationValue_When_CacheExpirationIsNull_Then_ReturnFalse()
        {
            //Arrange
            var contextUtils = new ContextUtils(It.IsAny<IKeyManager>(), It.IsAny<IConfiguration>(), It.IsAny<BaseOptions>());
            TimeSpan? nullExpirationValue = null;

            //Act
            var isExpirationValid = contextUtils.IsCacheExpirationValid(nullExpirationValue);

            //Assert
            Assert.False(isExpirationValid);
        }

        [Fact]
        public void Given_ACacheExpirationValue_When_CacheExpirationIsZero_Then_ReturnFalse()
        {
            //Arrange
            var contextUtils = new ContextUtils(It.IsAny<IKeyManager>(), It.IsAny<IConfiguration>(), It.IsAny<BaseOptions>());
            var validExpiration = TimeSpan.FromSeconds(0);

            //Act
            var isExpirationValid = contextUtils.IsCacheExpirationValid(validExpiration);

            //Assert
            Assert.False(isExpirationValid);
        }

        [Fact]
        public void Given_ARequestToCheckforCacheConfigSection_When_CacheConfigSectionExists_Then_ReturnTrue()
        {
            //Arrange
            var baseOptions = new RedisOptions();
            baseOptions.CacheSettings = new ConcurrentDictionary<string, TimeSpan>();
            var contextUtils = new ContextUtils(It.IsAny<IKeyManager>(), It.IsAny<IConfiguration>(), baseOptions);

            //Act
            var cacheConfigSectionExists = contextUtils.CacheConfigSectionExists();

            //Assert
            Assert.True(cacheConfigSectionExists);
        }

        [Fact]
        public void Given_ARequestToCheckforCacheConfigSection_When_CacheConfigSectionDoesNotExist_Then_ReturnFalse()
        {
            //Arrange
            var baseOptions = new RedisOptions();
            var contextUtils = new ContextUtils(It.IsAny<IKeyManager>(), It.IsAny<IConfiguration>(), baseOptions);

            //Act
            var cacheConfigSectionExists = contextUtils.CacheConfigSectionExists();

            //Assert
            Assert.False(cacheConfigSectionExists);
        }

        //Unit test data
        public static IEnumerable<object[]> ValidGenericParamNames
        {
            get
            {
                return new List<object[]>
                        {
                            new object[] {"paramName1", "{paramName1}"},
                            new object[] {"paramName2", "{paramName2}"},
                            new object[] {"aVeryLongParamNameWithNoMeaning", "{aVeryLongParamNameWithNoMeaning}"}
                        };
            }
        }

    }
}