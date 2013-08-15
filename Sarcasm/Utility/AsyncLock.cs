#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

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

#if NET4_0
        public Task<Releaser> LockAsync()
        {
            Task wait = m_semaphore.WaitAsync();

            return wait.IsCompleted
                ? m_releaser
                : wait.ContinueWith(
                    _ => new Releaser(this),
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default
                    );
        }
#else
        public async Task<Releaser> LockAsync()
        {
            Task wait = m_semaphore.WaitAsync();

            if (wait.IsCompleted)
                return await m_releaser;
            else
            {
                await wait;
                return new Releaser(this);
            }
        }
#endif

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
