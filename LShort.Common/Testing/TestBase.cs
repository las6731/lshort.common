using AutoFixture;
using AutoFixture.AutoFakeItEasy;

namespace LShort.Common.Testing
{
    public abstract class TestBase
    {
        protected IFixture fixture;

        public TestBase()
        {
            fixture = new Fixture()
                .Customize(new AutoFakeItEasyCustomization());
        }
    }
}