using EventSub.Models;
using System;
using System.Collections.Generic;

namespace EventSub.Repositories
{
    public interface IEventSubscriptionRepository
    {
        Guid Subscribe(Guid eventId, LiveEventSubscription subscriptionData);
        void UnSubscribe(Guid eventId, IUserIdentifier userIdentifier);
        IEnumerable<LiveEventSubscription> GetEventSubscriptions(Guid eventId);
    }
}
