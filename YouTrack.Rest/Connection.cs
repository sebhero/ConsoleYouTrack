using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;
using YouTrack.Rest.Exceptions;
using YouTrack.Rest.Factories;
using YouTrack.Rest.Requests;

namespace YouTrack.Rest
{
    class Connection : IConnection
    {
        private readonly IRestClient restClient;
        private readonly ISession session;
        private readonly IRestFileRequestFactory requestFactory;

        public Connection(IRestClient restClient, ISession session, IRestFileRequestFactory requestFactory)
        {
            this.restClient = restClient;
            this.session = session;
            this.requestFactory = requestFactory;
        }

        private IRestResponse ExecuteRequest(IYouTrackRequest request, Method method)
        {
            IRestRequest restRequest = requestFactory.CreateRestRequest(request, session, method);
            IRestResponse restResponse = restClient.Execute(restRequest);

            ThrowIfRequestFailed(restResponse);

            return restResponse;
        }

        private IRestResponse ExecuteRequestWithFile(IYouTrackFileRequest request, Method method)
        {
            IRestRequest restRequest = requestFactory.CreateRestRequestWithFile(request, session, method);
            IRestResponse restResponse = restClient.Execute(restRequest);

            ThrowIfRequestFailed(restResponse);

            return restResponse;
        }


        private IRestResponse<TResponse> ExecuteRequest<TResponse>(IYouTrackRequest request, Method method) where TResponse : new()
        {
            IRestRequest restRequest = requestFactory.CreateRestRequest(request, session, method);
            IRestResponse<TResponse> restResponse = restClient.Execute<TResponse>(restRequest);

            ThrowIfRequestFailed(restResponse);

            return restResponse;
        }

        private void ThrowIfRequestFailed(IRestResponse response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotAcceptable:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.UnsupportedMediaType:
                    throw new RequestFailedException(response);

                case HttpStatusCode.NotFound:
                    throw new RequestNotFoundException(response);
            }
        }

        public string Put(IYouTrackPutRequest request)
        {
            IRestResponse response = ExecuteRequestWithAuthentication(request, Method.PUT);

            return GetLocationHeaderValue(response);
        }

        public TResponse Get<TResponse>(IYouTrackGetRequest request) where TResponse : new()
        {
            IRestResponse<TResponse> response = ExecuteRequestWithAuthentication<TResponse>(request, Method.GET);

            return response.Data;
        }

        public void Get(IYouTrackGetRequest request)
        {
            ExecuteRequestWithAuthentication(request, Method.GET);
        }

        public void Delete(IYouTrackDeleteRequest request)
        {
            ExecuteRequestWithAuthentication(request, Method.DELETE);
        }

        public void Post(IYouTrackPostRequest request)
        {
            ExecuteRequestWithAuthentication(request, Method.POST);
        }

        public void PostWithFile(IYouTrackPostWithFileRequest request)
        {
            ExecuteRequestWithAuthenticationAndFile(request, Method.POST);
        }

        private string GetLocationHeaderValue(IRestResponse response)
        {
            Func<Parameter, bool> locationPredicate = h => h.Name.ToLowerInvariant() == "location";

            ThrowIfHeaderCountInvalid(response, locationPredicate);

            return response.Headers.Single(locationPredicate).Value.ToString();
        }

        private void ThrowIfHeaderCountInvalid(IRestResponse response, Func<Parameter, bool> locationPredicate)
        {
            if (response.Headers == null)
            {
                throw new ArgumentNullException("response.Headers", "Response Headers are null.");
            }

            if (response.Headers.Count(locationPredicate) != 1)
            {
                throw new LocationHeaderCountInvalidException(response.Headers.Count(locationPredicate));
            }
        }

        private IRestResponse ExecuteRequestWithAuthentication(IYouTrackRequest request, Method method)
        {
            LoginIfNotAuthenticated();

            return ExecuteRequest(request, method);
        }

        private IRestResponse ExecuteRequestWithAuthenticationAndFile(IYouTrackFileRequest request, Method method)
        {
            LoginIfNotAuthenticated();

            return ExecuteRequestWithFile(request, method);
        }

        private IRestResponse<TResponse> ExecuteRequestWithAuthentication<TResponse>(IYouTrackRequest request, Method method) where TResponse : new()
        {
            LoginIfNotAuthenticated();

            return ExecuteRequest<TResponse>(request, method);
        }


        private void LoginIfNotAuthenticated()
        {
            if (!session.IsAuthenticated)
            {
                session.Login();
            }
        }
    }
}