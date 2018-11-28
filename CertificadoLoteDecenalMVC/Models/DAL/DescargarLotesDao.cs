using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using CertificadoLoteDecenalMVC.Models.Business.FileRecords;

namespace CertificadoLoteDecenalMVC.Models.DAL.DescargarLotes
{
    public static class DescargarLotesDao
    {
        /// <summary>
        /// Regresa una lista con los nombres de los archivos Excel de lotes generados
        /// y sus fechas de procesamiento hasta el momento por la aplicación.
        /// </summary>
        /// <param name="lotesDir">El directorio de la aplicación donde se almacenan los lotes generados.</param>
        /// <returns>Una lista de objetos DescargaLote.</returns>
        public static IEnumerable<DescargaLote> ObtenerNombresLotesGenerados(string lotesDir)
        {
            return Directory
                .EnumerateFiles(lotesDir)
                .Select(archivo => {
                    var nombre = Path.GetFileName(archivo);
                    var fechaArchivo = nombre
                        .Substring(nombre.IndexOf("LOTE_") + 5)
                        .Replace(".xlsx", string.Empty);

                    fechaArchivo = fechaArchivo.Contains("p. m.")
                        ? fechaArchivo.Replace("p. m.", "PM")
                        : fechaArchivo.Replace("a. m.", "AM");

                    var fechaProcesamiento = DateTime.ParseExact(fechaArchivo, "dd-MM-yyyy hh-mm-ss tt", CultureInfo.InvariantCulture);

                    return new DescargaLote {
                        Nombre = nombre,
                        FechaProcesamiento = fechaProcesamiento
                    };
                })
                .ToList();
        }
    }
}