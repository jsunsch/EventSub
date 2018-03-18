using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EventSub.Models
{
    /// <summary>
    /// A custom Json Converter has been created <see cref="EventSub.Converters.LiveEventConverter"/> which
    /// populates this object with a dynamic dataset, while still supporting the ApiControllers
    /// ModelState validation techniques.
    /// </summary>
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