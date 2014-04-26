using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using log4net;

namespace CapsuleDotNetWrapper.Services
{
    public class PersonService : CapsuleApi
    {
        private static ILog log = LogManager.GetLogger("Default");

        public PersonService(string authToken, string url) : base(authToken, url) { }

        public Person GetPerson(string personId)
        {
            var request = new RestRequest();
            request.Resource = "/api/party/" + personId;
            request.RootElement = "person";

            return Execute<Person>(request);
        }
    }
}