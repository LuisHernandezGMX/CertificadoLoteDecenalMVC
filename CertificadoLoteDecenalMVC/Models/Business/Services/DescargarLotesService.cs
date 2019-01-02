using System.Collections.Generic;
using CertificadoLoteDecenalMVC.Models.DAL.DAO;
using CertificadoLoteDecenalMVC.Models.Business.FileRecords;

namespace CertificadoLoteDecenalMVC.Models.Business.Services.DescargarLotes
{
    public static class DescargarLotesService
    {
        /// <summary>
        /// Regresa una lista con los nombres de los archivos Excel de lotes generados
        /// y sus fechas de procesamiento hasta el momento por la aplicación.
        /// </summary>
        /// <param name="lotesDir">El directorio de la aplicación donde se almacenan los lotes generados.</param>
        /// <returns>Una lista de objetos DescargaLote.</returns>
        public static IEnumerable<DescargaLote> ObtenerLotesGenerados(string lotesDir)
        {
            return DescargarLotesDao.ObtenerNombresLotesGenerados(lotesDir);
        }
    }
}