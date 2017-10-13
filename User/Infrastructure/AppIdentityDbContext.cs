using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using User.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace User.Infrastructure
{
    public class AppIdentityDbContext:IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext():base("IdentityDbUser")
        {

        }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<AppUser>()
               .ToTable("Users");
        }
    }

    public class AppUserMap: EntityTypeConfiguration<AppUser>
    {
        public AppUserMap()
        {
           
                
                
        }
        
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            InitializerIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializerIdentityForEF(AppIdentityDbContext db)
        {
            //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            //const string name = "Admin@Admin.com";
            //const string password = "Admin@Admin123";
            //const string roleName = "Admin";

            //var role = roleManager.FindByName(roleName);
            //if (role == null)
            //{
            //    role = new ApplicationRole(roleName);
            //    var roleresult = roleManager.Create(role);
            //}
            //var user = userManager.FindByName(name);
            //if (user == null)
            //{
            //    user = new ApplicationUser
            //    {
            //        UserName = name,
            //        Email = name
            //    };
            //    var result = userManager.Create(user, password);
            //    result = userManager.SetLockoutEnabled(user.Id, false);
            //}

            //var rolesForUser = userManager.GetRoles(user.Id);
            //if (!rolesForUser.Contains(role.Name))
            //{
            //    var result = userManager.AddToRole(user.Id, role.Name);
            //}
        }
    }
}