using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CertificadoLoteDecenalMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
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
}
