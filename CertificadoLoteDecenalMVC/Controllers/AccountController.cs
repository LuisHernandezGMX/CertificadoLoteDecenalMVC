using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CertificadoLoteDecenalMVC.Models.Business.Logging;
using CertificadoLoteDecenalMVC.Models.Business.Extensions;
using CertificadoLoteDecenalMVC.Models.DAL.ViewModels.Account;

namespace CertificadoLoteDecenalMVC.Controllers
{
    public class AccountController : Controller
    {
        private ILogger logger;

        public AccountController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Login()
        {
            return View();
        }

        [HttpGet]
        public RedirectToRouteResult Logout()
        {
            logger.InfoLog(
                "Cierre de Sesión",
                GetType().FullName,
                $"El usuario {User.Identity.Name} ha cerrado sesión.",
                MethodBase.GetCurrentMethod());

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
            var clase = GetType().FullName;

            if (ModelState.IsValid) {
                using (var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>())
                using (var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>()) {
                    var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                    switch (result) {
                        case SignInStatus.Success:
                            Session["NombreCompleto"] = userManager.FindByName(model.UserName).Nombre;

                            logger.InfoLog(
                                "Inicio de Sesión",
                                clase,
                                "Login",
                                $"El usuario {model.UserName} ha iniciado sesión.",
                                new [] {
                                    new Log.LogArgumento {
                                        Parametro = nameof(model),
                                        TipoDato = typeof(LoginViewModel).ToString(),
                                        Valor = "VistaModelo válido"
                                    },
                                    new Log.LogArgumento {
                                        Parametro = nameof(returnUrl),
                                        TipoDato = typeof(string).ToString(),
                                        Valor = returnUrl ?? "NULL"
                                    }
                                });

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

            logger.WarningLog(
                "Intento Fallido de Inicio de Sesión",
                clase,
                "Login",
                $"Se intentó iniciar sesión desde el equipo {Request.UserHostName} ({Request.UserHostAddress})",
                new [] {
                    new Log.LogArgumento {
                        Parametro = nameof(model),
                        TipoDato = typeof(LoginViewModel).ToString(),
                        Valor = $"UserName = {model.UserName}"
                    },
                    new Log.LogArgumento {
                        Parametro = nameof(returnUrl),
                        TipoDato = typeof(string).ToString(),
                        Valor = returnUrl ?? "NULL"
                    }
                });

            return View(model);
        }
    }
}