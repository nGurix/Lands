using Lands.Domain;


namespace Lands.Backend.Models
{
    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<User> Users { get; set; }

        public System.Data.Entity.DbSet<Lands.Domain.UserType> UserTypes { get; set; }
    }
}