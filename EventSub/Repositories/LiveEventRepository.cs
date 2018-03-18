using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using EventSub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSub.Repositories
{
    class LiveEventRepository : ILiveEventRepository
    {
        private static readonly string _eventTableName = "LiveEvents";

        private readonly AmazonDynamoDBClient _amazonDynamoDBClient;

        public LiveEventRepository()
        {
            _amazonDynamoDBClient = new AmazonDynamoDBClient();
        }

        public Guid CreateEvent(LiveEvent eventData)
        {
            Table table = Table.LoadTable(_amazonDynamoDBClient, _eventTableName);

            var eventId = Guid.NewGuid();

            Document newEvent = new Document
            {
                ["Id"] = eventId,
                ["Name"] = eventData.Name
            };

            eventData.Data.Where(d => d.Key != "Id")
                .ToList()
                    .ForEach(kv => newEvent[kv.Key] = kv.Value);

            table.PutItem(newEvent);

            return eventId;
        }

        public LiveEvent GetLiveEvent(Guid eventId)
        {
            Table table = Table.LoadTable(_amazonDynamoDBClient, _eventTableName);

            // TODO: What happens if the item could not be found?
            // We should return null.
            var document = table.GetItem(eventId);

            return ParseDynamoDbDataEntry(document);
        }

        public IEnumerable<LiveEvent> GetLiveEvents()
        {
            Table table = Table.LoadTable(_amazonDynamoDBClient, _eventTableName);
            var dbSearch = table.Scan(new ScanFilter());

            var liveEvents = dbSearch.GetRemaining()
                    .Select(doc => ParseDynamoDbDataEntry(doc))
                        .ToList();

            return liveEvents;
        }

        public void UpdateLiveEvent(Guid eventId, LiveEvent eventData)
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private LiveEvent ParseDynamoDbDataEntry(Document document)
        {
            var liveEvent = new LiveEvent
            {
                Id = document["Id"].AsGuid(),
                Name = document["Name"].AsString(),
                Data = document.ToDictionary(e => e.Key, e => e.Value.AsString())
            };

            return liveEvent;
        }

        #endregion Helpers
    }
}
