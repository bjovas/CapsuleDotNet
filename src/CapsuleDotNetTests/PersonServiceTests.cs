using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;
using CapsuleDotNetWrapper;

namespace CapsuleDotNetTests
{
    [TestFixture]
    public class PersonServiceTests
    {
        [Test]
        public void GetContact()
        {
            var wrapper = new CapsuleDotNetWrapper.Services.PersonService("", "");
            var result = wrapper.GetPerson("60275159");
            result.firstName.ShouldEqual("Ola");
            result.lastName.ShouldEqual("Nordmann");
        }
    }
}