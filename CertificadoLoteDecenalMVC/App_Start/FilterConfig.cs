using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CertificadoLoteDecenalMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
            filters.Add(new RestaurarSesionAttribute());
        }
    }

    /// <summary>
    /// Autorización personalizada para esta aplicación. Si el usuario está autenticado lo redirige al Index
    /// de HomeController. De lo contrario, lo redirige a la página de Login.
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                    controller = "Home",
                    action = "Index"
                }));
            } else {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }

    /// <summary>
    /// Restaura los elementos almacenados previamente en la sesión si no están presentes.
    /// </summary>
    public class RestaurarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var http = filterContext.HttpContext;
            var user = http.User;
            var session = http.Session;

            if (user.Identity.IsAuthenticated) {
                if (session["NombreCompleto"] == null) {
                    using (var userManager = http.GetOwinContext().Get<ApplicationUserManager>()) {
                        session["NombreCompleto"] = userManager.FindByName(user.Identity.Name).Nombre;
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
