using System.Web;
using System.Web.Mvc;
using CertificadoLoteDecenalMVC.Models.Business.Logging;
using CertificadoLoteDecenalMVC.Models.Business.Messages;
using CertificadoLoteDecenalMVC.Models.Business.Services.Home;

namespace CertificadoLoteDecenalMVC.Controllers
{
    public class HomeController : Controller
    {
        private ILogger logger;

        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult LayoutCertificados()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult PrevisualizarCertificados(HttpPostedFileBase layoutCertificados)
        {
            Message resultado;
            var tempDir = Server.MapPath("~/TEMP");
            var registros = new HomeService(logger)
                .LeerArchivo(layoutCertificados, tempDir, out resultado);

            if (!resultado.Success) {
                ViewBag.ResultadoProcesamiento = resultado;
                return View("LayoutCertificados");
            }

            return View("LayoutCertificados", registros);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ViewResult SubirCertificados(HttpPostedFileBase layoutCertificados)
        {
            var tempDir = Server.MapPath("~/TEMP");
            var lotesDir = Server.MapPath("~/LotesGenerados");
            var resultado = new HomeService(logger)
                .LeerYProcesarRegistros(layoutCertificados, tempDir, lotesDir);

            if (!resultado.Success) {
                ViewBag.ResultadoProcesamiento = resultado;
                return View("LayoutCertificados");
            }

            return View(resultado);
        }
    }
}