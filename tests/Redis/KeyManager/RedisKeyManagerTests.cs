using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using Nebula.Caching.Common.Constants;
using Nebula.Caching.Redis.KeyManager;
using Xunit;

namespace Nebula.Caching.tests.Redis.KeyManager
{
    public class RedisKeyManagerTests
    {
        [Fact]
        public void Given_AMethodExecutedWithRedisCacheAttribute_When_NoArgumentsArePassed_Then_CacheKeyOnlyContainsFullPathMethod()
        {
            //Arrrange
            var redisKeyManager = new RedisKeyManager();
            var arguments = new string[] { };
            var mockedMethodInfo = new Mock<MethodInfo>();
            mockedMethodInfo.SetupGet(m => m.DeclaringType.FullName).Returns("my.full.name");
            mockedMethodInfo.SetupGet(m => m.Name).Returns("myMethod");
            var methodInfo = mockedMethodInfo.Object;
            var expectedCacheKey = $"{methodInfo.DeclaringType.FullName}{KeyConstants.MethodAndParametersSeparator}{methodInfo.Name}";

            //Act
            var generatedCacheKey = redisKeyManager.GenerateKey(methodInfo, arguments);

            //Assert
            Assert.NotEmpty(generatedCacheKey);
            Assert.Equal(expectedCacheKey, generatedCacheKey);
        }

        [Theory]
        [MemberData(nameof(ExecutedMethodArguments))]
        public void Given_AMethodExecutedWithRedisCacheAttribute_When_ArgumentsArePassed_Then_CacheKeyShouldContainArguments(string[] methodArguments)
        {
            //Arrrange
            var redisKeyManager = new RedisKeyManager();
            var mockedMethodInfo = new Mock<MethodInfo>();
            mockedMethodInfo.SetupGet(m => m.DeclaringType.FullName).Returns("my.full.name");
            mockedMethodInfo.SetupGet(m => m.Name).Returns("myMethod");
            var methodInfo = mockedMethodInfo.Object;
            string methodParamsAggregated = string.Join(KeyConstants.MethodAndParametersSeparator, methodArguments);
            var expectedCacheKey = $"{methodInfo.DeclaringType.FullName}{KeyConstants.MethodAndParametersSeparator}{methodInfo.Name}{KeyConstants.MethodAndParametersSeparator}{methodParamsAggregated}";

            //Act
            var generatedCacheKey = redisKeyManager.GenerateKey(methodInfo, methodArguments);

            //Assert
            Assert.NotEmpty(generatedCacheKey);
            Assert.Equal(expectedCacheKey, generatedCacheKey);
        }

        [Theory]
        [MemberData(nameof(ValidCacheKeys))]
        public void Given_ACacheKey_When_AConfigKeyTransformationIsNeeded_Then_PerformTransformationFromCacheKeyToConfigKey(string key, string expectedConfigKey)
        {
            //Arrange
            var redisKeyManager = new RedisKeyManager();

            //Act
            var generatedConfigKey = redisKeyManager.ConvertCacheKeyToConfigKey(key);

            //Assert
            Assert.Equal(expectedConfigKey, generatedConfigKey);
        }

        [Theory]
        [MemberData(nameof(ValidGenericParamNames))]
        public void Given_When_Then(string paramName, string expectedGenericConfigCacheParameter)
        {
            //Arrange
            var redisKeyManager = new RedisKeyManager();

            //Act
            var generatedGenericConfigCacheParameter = redisKeyManager.GenerateGeneriConfigCacheParameter(paramName);

            //Assert
            Assert.Equal(expectedGenericConfigCacheParameter, generatedGenericConfigCacheParameter);
        }


        //Unit test data
        public static IEnumerable<object[]> ExecutedMethodArguments
        {
            get
            {
                var values1 = new string[] { "1", "2", "Rafael" };
                var values2 = new string[] { "10", "6", "3" };
                var values3 = new string[] { "Rafael", "Camara" };
                var values4 = new string[] { "a", "Rafael", "true", "123" };
                return new List<object[]>
                        {
                            new object[] { values1 },
                            new object[] { values2 },
                            new object[] { values3 },
                            new object[] { values4 }
                        };
            }
        }
        public static IEnumerable<object[]> ValidCacheKeys
        {
            get
            {
                return new List<object[]>
                        {
                            new object[] {"full.path.to.method:method:param1:param2:param3:param4", "full-path-to-method--method--param1--param2--param3--param4"},
                            new object[] {"a.happy.path.to.method:method", "a-happy-path-to-method--method"},
                            new object[] {"path.to.amazing.method:method:param1", "path-to-amazing-method--method--param1"}
                        };
            }
        }
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