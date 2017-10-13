using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;

using User.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace User.Infrastructure
{
    public class AppRoleManager:RoleManager<AppRole>
    {
        public AppRoleManager(RoleStore<AppRole> store):base(store)
        {

        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<AppIdentityDbContext>()));
        }
    }
}