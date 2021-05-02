using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data
{
    //DbContext -> representação do banco de dados em memória
    //DbSet -> representação das tabelas em memória
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
    }

}