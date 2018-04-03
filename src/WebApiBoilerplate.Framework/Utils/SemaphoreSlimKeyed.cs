using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WebApiBoilerplate.Framework.Utils
{
    public class SemaphoreSlimKeyed
    {
        private readonly ConcurrentDictionary<string, Lazy<SemaphoreSlim>> _semaphoreDic = new ConcurrentDictionary<string, Lazy<SemaphoreSlim>>();

        [NotNull]
        private SemaphoreSlim GetSlim([NotNull] string key)
        {
            var slim = _semaphoreDic.GetOrAdd(key, _ => new Lazy<SemaphoreSlim>(() => new SemaphoreSlim(initialCount: 1)));
            return slim.Value;
        }

        [ItemNotNull]
        public async Task<IDisposable> WaitAsync([NotNull] string key)
        {
            var slim = GetSlim(key);

            await slim.WaitAsync().ConfigureAwait(false);

            return new Releaser(slim);
        }

        [NotNull]
        public IDisposable Wait([NotNull] string key)
        {
            var slim = GetSlim(key);

            slim.Wait();

            return new Releaser(slim);
        }

        private struct Releaser : IDisposable
        {
            private readonly SemaphoreSlim _semaphoreSlim;

            public Releaser([NotNull] SemaphoreSlim semaphoreSlim)
            {
                _semaphoreSlim = semaphoreSlim;
            }

            public void Dispose()
            {
                _semaphoreSlim.Release();
            }
        }
    }
}
