using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using CapsuleDotNetWrapper;
using CapsuleDotNetWrapper.Services;
using RandomStringGenerator;

namespace CapsuleDotNetTests
{
    [TestFixture]
    public class PersonServiceTests
    {
        [Test]
        public void GetPerson()
        {
            var result = personService.GetPerson("60275092");
            result.id.ShouldEqual("60275092");
            result.firstName.ShouldEqual("Bjørn");
            result.lastName.ShouldEqual("Vasbotten");
        }

        [Test]
        public void CreatePerson()
        {
            var generator = new StringGenerator();
          
            var person = new PersonToCreate()
            {
                person = new Person() 
                { 
                    firstName = generator.GenerateString(6), 
                    lastName = generator.GenerateString(10),
                    contacts = new Contacts()
                    {
                        email = new Email()
                        {
                            type = "Work",
                            emailAddress = "b@vasbotten.net"
                        },
                        phone = new Phone()
                        {
                            type = "Work",
                            phoneNumber = "12345678"
                        }

                    }
                }
                    
            };

            var result = personService.Create(person);
            result.id.ShouldBeGreaterThan("60275159");
            result.firstName.ShouldEqual(person.person.firstName);
            result.lastName.ShouldEqual(person.person.lastName);
        }


        [Test]
        public void CreatePersonWithOrganization()
        {
            var generator = new StringGenerator();

            var person = new PersonToCreate()
            {
                person = new Person()
                {
                    firstName = generator.GenerateString(6),
                    lastName = generator.GenerateString(10),
                    organisationName = "PSWinCom1 AS"
                }
            };

            var result = personService.Create(person);
            result.id.ShouldBeGreaterThan("60275159");
            result.firstName.ShouldEqual(person.person.firstName);
            result.lastName.ShouldEqual(person.person.lastName);
        }



        [Test]
        public void DeletePerson()
        {
            var person = new PersonToCreate()
            {
                person = new Person()
                {
                    firstName = generator.GenerateString(7),
                    lastName = generator.GenerateString(12)
                }
            };

            var newperson = personService.Create(person);
            personService.Delete(newperson).ShouldBeTrue();
        }

        [Test]
        public void Should_get_exception_when_user_config_is_insufficient()
        {
            Assert.That(() => new CapsuleDotNetWrapper.Services.PersonService("", "https://someapi.com"), Throws.Exception.TypeOf<ArgumentException>());         
        }

        [Test]
        public void Should_get_exception_when_url_config_is_insufficient()
        {
            Assert.That(() => new CapsuleDotNetWrapper.Services.PersonService("somepassword", ""), Throws.Exception.TypeOf<ArgumentException>());
        }

        [SetUp]
        public void Setup()
        {
            // Insert a valid API token and Base URL here.
            personService = new CapsuleDotNetWrapper.Services.PersonService(ConfigurationManager.AppSettings["Usertoken"], ConfigurationManager.AppSettings["ApiBaseUrl"]);

            // random string generator used as a crux to make tests a bit easier to follow.
            generator = new StringGenerator();
        }

        PersonService personService;
        StringGenerator generator;
    }
}