using System.Collections.Generic;

namespace FullStackTest.Models
{
    public class PersonDataModel
    {
        public int id { get; set; }
        public string creditorName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public double minPaymentPercentage { get; set; }
        public double balance { get; set; }
    }

    public class PersonList
    {
        public List <PersonDataModel> personList { get; set; }
    }

}
