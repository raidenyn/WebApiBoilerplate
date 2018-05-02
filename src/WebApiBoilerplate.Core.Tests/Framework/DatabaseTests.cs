using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebApiBoilerplate.Core.Tests.Framework
{
    [Category("Database")]
    public abstract class DatabaseTests
    {
        private static DatabaseFixture _dbFixuter;

        protected DatabaseTestFixture State { get; private set; }

        [OneTimeSetUp]
        public async Task GlobalSetUpAsync()
        {
            _dbFixuter = await DatabaseFixture.CreateAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public Task GlobalTearDownAsync()
        {
            return _dbFixuter.DisposeAsync();
        }

        [SetUp]
        public void SetUp()
        {
            State = new DatabaseTestFixture(_dbFixuter);
        }

        [TearDown]
        public void TearDown()
        {
            ((IDisposable)State).Dispose();
            State = null;
        }
    }
}
