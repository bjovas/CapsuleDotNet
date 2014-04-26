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

            LogRequest(request, client);
            var response = client.Execute<T>(request);
            LogResponse<T>(response);

            ErrorHandling<T>(response);
            return response.Data;
        }

        private static void LogRequest(RestRequest request, RestClient client)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("{0} request to {1}{2}", request.Method, client.BaseUrl, request.Resource);
                if (request.Method == Method.POST || request.Method == Method.PUT)
                    log.DebugFormat("Body: ", request.Parameters.Select(p => p.Type == ParameterType.RequestBody).Single().ToString());
            }
        }

        private static void LogResponse<T>(IRestResponse<T> response)  where T : new()
        {
            log.Debug("Response content:");
            log.Debug(response.Content);
        }

        private static void ErrorHandling<T>(IRestResponse<T> response) where T : new()
        {
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response from Capsule CRM API.  Check inner details for more info.";
                log.Error(message);
                var apiException = new ApplicationException(message, response.ErrorException);
                throw apiException;
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string message = "HTTP Errorcode in response from Capsule CRM API. " + response.StatusCode.ToString() + " - " + response.StatusDescription;
                log.Error(message);
                var apiException = new ApplicationException(message, response.ErrorException);
                throw apiException;
            }
        }

        private static void ThrowExceptionIfConfigIsEmpty(string authToken, string url)
        {
            if (string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(url))
                throw new ArgumentException("Config values was null or empty");
        }
    }
}