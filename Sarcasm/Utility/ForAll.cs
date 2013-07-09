using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sarcasm.Utility
{
    public class ForAll<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> items;
        private readonly Action<T> executeBeforeEachIteration;
        private readonly Action executeAfterFinished;

        public ForAll(IEnumerable<T> items, Action<T> executeBeforeEachIteration, Action executeAfterFinished)
        {
            this.items = items;
            this.executeBeforeEachIteration = executeBeforeEachIteration;
            this.executeAfterFinished = executeAfterFinished;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(items.GetEnumerator(), executeBeforeEachIteration, executeAfterFinished);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Enumerator

        private class Enumerator : IEnumerator<T>
        {
            private bool disposed;
            private readonly IEnumerator<T> enumerator;
            private readonly Action<T> executeBeforeEachIteration;
            private readonly Action executeAfterFinished;

            public Enumerator(IEnumerator<T> enumerator, Action<T> executeBeforeEachIteration, Action executeAfterFinished)
            {
                this.enumerator = enumerator;
                this.executeBeforeEachIteration = executeBeforeEachIteration;
                this.executeAfterFinished = executeAfterFinished;
            }

            public T Current
            {
                get { return enumerator.Current; }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                bool moved = enumerator.MoveNext();

                if (moved && executeBeforeEachIteration != null)
                    executeBeforeEachIteration(Current);

                return moved;
            }

            public void Reset()
            {
                enumerator.Reset();
            }

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
        }

        #endregion
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
