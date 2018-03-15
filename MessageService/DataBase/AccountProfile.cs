using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageService.DataBase
{
    public class AccountProfile
    {
        [Key]
        [ForeignKey("Account")]
        public int EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Department { get; set; }

        public Account Account { get; set; }
    }
}