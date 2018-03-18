using System;
using System.Collections.Generic;
using EventSub.Models;

namespace EventSub.Repositories
{
    /// <summary>
    /// This class can be moved to a separate Test project, whereby we can
    /// run tests automatically.
    /// </summary>
    public class EventSubscriptionTestRepository : IEventSubscriptionRepository
    {
        public IEnumerable<LiveEventSubscription> GetEventSubscriptions(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Guid Subscribe(Guid eventId, LiveEventSubscription subscriptionData)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribe(Guid eventId, IUserIdentifier userIdentifier)
        {
            throw new NotImplementedException();
        }
    }
}