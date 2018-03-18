using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EventSub.Models
{
    public class LiveEventSubscription : UserIdentifier
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid LiveEventId { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}