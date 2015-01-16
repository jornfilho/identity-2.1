using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Identity.Filters;
using IdentitySample.Identity.MongoDb;
using IdentitySample.Identity.MongoDb.Manager;
using IdentitySample.Identity.MongoDb.Models;
using IdentitySample.ViewModels.Claim;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace IdentitySample.Controllers
{
    [ClaimsAuthorize("AdmClaims","True")]
    public class ClaimsAdminController : Controller
    {
        #region Propriedades
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        private ApplicationDbContext _dbContext;

        public ApplicationDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = HttpContext.GetOwinContext().GetUserManager<ApplicationDbContext>();

                return _dbContext;
            }
            set { _dbContext = value; }
        }
        #endregion

        #region Construtor
        public ClaimsAdminController()
        {
        }

        public ClaimsAdminController(ApplicationUserManager userManager, ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            UserManager = userManager;
        } 
        #endregion

        // GET: ClaimsAdmin
        public ActionResult Index()
        {
            var viewModel = new ClaimsListViewModel
            {
                Claims = DbContext.Claims.ToList()
            };
            return View(viewModel);
        }

        // GET: ClaimsAdmin/SetUserClaim
        public ActionResult SetUserClaim(string id)
        {
            var user = UserManager.FindById(id);
            var userClaims = UserManager.GetClaimsAsync(id);
            

            var viewModel = new SetUserClaimViewModel
            {
                ClainsList = new SelectList(DbContext.Claims.ToList(), "Name", "Name"),
                UserName = user != null ? user.UserName : "",
                UserId = id
            };

            return View(viewModel);
        }

        // POST: ClaimsAdmin/SetUserClaim
        [HttpPost]
        public ActionResult SetUserClaim(ClaimViewModel claim, string id)
        {
            try
            {
                UserManager.AddClaimAsync(id, new Claim(claim.Type, claim.Value));

                return RedirectToAction("Details","UsersAdmin", new { id });
            }
            catch
            {
                return View();
            }
        }

        // GET: ClaimsAdmin/CreateClaim
        public ActionResult CreateClaim()
        {
            return View();
        }

        // POST: ClaimsAdmin/CreateClaim
        [HttpPost]
        public ActionResult CreateClaim(IdentityClaim identityUserClaim)
        {
            try
            {
                var saveResult = false;
                if (ModelState.IsValid)
                    saveResult = SaveClaim(identityUserClaim);

                if (!saveResult)
                    return View();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #region Sql Server
        //private bool SaveClaim(IdentityUserClaim IdentityUserClaim)
        //{
        //    try
        //    {
        //        DbContext.Claims.Add(IdentityUserClaim);
        //        DbContext.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //} 
        #endregion

        #region MongoDb
        private bool SaveClaim(IdentityClaim identityClaim)
        {
            try
            {
                if (identityClaim == null)
                    return false;

                if (!IsUniqueClaim(identityClaim))
                    return false;

                var saveResult = DbContext.ClainsCollection.Save(identityClaim);
                return saveResult.Ok;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool IsUniqueClaim(IdentityClaim identityClaim)
        {
            var query = Query.And(Query.EQ("name", identityClaim.Name), Query.Not(Query.EQ("_id", identityClaim.Id)));
            var hasRegister = DbContext.ClainsCollection.FindOneAs<IdentityClaim>(query);
            return hasRegister == null;
        }
        #endregion
    }
}
