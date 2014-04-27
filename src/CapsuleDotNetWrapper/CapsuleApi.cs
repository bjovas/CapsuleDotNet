using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using log4net;

namespace CapsuleDotNetWrapper.Services
{
    public class CapsuleApi
    {
        private static ILog log = LogManager.GetLogger("CapsuleApiBase");

        readonly string BaseUrl;
        readonly string _authtoken;
  

        public CapsuleApi(string authToken, string url)
        {
            _authtoken = authToken;
            BaseUrl = url;

            ThrowExceptionIfConfigIsEmpty(authToken, url);
        }

       
        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            client.Authenticator = new HttpBasicAuthenticator(_authtoken,"x");
         
            if (request.Method == Method.POST)
                client.FollowRedirects = false;
            
            var response = client.Execute<T>(request);
            LogRequest(request, client);
            LogResponse<T>(response);

            ErrorHandling<T>(response);
            return response.Data;
        }


        /// <summary>
        /// Using this method when doing HTTP POST, because the CapsuleCRM only returns a location header, no body.
        /// Method does a second request following the location header url to retrieve the newly created row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public T CreateExecute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            client.Authenticator = new HttpBasicAuthenticator(_authtoken, "x");

            if (request.Method == Method.POST)
                client.FollowRedirects = false;

            var response = client.Execute<T>(request);
            LogRequest(request, client);

            string locationUrl = (string)response.Headers.Where(h => h.Type == ParameterType.HttpHeader && h.Name == "Location").SingleOrDefault().Value;
            int id;
            if (int.TryParse(locationUrl.Remove(0, string.Format("{0}{1}", client.BaseUrl, request.Resource).Length), out id))
            {
                
                var newresource = locationUrl.Remove(0, string.Format("{0}", client.BaseUrl).Length);
                log.Debug("Doing a second request to get the newly created row");
                var secondRequest = new RestRequest();
                secondRequest.Resource = newresource;
                secondRequest.RootElement = request.RootElement;

                return Execute<T>(secondRequest);
            }
            else
            {
                throw new ApplicationException("Could not get ID of newly created row");
            }
            
            
            ErrorHandling<T>(response);
            return response.Data;
        }

        private static void LogRequest(RestRequest request, RestClient client)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("{0} request to {1}{2}", request.Method, client.BaseUrl, request.Resource);
            //    if (request.Method == Method.POST || request.Method == Method.PUT)
              //      log.DebugFormat("Body: ", request.Parameters.Select(p => p.Type == ParameterType.RequestBody).Single().ToString());
            }
        }

        private static void LogResponse<T>(IRestResponse<T> response)  where T : new()
        {
            log.Debug("Response content:");
            log.Debug(response.Content);
        }

        private static void ErrorHandling<T>(IRestResponse<T> response) where T : new()
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                log.Debug("Seems promising :) ");
            }
            else if (response.ErrorException != null)
            {
                const string message = "Error retrieving response from Capsule CRM API.  Check inner details for more info.";
                log.Error(message);
                var apiException = new ApplicationException(message, response.ErrorException);
                throw apiException;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                     response.StatusCode == System.Net.HttpStatusCode.Forbidden ||
                      response.StatusCode == System.Net.HttpStatusCode.InternalServerError ||
                     response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
            {
                string message = "HTTP Errorcode in response from Capsule CRM API. " + response.StatusCode.ToString() + " - " + response.StatusDescription;
                log.Error(message);
                var apiException = new ApplicationException(message, response.ErrorException);
                throw apiException;
            }
            else
                throw new NotImplementedException("Unexpected HTTP statuscode");
        }

        private static void ThrowExceptionIfConfigIsEmpty(string authToken, string url)
        {
            if (string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(url))
                throw new ArgumentException("Config values was null or empty");
        }
    }
}