using Lands.Domain;


namespace Lands.Backend.Models
{
    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<User> Users { get; set; }
    }
}