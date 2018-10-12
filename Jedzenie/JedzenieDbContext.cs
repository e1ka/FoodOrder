using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Models;
using System.Threading.Tasks;

namespace Jedzenie
{

    public class JedzenieDbContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Website> Websites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public JedzenieDbContext()
            : base("name=JedzenieContext")
        {
            // Database.SetInitializer<JedzenieDbContext>(new CreateDatabaseIfNotExists<JedzenieDbContext>());
            Database.SetInitializer<JedzenieDbContext>(new JedzenieInitializer());
        }

    }

    public class JedzenieInitializer : CreateDatabaseIfNotExists<JedzenieDbContext>
    {
        
        protected override void Seed(JedzenieDbContext context)
        {
            
            Task.Run(async () =>
            {
                Parsers.PodwaleParser par = new Parsers.PodwaleParser();
                Website dishes = await par.GetDishes(@"http://www.stolowkapodwale.pl");
                context.Websites.Add(dishes);

                //Parsers.ParserMaximus par2 = new Parsers.ParserMaximus();
               // Website dishes2 = await par2.GetDishes(@"http://maximus.bielsko.pl/pl/artykul/menu-dnia-18.html");
               // context.Websites.Add(dishes2);

                context.SaveChanges();

            }).GetAwaiter().GetResult();
            base.Seed(context);
        }
    }
}