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
        private static ILog log = LogManager.GetLogger("PersonService");

        public PersonService(string authToken, string url) : base(authToken, url) { }

        public Person Create(PersonToCreate person)
        {
            log.InfoFormat("Creating person: {0} {1}", person.person.firstName, person.person.lastName);
            var request = new RestRequest("/api/person", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.RootElement = "person";
            request.AddBody(person);
            

            return CreateExecute<Person>(request);
        }
        public Person GetPerson(string personId)
        {
            log.InfoFormat("Get person by id: {0}", personId);

            var request = new RestRequest();
            request.Resource = "/api/party/" + personId;
            request.RootElement = "person";

            return Execute<Person>(request);
        }
        public bool Delete(Person person)
        {
            log.InfoFormat("Deleting person id: {0}", person.id);

            var request = new RestRequest();
            request.Method = Method.DELETE;
            request.Resource = "/api/party/" + person.id;
            request.RootElement = "person";


            try
            {
                Execute<Person>(request);
                return true;
            }
            catch (Exception e)
            {
                log.Error("Could not delete person", e);
                return false;
            }
        }
    }
}