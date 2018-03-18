using EventSub.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSub.Converters
{
    /// <summary>
    /// Converts JSON to either <see cref="LiveEvent" /> or <see cref="LiveEventSubscription" />.
    /// This converter populates the given object type, and also fills a dictionary with the
    /// JSON data.
    /// </summary>
    public class LiveEventConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(LiveEvent) == objectType || typeof(LiveEventSubscription) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // We use jObject to create two instances of the reader. This 
            // is because JsonReader acts as a stream, and it cannot have
            // it position reset to 0.
            var jObject = JObject.Load(reader);

            // Serializes all JSON members into a dictionary.
            var dic = serializer.Deserialize<Dictionary<string, string>>(jObject.CreateReader());

            // Serializes all data into class members based on the objectType.
            var obj = existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
            serializer.Populate(jObject.CreateReader(), obj);

            if (objectType == typeof(LiveEvent))
            {
                var liveEvent = (LiveEvent)obj;
                liveEvent.Data = dic;

                return liveEvent;
            }
            else // objectType == typeof(LiveEventSubscription)
            {
                var liveEventSubscription = (LiveEventSubscription)obj;
                liveEventSubscription.Data = dic;

                return liveEventSubscription;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            if (value.GetType() == typeof(LiveEvent))
            {
                var liveEvent = (LiveEvent)value;
                WriteValueToJson(writer, serializer, "Id", liveEvent.Id);
                WriteValueToJson(writer, serializer, "Name", liveEvent.Name);

                var skipProperties = new List<string> { "Id", "Name" };
                foreach (var e in liveEvent.Data)
                    if (!skipProperties.Any(p => string.Equals(p, e.Key, StringComparison.OrdinalIgnoreCase)))
                        WriteValueToJson(writer, serializer, e.Key, e.Value);
            }
            else // value.GetType() == typeof(LiveEventSubscription)
            {
                var liveEventSubscription = (LiveEventSubscription)value;
                WriteValueToJson(writer, serializer, "Email", liveEventSubscription.Email);
                WriteValueToJson(writer, serializer, "LastName", liveEventSubscription.LastName);
                WriteValueToJson(writer, serializer, "LiveEventId", liveEventSubscription.LiveEventId);
                WriteValueToJson(writer, serializer, "Name", liveEventSubscription.Name);

                var skipProperties = new List<string> { "Email", "LastName", "LiveEventId", "Name" };
                foreach (var e in liveEventSubscription.Data)
                    if (!skipProperties.Any(p => string.Equals(p, e.Key, StringComparison.OrdinalIgnoreCase)))
                        WriteValueToJson(writer, serializer, e.Key, e.Value);
            }

            writer.WriteEndObject();
        }

        private void WriteValueToJson(JsonWriter writer, JsonSerializer serializer, string propertyName, object value)
        {
            writer.WritePropertyName(propertyName);
            serializer.Serialize(writer, value);
        }
    }
}