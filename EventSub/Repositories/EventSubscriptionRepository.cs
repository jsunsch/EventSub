using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using EventSub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSub.Repositories
{
    class EventSubscriptionRepository : IEventSubscriptionRepository
    {
        private static readonly string _eventSubscriptionTableName = "LiveEventSubscriptions";

        private readonly AmazonDynamoDBClient _amazonDynamoDBClient;

        public EventSubscriptionRepository()
        {
            _amazonDynamoDBClient = new AmazonDynamoDBClient();
        }

        public IEnumerable<LiveEventSubscription> GetEventSubscriptions(Guid eventId)
        {
            Table table = Table.LoadTable(_amazonDynamoDBClient, _eventSubscriptionTableName);

            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("LiveEventId", ScanOperator.Equal, eventId);
            var scan = table.Scan(scanFilter);

            var liveEvents = scan.GetRemaining()
                    .Select(doc => ParseDynamoDbDataEntry(doc))
                        .ToList();

            return liveEvents;
        }

        public Guid Subscribe(Guid eventId, LiveEventSubscription subscriptionData)
        {
            Table table = Table.LoadTable(_amazonDynamoDBClient, _eventSubscriptionTableName);

            var subscriptionGuid = Guid.NewGuid();

            Document newEvent = new Document
            {
                ["Id"] = subscriptionGuid,
                ["LiveEventId"] = eventId,
                ["Email"] = subscriptionData.Email,
                ["Name"] = subscriptionData.Name,
                ["LastName"] = subscriptionData.LastName
            };

            subscriptionData.Data.Where(d => d.Key != "Id" && d.Key != "LiveEventId")
                .ToList()
                    .ForEach(kv => newEvent[kv.Key] = kv.Value);

            table.PutItem(newEvent);

            return subscriptionGuid;
        }

        public void UnSubscribe(Guid eventId, IUserIdentifier userIdentifier)
        {
            Table table = Table.LoadTable(_amazonDynamoDBClient, _eventSubscriptionTableName);

            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("LiveEventId", ScanOperator.Equal, eventId);
            scanFilter.AddCondition("Email", ScanOperator.Equal, userIdentifier.Email);

            var scan = table.Scan(scanFilter);

            var subscriptions = scan.GetRemaining().Select(doc => doc["Id"].AsString());

            foreach(var subscriptionGuid in subscriptions)
            {
                var request = new DeleteItemRequest
                {
                    TableName = _eventSubscriptionTableName,
                    Key = new Dictionary<string, AttributeValue>() {
                        { "Id", new AttributeValue { S = subscriptionGuid } },
                        { "LiveEventId", new AttributeValue { S = eventId.ToString() } }
                    }
                };

                _amazonDynamoDBClient.DeleteItem(request);
            }

            // First find the subscription ID, then delete by ID
            //https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/LowLevelDotNetItemCRUD.html#DeleteMidLevelDotNet
        }

        #region Helpers

        private LiveEventSubscription ParseDynamoDbDataEntry(Document document)
        {
            var liveEvent = new LiveEventSubscription
            {
                LiveEventId = document["LiveEventId"].AsGuid(),
                Email = document["Email"].AsString(),
                Name = document["Name"].AsString(),
                LastName = document["LastName"].AsString(),
                Data = document.ToDictionary(e => e.Key, e => e.Value.AsString())
            };

            return liveEvent;
        }

        #endregion Helpers
    }
}
