using System;
using System.Collections.Generic;
using CertificadoLoteDecenalMVC.Models.Business.FileRecords;

namespace CertificadoLoteDecenalMVC.Models.Business.Excel
{
    /// <summary>
    /// Provee los métodos necesarios para leer y escribir archivos de Excel.
    /// </summary>
    public interface IExcelIO : IDisposable
    {
        /// <summary>
        /// Lee el archivo de Excel y regresa los registros como una colección.
        /// </summary>
        /// <returns>Una colección de registros.</returns>
        IEnumerable<Certificado> LeerArchivo();

        /// <summary>
        /// Escribe en el archivo de Excel indicado las pólizas y sus respectivos números de endoso y números de lote.
        /// </summary>
        /// <param name="rutaArchivo">La ruta donde se guardará el archivo de Excel.</param>
        /// <param name="registros">Los registros ya procesados del archivo de Excel.</param>
        /// <param name="horaInicio">La hora en que se inició el proceso de los registros.</param>
        /// <param name="horaTermino">La hora en que terminaron de procesarse los registros.</param>
        /// <param name="tiempoProcesamiento">El tiempo de procesamiento.</param>
        void EscribirArchivo(string rutaArchivo, IEnumerable<PolizaEndosoLote> registros, DateTime fechaInicio, DateTime fechaTermino, TimeSpan tiempoProcesamiento);
    }
}