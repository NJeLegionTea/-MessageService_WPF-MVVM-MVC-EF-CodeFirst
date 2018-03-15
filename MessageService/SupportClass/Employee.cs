using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MessageService.SupportClass
{
    
    [DataContract]
    public class Employee
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Department { get; set; }

        [DataMember]
        public List<Employee> DialogWith { get; set; }
    }
}