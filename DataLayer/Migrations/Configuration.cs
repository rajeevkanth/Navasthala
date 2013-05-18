using System.Collections.Generic;
using System.Web.Security;
using DataLayer.Models;
using WebMatrix.WebData;
using System.Linq;
using System.Data.Entity.Migrations;

namespace DataLayer.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<NavasthalaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NavasthalaContext context)
        {
            if(!WebSecurity.Initialized)
            WebSecurity.InitializeDatabaseConnection("NavasthalaContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider) Membership.Provider;


            if (!context.AddressTypes.Any(p => p.Type == "Home"))
            {
                context.AddressTypes.Add(new AddressType {Type = "Home"});
            }

            if (!context.AddressTypes.Any(p => p.Type == "Work"))
            {
                context.AddressTypes.Add(new AddressType { Type = "Work" });
            }

            if (!context.PhoneTypes.Any(p => p.Type == "Mobile"))
            {
                context.PhoneTypes.Add(new PhoneType {Type = "Mobile"});
            }

            if (!context.PhoneTypes.Any(p => p.Type == "Landline"))
            {
                context.PhoneTypes.Add(new PhoneType { Type = "Landline" });
            }


            if (!roles.RoleExists("Admin"))
            {
                roles.CreateRole("Admin");
            }

            if (!roles.RoleExists("Investor"))
            {
                roles.CreateRole("Investor");
            }

            if (membership.GetUser("Rajeev", false) == null)
            {
                //membership.CreateUserAndAccount("Rajeev", "Admin123", new Dictionary<string, object> { new { "Email", "myemail@gmail.com" }});
                WebSecurity.CreateUserAndAccount("Rajeev", "Admin123", new { Email = "myemail@gmail.com", IsActive = "True" });
            }

            if (membership.GetUser("Vikram", false) == null)
            {
                //membership.CreateUserAndAccount("Vikram", "Admin123");
                WebSecurity.CreateUserAndAccount("Vikram", "Admin123", new { Email = "myemail@gmail.com", IsActive = "True" });
           
            }

            if (membership.GetUser("Aparna", false) == null)
            {
                //membership.CreateUserAndAccount("Aparna", "Admin123");
                WebSecurity.CreateUserAndAccount("Aparna", "Admin123", new { Email = "myemail@gmail.com",IsActive="True",LastName="Gudaloor" });
           
            }

            if (membership.GetUser("Vidya", false) == null)
            {
                //membership.CreateUserAndAccount("Vidya", "Admin123");
                WebSecurity.CreateUserAndAccount("Vidya", "Admin123", new { Email = "myemail@gmail.com", IsActive = "True", LastName = "RamMohan" });
           
            }

            if (membership.GetUser("Fanboy", false) == null)
            {
                //membership.CreateUserAndAccount("Fanboy", "Admin123");
                WebSecurity.CreateUserAndAccount("Fanboy", "Admin123", new { Email = "myemail@gmail.com",IsActive="True" });
           
            }

            if (!roles.GetRolesForUser("Rajeev").Contains("Admin"))
            {
                roles.AddUsersToRoles(new []{"Rajeev"},new[]{"Admin"} );
            }
            

            if (!roles.GetRolesForUser("Vikram").Contains("Admin"))
            {
                roles.AddUsersToRoles(new[] { "Vikram" }, new[] { "Admin" });
            }

            if (!roles.GetRolesForUser("Aparna").Contains("Investor"))
            {
                roles.AddUsersToRoles(new[] { "Aparna" }, new[] { "Investor" });
            }

            if (!roles.GetRolesForUser("Vidya").Contains("Investor"))
            {
                roles.AddUsersToRoles(new[] { "Vidya" }, new[] { "Investor" });
            }


            context.SaveChanges();
        }
    }
}
