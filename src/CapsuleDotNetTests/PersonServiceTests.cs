﻿using System;
using System.Collections.Generic;
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
            var result = personService.GetPerson("60275159");
            result.id.ShouldEqual("60275159");
            result.firstName.ShouldEqual("Ola");
            result.lastName.ShouldEqual("Nordmann");
        }

        [Test]
        public void CreatePerson()
        {
            var generator = new StringGenerator();
          
            var person = new PersonToCreate()
            {
                person = new Person() 
                { 
                    firstName = generator.GenerateString(7), 
                    lastName = generator.GenerateString(12) }
            };

            var result = personService.Create(person);
            result.id.ShouldBeGreaterThan("60275159");
            result.firstName.ShouldEqual(person.person.firstName);
            result.lastName.ShouldEqual(person.person.lastName);
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
            personService = new CapsuleDotNetWrapper.Services.PersonService("", "");
        }

        PersonService personService;
    }
}