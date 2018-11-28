using System;

namespace CertificadoLoteDecenalMVC.Models.Business.FileRecords
{
    /// <summary>
    /// Representa un lote generado para descargar junto a
    /// su respectiva fecha de procesamiento.
    /// </summary>
    public class DescargaLote
    {
        /// <summary>
        /// El nombre del lote generado con extensión de archivo.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// La fecha en que se procesó el archvo.
        /// </summary>
        public DateTime FechaProcesamiento { get; set; }
    }
}