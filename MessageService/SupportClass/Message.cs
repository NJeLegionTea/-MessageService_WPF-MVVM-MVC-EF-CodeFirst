using System;
using System.Runtime.Serialization;

namespace MessageService.SupportClass
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public int MessageId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public Employee Recipient { get; set; }

        [DataMember]
        public Employee Sender { get; set; }

        [DataMember]
        public string Contents { get; set; }
    }

}