using System;

namespace CertificadoLoteDecenalMVC.Models.Business.FileRecords
{
    /// <summary>
    /// Representa un registro leído del archivo de Excel de certificados.
    /// </summary>
    public class Certificado
    {
        public string Poliza { get; set; }
        public int Ubc { get; set; }
        public string NoCertificado { get; set; }
        public DateTime FecIni { get; set; }
        public DateTime? FecFinDos { get; set; }
        public DateTime? FecFinTres { get; set; }
        public DateTime FecFinCinco { get; set; }
        public DateTime FecFinDiez { get; set; }
        public decimal Avaluo { get; set; }
        public decimal Anticipo { get; set; }
        public decimal PrimaNetaReal { get; set; }
        public decimal PrimaNetaEndo { get; set; }
        public decimal PrimaHabitabilidad { get; set; }
        public decimal PrimaSeguridad { get; set; }
        public decimal PrimaImpermeabilizacionCubiertas { get; set; }
        public decimal ImpermeabilizacionFachadas { get; set; }
        public decimal CuotaMillar { get; set; }
        public string NombreAcreditado { get; set; }
        public string RFC { get; set; }
        public string NSS { get; set; }
        public string Desarrollo { get; set; }
        public decimal? NoCredito { get; set; }
        public string CUV { get; set; }
        public string Condominio { get; set; }
        public string Calle { get; set; }
        public string NoExterior { get; set; }
        public string NoInterior { get; set; }
        public string Manzana { get; set; }
        public string Lote { get; set; }
        public string SuperManzana { get; set; }
        public string Edificio { get; set; }
        public string Nivel { get; set; }
        public string CP { get; set; }
        public string Colonia { get; set; }
        public string Municipio { get; set; }
        public string Entidad { get; set; }
        public string Desarrollador { get; set; }
        public string RFCDesarrollador { get; set; }
        public DateTime? InicioGarantia { get; set; }
        public DateTime? FinObra { get; set; }
        public DateTime? Adquisicion { get; set; }
        public string NombreNotario { get; set; }
        public decimal? NoEscritura { get; set; }
        public DateTime DeFecha { get; set; }
        public decimal? NoNotario { get; set; }
        public string Entidad2 { get; set; }
    }
}