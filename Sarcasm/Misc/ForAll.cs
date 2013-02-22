using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sarcasm
{
    public class ForAll<T> : IEnumerable<T>, IDisposable
    {
        private bool disposed;
        private readonly IEnumerable<T> items;
        private readonly Action<T> executeBeforeEachIteration;
        private readonly Action executeAfterFinished;

        public ForAll(IEnumerable<T> items, Action<T> executeBeforeEachIteration, Action executeAfterFinished)
        {
            this.items = items;
            this.executeBeforeEachIteration = executeBeforeEachIteration;
            this.executeAfterFinished = executeAfterFinished;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.executeAfterFinished != null)
                        this.executeAfterFinished();
                }

                this.disposed = true;
            }
        }

        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in items)
            {
                executeBeforeEachIteration(item);
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public static class ForAllExtensions
    {
        public static IEnumerable<T> ForAll<T>(this IEnumerable<T> items, Action<T> executeBeforeEachIteration, Action executeAfterFinished = null)
        {
            return new ForAll<T>(items, executeBeforeEachIteration, executeAfterFinished);
        }

        public static IEnumerable<T> AfterFinished<T>(this IEnumerable<T> items, Action executeAfterFinished)
        {
            return new ForAll<T>(items, executeBeforeEachIteration: null, executeAfterFinished: executeAfterFinished);
        }
    }
}
