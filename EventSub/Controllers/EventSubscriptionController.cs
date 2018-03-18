using EventSub.Models;
using EventSub.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EventSub.Controllers
{
    /// <summary>
    /// A stateless service, but given RPC-style action names.
    /// The service allows end users to sign up/unsubscribe from a 
    /// specific event, and also allows administrators to pull participant lists
    /// from a specific event.
    /// </summary>
    public class EventSubscriptionController : ApiController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly IEventSubscriptionRepository _subscriptionRepository;

        public EventSubscriptionController(IEventSubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        /// <summary>
        /// Subscribe/Sign up to an event. This requires an eventId (Unique event identifier),
        /// and an E-mail, Name and Last name.
        /// If the given data is not valid, a message will be returned stating so.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="subscriptionData">Required member: Email, Name and LastName</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Subscribe(Guid eventId, [FromBody]LiveEventSubscription subscriptionData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subscriptionGuid = _subscriptionRepository.Subscribe(eventId, subscriptionData);

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new JsonContent(new
                        {
                            Id = subscriptionGuid
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

        /// <summary>
        /// Unsubscribes a person from an event based on a <see cref="IUserIdentifier"/>.
        /// If no subscriptions are found, <see cref="HttpStatusCode.OK"/> is still returned.
        /// If the given data is not valid, a message will be returned stating so.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userIdentifier"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UnSubscribe(Guid eventId, [FromBody]UserIdentifier userIdentifier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _subscriptionRepository.UnSubscribe(eventId, userIdentifier);

                    return new HttpResponseMessage(HttpStatusCode.OK);
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

        /// <summary>
        /// Retrieves a list of subscriptions/sign up's based on the given
        /// Event ID.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<LiveEventSubscription> GetEventSubscriptions(Guid eventId)
        {
            try
            {
                return _subscriptionRepository.GetEventSubscriptions(eventId);
            }
            catch (Exception e)
            {
                _logger.Error(e, "An error occurred");

                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        #region GDPR Support
        //public void GdprRequest([FromBody] UserIdentifier userIdentifier)
        //{
        //    // GDPR requests.

        //    // The flow for deleting ones data should be as follows:
        //    //  1) The user makes a GDPR information removal request.
        //    //     We use an e-mail to identify a user.
        //    //
        //    //  2) An confirmation e-mail is sent to the user's e-mail addrress,
        //    //     this will authenticate the user and ensure that person
        //    //     is the owner of the e-mail address. The e-mail will contain
        //    //     a token (in this case a GUID) which is stored for a limited time
        //    //     in a database.
        //    //
        //    //  3) If the user decides to click the link in the e-mail, a request with
        //    //     the e-mail, GUID combination is sent to the service, after which the
        //    //     following data will be deleted:
        //    //          - The event subscription
        //    //          - The e-mail, GUID combination

        //    throw new NotImplementedException();
        //}
        #endregion GDPR Support
    }
}