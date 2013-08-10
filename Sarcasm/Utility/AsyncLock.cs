using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sarcasm.Utility
{
    public class AsyncLock
    {
        private readonly AsyncSemaphore m_semaphore;
        private readonly Task<Releaser> m_releaser;

        public AsyncLock()
        {
            m_semaphore = new AsyncSemaphore(1);
#if NET4_0
            m_releaser = new Task<Releaser>(() => new Releaser(this));
#else
            m_releaser = Task.FromResult(new Releaser(this));
#endif
        }

        public Task<Releaser> LockAsync()
        {
            var wait = m_semaphore.WaitAsync();

            return wait.IsCompleted
                ? m_releaser
                : wait.ContinueWith(
                    _ => new Releaser(this),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default
                    );
        }

        public struct Releaser : IDisposable
        {
            private readonly AsyncLock m_toRelease;

            internal Releaser(AsyncLock toRelease) { m_toRelease = toRelease; }

            public void Dispose()
            {
                if (m_toRelease != null)
                    m_toRelease.m_semaphore.Release();
            }
        }
    }
}
