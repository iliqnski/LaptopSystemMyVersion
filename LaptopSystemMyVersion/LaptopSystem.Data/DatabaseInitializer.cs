using LaptopSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaptopSystem.Data
{
    public class DatabaseInitializer : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public DatabaseInitializer()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (context.Laptops.Count() > 0)
            {
                return;
            }

            Random rand = new Random();
            Manufacturer manufacturer = new Manufacturer { Name = "Lenovo" };
            ApplicationUser user = new ApplicationUser() { UserName = "TestUser", Email = "Test@test.bg" };

            for (int i = 0; i < 10; i++)
            {
                Laptop laptop = new Laptop();
                laptop.HardDiskSize = rand.Next(10, 1000);
                laptop.ImageUrl = 
                    "http://laptop.bg/system/images/26781/thumb/Aspire_V5_573G.jpg?1380200094";
                laptop.Manufacturer = manufacturer;
                laptop.Model = "IdeaPad";
                laptop.MonitorSize = 15.6;
                laptop.Price = rand.Next(600, 12000);
                laptop.RamMemorySize = rand.Next(1, 16);
                laptop.Weight = 3;

                var votesCount = rand.Next(0, 10);
                for (int j = 0; j < votesCount; j++)
                {
                    laptop.Votes.Add(new Vote { Laptop = laptop, VotedBy = user});
                }

                var commentsCount = rand.Next(0, 10);
                for (int j = 0; j < commentsCount; j++)
                {
                    laptop.Comments.Add(new Comment { Laptop = laptop, Author = user, Content = "Mnogo qk laptop" });
                }

                context.Laptops.Add(laptop);
                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}