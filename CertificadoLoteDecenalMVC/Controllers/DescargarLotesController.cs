using System.Web.Mvc;
using CertificadoLoteDecenalMVC.Models.Business.Services.DescargarLotes;

namespace CertificadoLoteDecenalMVC.Controllers
{
    public class DescargarLotesController : Controller
    {
        private readonly string EXCEL_MIME_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        [HttpGet]
        public ActionResult Index()
        {
            var lotesDir = Server.MapPath("~/LotesGenerados");
            var archivos = DescargarLotesService.ObtenerLotesGenerados(lotesDir);
            return View(archivos);
        }

        [HttpGet]
        public FilePathResult DescargarLote(string nombreArchivo)
        {
            var rutaArchivo = Server.MapPath($"~/LotesGenerados/{nombreArchivo}");
            return File(rutaArchivo, EXCEL_MIME_TYPE, nombreArchivo);
        }
    }
}