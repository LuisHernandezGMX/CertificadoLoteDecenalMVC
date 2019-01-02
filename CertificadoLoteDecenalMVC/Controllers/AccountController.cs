using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid) {
                return Login();
            }

            return View(model);
        }
    }
}