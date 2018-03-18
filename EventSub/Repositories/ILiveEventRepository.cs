using EventSub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSub.Repositories
{
    public interface ILiveEventRepository
    {
        Guid CreateEvent(LiveEvent eventData);
        IEnumerable<LiveEvent> GetLiveEvents();
        LiveEvent GetLiveEvent(Guid eventId);
        void UpdateLiveEvent(Guid eventId, LiveEvent eventData);
    }
}
