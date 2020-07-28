using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ML.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        List<UserAndRolesModel> GetReport1 = new List<UserAndRolesModel>();
        // GET: Roles
        public ActionResult Index()
        {
            var myrole = new Class1();
            var userID = User.Identity.GetUserId();
            ViewBag.currentrole=myrole.a(userID);
            var users = from u in context.Users
                        from ur in u.Roles
                        join r in context.Roles on ur.RoleId equals r.Id
                        select new
                        {
                            UserId = u.Id,
                            Name = u.UserName,
                            Email = u.Email,
                            Role = r.Name
                        };
            foreach (var item in users)
            {
                GetReport1.Add(new UserAndRolesModel
                {
                    UserId = item.UserId,
                    Username = item.Name,
                    Email = item.Email,
                    Role = item.Role
                });
            }

            //var usersWithRoles = (from user in context.Users
            //                      select new
            //                      {
            //                          UserId = user.Id,
            //                          Username = user.UserName,
            //                          Email = user.Email,
            //                          RoleNames = (from userRole in user.Roles
            //                                       join role in context.Roles on userRole.RoleId
            //                                       equals role.Id
            //                                       select role.Name).ToList()
            //                      }).ToList().Select(p => new UserAndRolesModel()

            //                      {
            //                          UserId = p.UserId,
            //                          Username = p.Username,
            //                          Email = p.Email,
            //                          Role = string.Join(",", p.RoleNames)
            //                      });


            return View(GetReport1);
        }
        public async Task<ActionResult> DeleteUserid(string userId)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get User Data from Userid
            var user = await manager.FindByIdAsync(userId);

            //List Logins associated with user
            var logins = user.Logins;

            //Gets list of Roles associated with current user
            var rolesForUser = await manager.GetRolesAsync(userId);

            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                {
                    await manager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await manager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                //Delete User
                await manager.DeleteAsync(user);
                transaction.Commit();
            }

            return RedirectToAction("Index");
        }
        
        public ActionResult ApproveUser(string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            UserManager.RemoveFromRole(userId, "Pending");
            UserManager.AddToRole(userId, "User");
            return RedirectToAction("Index");
        }
        public ActionResult UnBanUser(string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            UserManager.RemoveFromRole(userId, "Banned");
            UserManager.AddToRole(userId, "User");
            return RedirectToAction("Index");
        }
        public ActionResult BanUser(string userId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var rolesForUser = UserManager.FindById(userId).Roles;
            foreach (var item in rolesForUser)
            {
                IdentityRole role = RoleManager.FindById(item.RoleId);
                currentrole = role.Name;
            }
            UserManager.RemoveFromRole(userId, currentrole);
            UserManager.AddToRole(userId, "Banned");
            return RedirectToAction("Index");
        }

        public ActionResult ModifyGivenRole(string userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            ApplicationUser user = UserManager.FindById(userId);
            var rolesForUser = UserManager.FindById(userId).Roles;
            var users = from u in context.Users
                        where u.Id == userId
                        select new
                        {
                            UserId = u.Id,
                            Name = u.UserName,
                            Email = u.Email
                        };
            foreach (var item in rolesForUser)
            {
                IdentityRole role = RoleManager.FindById(item.RoleId);
                currentrole = role.Name;
            }
            UserAndRolesModel urm = new UserAndRolesModel();
            urm.Username = user.UserName;
            urm.Role = currentrole;
            List<SelectListItem> list = context.Roles.Where(r => r.Name == "Admin" || r.Name == "User").OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem
            { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.ddl = new SelectList(list, "Value", "Text", currentrole);
            return PartialView("ModifyGivenRole", urm);
        }
        string currentrole;
        [HttpPost]
        public ActionResult ModifyGivenRole(UserAndRolesModel um, string ddl)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            RoleManager<IdentityRole> RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var rolesForUser = UserManager.FindById(um.UserId).Roles;
            ApplicationUser user = UserManager.FindById(um.UserId);

            foreach (var item in rolesForUser)
            {
                IdentityRole role = RoleManager.FindById(item.RoleId);
                currentrole = role.Name;
            }
            if (ModelState.IsValid)
            {
                UserManager.RemoveFromRole(user.Id, currentrole);
                UserManager.AddToRole(user.Id, ddl);
                return RedirectToAction("Index");
            }
            else
            {
                return PartialView(um);
            }
        }
    }
}