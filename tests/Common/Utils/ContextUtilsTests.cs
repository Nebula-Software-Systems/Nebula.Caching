using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using Moq;
using Nebula.Caching.Common.KeyManager;
using Nebula.Caching.Common.Utils;
using Nebula.Caching.Redis.KeyManager;
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