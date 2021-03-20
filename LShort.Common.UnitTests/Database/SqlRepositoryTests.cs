using System;
using System.Data;
using AutoFixture;
using FakeItEasy;
using LShort.Common.Database;
using LShort.Common.Database.Attributes;
using LShort.Common.Database.Implementation;
using LShort.Common.Logging;
using LShort.Common.Models;
using LShort.Common.Testing;
using NUnit.Framework;

namespace Lshort.Common.UnitTests.Database
{
    [TestFixture]
    public class SqlRepositoryTests : TestBase
    {
        private class TestModel : ModelBase
        {
            public string StringProp { get; set; }
            public int IntProp { get; set; }
            public Guid GuidProp { get; set; }
            protected string PrivateProp { get; set; }
        }

        [Table("TestModels")]
        private class TestSqlRepository : SqlRepository<TestModel>
        {
            public TestSqlRepository(ISqlDatabase db, IAppLogger logger) : base(db, logger) {}

            /// <summary>
            /// Exposes EnsureSchema so it can be tested.
            /// </summary>
            public void TestEnsureSchema() => EnsureSchema();
        }

        private TestSqlRepository sut;
        private ISqlDatabase db;

        [SetUp]
        public void SetUp()
        {
            db = fixture.Freeze<ISqlDatabase>();

            A.CallTo(() => db.Map(A<Type>._)).Returns("type");

            sut = fixture.Create<TestSqlRepository>();
            
            A.CallTo(() => db.Statement(A<string>._, A<object>._, A<IDbTransaction>._, A<int?>._))
                .MustHaveHappenedOnceExactly();
            
            Fake.ClearRecordedCalls(db);
        }

        [Test]
        public void EnsureSchema_Test()
        {
            // arrange
            var expectedSchema =
@"CREATE TABLE [IF NOT EXISTS] TestModels (
Id uuid PRIMARY KEY,
StringProp type,
IntProp type,
GuidProp type,
);"; // unindented because the whitespace would make the matching fail
            
            // act
            sut.TestEnsureSchema();
            
            // assert
            A.CallTo(() => db.Statement(expectedSchema, A<object>._, A<IDbTransaction>._, A<int?>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}