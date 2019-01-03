using System.Web.Mvc;
using CertificadoLoteDecenalMVC.Models.Business.Logging;

namespace CertificadoLoteDecenalMVC.Controllers
{
    [CustomAuthorize(Roles = "Internet_Desarrollo")]
    public class LogController : Controller
    {
        private ILogger logger;

        public LogController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var logDays = logger.GetLogDays();
            return View(logDays);
        }

        [HttpGet]
        public ViewResult Details(string dia)
        {
            var logs = logger.GetLogs(dia);
            ViewBag.Dia = dia;

            return View(logs);
        }

        [HttpGet]
        public FilePathResult DescargarLog(string dia)
        {
            var rutaArchivo = Server.MapPath($"~/Logs/{dia}.json");
            return File(rutaArchivo, "application/json", dia);
        }
    }
}