using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sarcasm
{
    public class ResourceCounter
    {
        private readonly object lockObject = new object();

        private readonly int totalNumberOfResources;

        public int TotalNumberOfResources { get { return totalNumberOfResources; } }
        public int FreeNumberOfResources { get; private set; }
        public int AcquiredNumberOfResources { get { return TotalNumberOfResources - FreeNumberOfResources; } }

        public ResourceCounter(int totalNumberOfResources, int initialAcquiredNumberOfResources = 0)
        {
            this.totalNumberOfResources = totalNumberOfResources;
            this.FreeNumberOfResources = totalNumberOfResources - initialAcquiredNumberOfResources;
        }

        public bool TryAcquire(int numberOfResourcesToAcquire)
        {
            if (numberOfResourcesToAcquire <= 0)
                throw new ArgumentOutOfRangeException("numberOfResourcesToAcquire", "numberOfResourcesToAcquire should be positive");

            lock (lockObject)
            {
                if (numberOfResourcesToAcquire <= FreeNumberOfResources)
                {
                    FreeNumberOfResources -= numberOfResourcesToAcquire;
                    return true;
                }
                else
                    return false;
            }
        }

        public bool TryAcquireOrLess(int numberOfResourcesToAcquireIdeally, out int numberOfResourcesAcquiredActually)
        {
            if (numberOfResourcesToAcquireIdeally <= 0)
                throw new ArgumentOutOfRangeException("numberOfResourcesToAcquire", "numberOfResourcesToAcquire should be positive");

            lock (lockObject)
            {
                if (FreeNumberOfResources > 0)
                {
                    numberOfResourcesAcquiredActually = Math.Min(numberOfResourcesToAcquireIdeally, FreeNumberOfResources);
                    FreeNumberOfResources -= numberOfResourcesAcquiredActually;
                    return true;
                }
                else
                {
                    numberOfResourcesAcquiredActually = 0;
                    return false;
                }
            }
        }

        public void Release(int numberOfResourcesToRelease)
        {
            if (numberOfResourcesToRelease <= 0)
                throw new ArgumentOutOfRangeException("numberOfResourcesToAcquire", "numberOfResourcesToAcquire should be positive");

            lock (lockObject)
            {
                if (numberOfResourcesToRelease > AcquiredNumberOfResources)
                    throw new ArgumentOutOfRangeException("numberOfResourcesToAcquire", "numberOfResourcesToAcquire should not be more than AcquiredNumberOfResources");

                FreeNumberOfResources += numberOfResourcesToRelease;
            }
        }
    }
}
