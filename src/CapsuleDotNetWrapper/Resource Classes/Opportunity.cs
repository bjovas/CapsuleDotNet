using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CapsuleDotNetWrapper
{
    public class Opportunity
    {  
            public string id { get; set; }
            public string name { get; set; }
            public string currency { get; set; }
            public string milestoneId { get; set; }
            public string durationBasis { get; set; }
        
    }

    public class OpportunityToCreate
    {
        public Opportunity opportunity { get; set; }
    }
}
