using System;
using System.Collections.Generic;
using EventSub.Models;

namespace EventSub.Repositories
{
    /// <summary>
    /// This class can be moved to a separate Test project, whereby we can
    /// run tests automatically.
    /// </summary>
    public class LiveEventTestRepository : ILiveEventRepository
    {
        public Guid CreateEvent(LiveEvent eventData)
        {
            throw new NotImplementedException();
        }

        public LiveEvent GetLiveEvent(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LiveEvent> GetLiveEvents()
        {
            throw new NotImplementedException();
        }

        public void UpdateLiveEvent(Guid eventId, LiveEvent eventData)
        {
            throw new NotImplementedException();
        }
    }
}