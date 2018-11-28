namespace CertificadoLoteDecenalMVC.Models.Business.FileRecords
{
    /// <summary>
    /// Representa un registro resultado de la ejecución de los lotes de certificados.
    /// </summary>
    public class PolizaEndosoLote
    {
        public decimal NroPoliza { get; set; }
        public decimal NroEndoso { get; set; }
        public int Lote { get; set; }
    }
}