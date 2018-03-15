using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MessageService.DataBase
{
    [DataContract]
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public int? RecipientEmployeeId { get; set; }

        public AccountProfile Recipient { get; set; }

        public int? SenderEmployeeId { get; set; }

        public AccountProfile Sender { get; set; }

        public string Contents { get; set; }
    }
}