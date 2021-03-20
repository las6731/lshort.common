using System;
using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using LShort.Common.Models;
using LShort.Common.Testing;
using NUnit.Framework;

namespace LShort.Common.UnitTests.Models
{
    [TestFixture]
    [Category("UnitTests")]
    public class LogInfoTests : TestBase
    {
        [Test]
        public void ToDynamicTest()
        {
            var properties = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("Prop1", "Value1"),
                new KeyValuePair<string, object>("Prop2", "Value2")
            };
            var logInfo = fixture.Build<LogInfo>()
                .With(log => log.Properties, properties)
                .Create();

            // act
            dynamic result = logInfo.ToDynamic();
            var resultList = result as IDictionary<string, object>;
            
            // assert
            logInfo.Level.Should().BeEquivalentTo(result.level);
            logInfo.Utc.Should().BeCloseTo(result.utc);
            logInfo.Source.Should().BeEquivalentTo(result.source);
            logInfo.Message.Should().BeEquivalentTo(result.message);
            logInfo.Properties[0].Value.Should().BeEquivalentTo(result.Prop1);
            logInfo.Properties[1].Value.Should().BeEquivalentTo(result.Prop2);
            logInfo.Details.Should().BeEquivalentTo(result.details);
            logInfo.Exception.Should().BeEquivalentTo(result.exception);
            resultList.Should().HaveCount(8);
        }
        
        [Test]
        public void ToDynamicMinimalTest()
        {
            // arrange
            var logInfo = new LogInfo
            {
                Level = "Information",
                Utc = DateTime.UtcNow,
                Message = "Test Message"
            };

            // act
            dynamic result = logInfo.ToDynamic();
            var resultList = result as IDictionary<string, object>;
            
            // assert
            logInfo.Level.Should().BeEquivalentTo(result.level);
            logInfo.Utc.Should().BeCloseTo(result.utc);
            logInfo.Message.Should().BeEquivalentTo(result.message);
            resultList.Should().HaveCount(3);
        }
    }
}