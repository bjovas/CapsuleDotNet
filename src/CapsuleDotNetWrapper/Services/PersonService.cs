using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace CapsuleDotNetWrapper.Services
{
    public class PersonService : CapsuleApi
    {
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