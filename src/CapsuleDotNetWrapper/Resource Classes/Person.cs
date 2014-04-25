using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace CapsuleDotNetWrapper
{
    /// <summary>
    /// http://developer.capsulecrm.com/v1/resources/parties/
    /// </summary>
    public class Person
    {
        public string id { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string organisationId { get; set; }
        public string organisationName { get; set; }
    }
}
