using EventSub.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Amazon.DynamoDBv2;
using NLog;
using EventSub.Repositories;

namespace EventSub.Controllers
{
    /// <summary>
    /// A stateless service, but given RPC-style action names.
    /// The service allows administrators to create events, retrieve lists of events
    /// as well as specific events.
    /// </summary>
    public class EventsController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly ILiveEventRepository _eventRepository;

        public EventsController(ILiveEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        /// <summary>
        /// Retrieves the list of all events found in the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LiveEvent> GetEvents()
        {
            try
            {
                return _eventRepository.GetLiveEvents();
            }
            catch (Exception e)
            {
                _logger.Error(e, "An error occurred");

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Retrieves a single event based on the given Event ID.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public LiveEvent GetEvent(Guid eventId)
        {
            LiveEvent liveEvent;
            try
            {
                liveEvent = _eventRepository.GetLiveEvent(eventId);
            }
            catch (Exception e)
            {
                _logger.Error(e, "An error occurred");

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            if(liveEvent == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return liveEvent;
        }

        /// <summary>
        /// Creates an event, and returns the newly created event's ID.
        /// Currently only the Name member in the eventData parameter is required.
        /// If the given data is not valid, a message will be returned stating so.
        /// </summary>
        /// <param name="eventData">The Name member is required.</param>
        /// <returns>The new event's ID</returns>
        public HttpResponseMessage CreateEvent([FromBody]LiveEvent eventData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var eventId = _eventRepository.CreateEvent(eventData);
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new JsonContent(new
                        {
                            Id = eventId
                        })
                    };
                }
                else
                {
                    return ActionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "An error occurred");

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}