using System.Data.Entity;

namespace MessageService.DataBase
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("MessageServiceDBConnection") { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountProfile> AccountProfiles { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}