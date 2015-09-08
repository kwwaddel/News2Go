namespace NewsAggregator.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NewsAggregator.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NewsAggregator.Data.ApplicationDbContext context)
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindByName("user@gmail.com");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                };
                userManager.Create(user, "password");
                user = userManager.FindByName("user@gmail.com");
            }

            context.Categories.AddOrUpdate(new Category { Name = "Sport" });
            context.Categories.AddOrUpdate(new Category { Name = "Environment" });

            var user2 = userManager.FindByName("bob@gmail.com");

            if (user2 == null)
            {
                user2 = new ApplicationUser
                {
                    UserName = "bob@gmail.com",
                    Email = "bob@gmail.com",
                };
                userManager.Create(user2, "password");
                user2 = userManager.FindByName("bob@gmail.com");
            }

            var user3 = userManager.FindByName("ted@gmail.com");

            if (user3 == null)
            {
                user3 = new ApplicationUser
                {
                    UserName = "ted@gmail.com",
                    Email = "ted@gmail.com",
                };
                userManager.Create(user3, "password");
                user3 = userManager.FindByName("ted@gmail.com");
            }

        }


    }
}
