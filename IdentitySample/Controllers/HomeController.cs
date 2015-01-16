using System.Web;
using System.Web.Mvc;
using IdentitySample.ViewModels.Home;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using IdentitySample.Identity;
using IdentitySample.Identity.MongoDb;
using IdentitySample.Identity.MongoDb.Manager;
using IdentitySample.ViewModels.Account;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            ViewBag.Message = "Sair";

            var viewModel = new SignOutViewModel();

            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            if(manager == null)
                return View(viewModel);

            var user = manager.FindById(User.Identity.GetUserId());
            if (user == null)
                return View(viewModel);

            viewModel.Clients = user.Clients;
            return View(viewModel);
        }
    }
}
