using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using CapsuleDotNetWrapper;
using CapsuleDotNetWrapper.Services;
using RandomStringGenerator;
using System.Configuration;

namespace CapsuleDotNetTests
{
    [TestFixture]
    public class OpportunityServiceTests
    {
        [Test]
        public void CreateOpportunity()
        {
            var generator = new StringGenerator();

            var person = new Person()
            {
                id = "60275159",
                firstName = "Ola",
                lastName = "Nordmann"
            };


            var opportunity = new OpportunityToCreate()
            {
                opportunity = new Opportunity()
                {
                    currency = "NOK",
                    milestoneId = "239401",
                    name = string.Format("Intouch demo for {0} - {1} {2}", person.organisationName, person.firstName, person.lastName),  
                    durationBasis = "FIXED"
                }
            };

            var newOpportunity = opportunityService.Create(person.id, opportunity);
            newOpportunity.id.ShouldBeGreaterThan("2175534");
            newOpportunity.currency.ShouldEqual("NOK");
            newOpportunity.name.ShouldContain("Intouch demo for ");
  
        }








        [Test]
        public void Should_get_exception_when_user_config_is_insufficient()
        {
            Assert.That(() => new CapsuleDotNetWrapper.Services.OpportunityService("", "https://someapi.com"), Throws.Exception.TypeOf<ArgumentException>());
        }



        [SetUp]
        public void Setup()
        {
            // Insert a valid API token and Base URL in the app.config file
            opportunityService = new CapsuleDotNetWrapper.Services.OpportunityService(ConfigurationManager.AppSettings["Usertoken"], ConfigurationManager.AppSettings["ApiBaseUrl"]);

            // random string generator used as a crux to make tests a bit easier to follow.
            generator = new StringGenerator();
        }

        OpportunityService opportunityService;
        StringGenerator generator;
    }
}
