using System.ComponentModel.DataAnnotations;

namespace MessageService.DataBase
{
    public class Account
    {
        [Key]
        public int EmployeeId { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public AccountProfile AccountProfile { get; set; }

        }
    }