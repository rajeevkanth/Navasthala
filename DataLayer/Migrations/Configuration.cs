using System.Web.Security;
using WebMatrix.WebData;

namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.NavasthalaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.NavasthalaContext context)
        {
            WebSecurity.InitializeDatabaseConnection("NavasthalaContext", "UserProfile", "UserId", "UserName", autoCreateTables: true);

            var roles = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider) Membership.Provider;

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
                membership.CreateUserAndAccount("Rajeev", "Admin123");
            }

            if (membership.GetUser("Vikram", false) == null)
            {
                membership.CreateUserAndAccount("Vikram", "Admin123");
            }

            if (membership.GetUser("Aparna", false) == null)
            {
                membership.CreateUserAndAccount("Aparna", "Admin123");
            }

            if (membership.GetUser("Vidya", false) == null)
            {
                membership.CreateUserAndAccount("Vidya", "Admin123");
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
        }
    }
}
