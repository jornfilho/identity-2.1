using System.Collections.ObjectModel;
using IdentitySample.Identity;
using IdentitySample.Identity.MongoDb;
using IdentitySample.Identity.MongoDb.Manager;
using IdentitySample.Identity.MongoDb.Models;
using IdentitySample.ViewModels.Role;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        #region Propriedades
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                if(_userManager == null)
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                return _userManager;
            }
            set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                if(_roleManager == null)
                    _roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();

                return _roleManager;
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        #region Construtor
        public RolesAdminController()
        {
        }

        public RolesAdminController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        } 
        #endregion

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            var roles = RoleManager.Roles;
            ICollection<RoleIndexViewModel> viewModel = new Collection<RoleIndexViewModel>();
            if (roles != null)
                viewModel = roles.Select(u => new RoleIndexViewModel { Id = u.Id, Name = u.Name }).ToList();

            return View(viewModel);
        }

        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id) == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var role = await RoleManager.FindByIdAsync(id);
            if(role == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            IList<string> usersNames = new List<string>();
            var users = UserManager.Users;
            if (users != null)
                foreach (var user in users.ToList())
                    if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                        usersNames.Add(user.UserName);

            var viewModel = new RoleDetailsViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Users = usersNames
            };

            return View(viewModel);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(roleModel);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                await RoleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            var viewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(viewModel);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result;
                if (deleteUser != null)
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                else
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
