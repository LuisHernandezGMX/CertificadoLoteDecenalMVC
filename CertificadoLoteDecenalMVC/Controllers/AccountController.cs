using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CertificadoLoteDecenalMVC.Models.DAL.ViewModels.Account;

namespace CertificadoLoteDecenalMVC.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpGet]
        public RedirectToRouteResult Logout()
        {
            Session.Clear();
            Session.Abandon();

            HttpContext
                .GetOwinContext()
                .Authentication
                .SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid) {
                using (var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>())
                using (var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>()) {
                    var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                    switch (result) {
                        case SignInStatus.Success:
                            Session["NombreCompleto"] = userManager.FindByName(model.UserName).Nombre;

                            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) {
                                return Redirect(returnUrl);
                            }

                            return RedirectToAction("Index", "Home");

                        default:
                            ModelState.AddModelError("UserName", "Credenciales no válidas.");
                            ModelState.AddModelError("Password", ".");
                            break;
                    }
                }
            }

            return View(model);
        }
    }
}