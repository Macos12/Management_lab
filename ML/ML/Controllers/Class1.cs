using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ML.Controllers
{
    public class Class1
    {
        public dynamic a(string id)
        {
            ApplicationDbContext db1 = new ApplicationDbContext();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db1));
            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db1));
            string currentrole = string.Empty;
            if (id != null)
            {
                
                var rolesForUser = UserManager.FindById(id).Roles;
                foreach (var item in rolesForUser)
                {
                    IdentityRole role = RoleManager.FindById(item.RoleId);
                    currentrole = role.Name;
                }
            }
            return currentrole;
        }
    }
}