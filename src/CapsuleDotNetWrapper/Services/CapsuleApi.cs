using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace CapsuleDotNetWrapper.Services
{
    public class CapsuleApi
    {
        readonly string BaseUrl;
        readonly string _authtoken;
  

        public CapsuleApi(string authToken, string url)
        {
            _authtoken = authToken;
            BaseUrl = url;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = BaseUrl;
            client.Authenticator = new HttpBasicAuthenticator(_authtoken,"x");
              
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response from Capsule CRM API.  Check inner details for more info.";
                var apiException = new ApplicationException(message, response.ErrorException);
                throw apiException;
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string message = "Error retrieving response from Capsule CRM API. " + response.StatusCode.ToString() + " - " + response.StatusDescription;

                var apiException = new ApplicationException(message, response.ErrorException);
                throw apiException;
            }
            return response.Data;
        }     
    }
}