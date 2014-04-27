using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using log4net;

namespace CapsuleDotNetWrapper.Services
{
    public class OpportunityService : CapsuleApi
    {
        private static ILog log = LogManager.GetLogger("OpportunityService");

        public OpportunityService(string authToken, string url) : base(authToken, url) { }

        public Opportunity Create(string PartyId, OpportunityToCreate opportunity)
        {
            log.InfoFormat("Creating opportunity for party: {0} ", opportunity.opportunity.id);
            var request = new RestRequest(String.Format("/api/party/{0}/opportunity", PartyId), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.RootElement = "opportunity";
            request.AddBody(opportunity);

            return CreateExecute<Opportunity>(request);
        }
    }
}
