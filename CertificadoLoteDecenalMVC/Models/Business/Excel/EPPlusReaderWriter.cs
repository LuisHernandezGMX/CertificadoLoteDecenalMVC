using System;
using System.Linq;
using System.Collections.Generic;
using OfficeOpenXml;
using CertificadoLoteDecenalMVC.Models.Business.FileRecords;

namespace CertificadoLoteDecenalMVC.Models.Business.Excel
{
    /// <summary>
    /// Lee y escribe archivos de Excel por medio de la biblioteca EPPlus.
    /// Funciona únicamente para archivos OpenXml (Office 2007 en adelante).
    /// </summary>
    public class EPPlusReaderWriter : IExcelIO
    {
        private ExcelPackage excel;
        private ExcelWorksheet worksheet;

        /// <summary>
        /// Abre el archivo de Excel indicado para su lectura.
        /// </summary>
        /// <param name="archivo">La ruta del archivo de Excel a leer.</param>
        public EPPlusReaderWriter(string archivo)
        {
            excel = new ExcelPackage(new System.IO.FileInfo(archivo));
            worksheet = excel.Workbook.Worksheets[1];
        }

        /// <summary>
        /// Crea un nuevo archivo de Excel con una hoja.
        /// </summary>
        public EPPlusReaderWriter()
        {
            excel = new ExcelPackage();
            worksheet = excel.Workbook.Worksheets.Add("Hoja1");
        }

        /// <summary>
        /// Lee el archivo de Excel y regresa los registros como una colección.
        /// </summary>
        /// <returns>Una colección de registros.</returns>
        public IEnumerable<Certificado> LeerArchivo()
        {
            var registros = new List<Certificado>();
            var start = worksheet.Dimension.Start;
            var end = worksheet.Dimension.End;

            for (int rowNum = start.Row + 1; rowNum <= end.Row; rowNum++) {
                var row = worksheet.Cells[$"A{rowNum}:AT{rowNum}"].Value as object[,];
                registros.Add(ParseFila(row));
            }

            return registros;
        }

        /// <summary>
        /// Escribe en el archivo de Excel indicado las pólizas y sus respectivos números de endoso y números de lote.
        /// </summary>
        /// <param name="rutaArchivo">La ruta donde se guardará el archivo de Excel.</param>
        /// <param name="registros">Los registros ya procesados del archivo de Excel.</param>
        /// <param name="horaInicio">La hora en que se inició el proceso de los registros.</param>
        /// <param name="horaTermino">La hora en que terminaron de procesarse los registros.</param>
        /// <param name="tiempoProcesamiento">El tiempo de procesamiento.</param>
        public void EscribirArchivo(string rutaArchivo, IEnumerable<PolizaEndosoLote> registros, DateTime fechaInicio, DateTime fechaTermino, TimeSpan tiempoProcesamiento)
        {
            // Se agrega el tiempo de procesamiento.
            var timeRange = worksheet.Cells["A1:C2"];
            var timeTable = worksheet.Tables.Add(timeRange, "TiempoProcesamiento");

            timeTable.Columns[0].Name = "Hora de Inicio";
            timeTable.Columns[1].Name = "Hora de Término";
            timeTable.Columns[2].Name = "Tiempo de Procesamiento";

            worksheet.Cells["A2"].Value = fechaInicio.ToLongTimeString();
            worksheet.Cells["B2"].Value = fechaTermino.ToLongTimeString();
            worksheet.Cells["C2"].Value = tiempoProcesamiento.ToString(@"hh\:mm\:ss");

            // Y se agrega una tabla con los resultados.
            int rowNum = 5;
            var range = worksheet.Cells[$"A4:C{registros.Count() + 4}"];
            var table = worksheet.Tables.Add(range, "CertificadoLoteDecenal");

            table.Columns[0].Name = "Póliza";
            table.Columns[1].Name = "Endoso";
            table.Columns[2].Name = "Lote";

            foreach (var registro in registros) {
                worksheet.Cells[$"A{rowNum}"].Value = registro.NroPoliza;
                worksheet.Cells[$"B{rowNum}"].Value = registro.NroEndoso;
                worksheet.Cells[$"C{rowNum}"].Value = registro.Lote;
                rowNum++;
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            excel.SaveAs(new System.IO.FileInfo(rutaArchivo));
        }

        /// <summary>
        /// Transforma una fila del archivo de Excel en un objeto.
        /// </summary>
        /// <param name="fila">La fila a transformar.</param>
        /// <returns>Un objeto Registro.</returns>
        private Certificado ParseFila(object[,] fila)
        {
            var poliza = fila[0, 0] as string;
            var ubc = (int)(double)fila[0, 1];
            var noCertificado = fila[0, 2] as string;
            var fecIni = (DateTime)fila[0, 3];
            var fecFinDos = fila[0, 4] as DateTime?;
            var fecFinTres = fila[0, 5] as DateTime?;
            var fecFinCinco = (DateTime)fila[0, 6];
            var fecFinDiez = (DateTime)fila[0, 7];
            var avaluo = (decimal)(double)fila[0, 8];
            var anticipo = (decimal)(double)fila[0, 9];
            var primaNetaReal = (decimal)(double)fila[0, 10];
            var primaNetaEndo = (decimal)(double)fila[0, 11];
            var primaHabitabilidad = (decimal)(double)fila[0, 12];
            var primaSeguridad = (decimal)(double)fila[0, 13];
            var primaImpermeabilizacionCubiertas = (decimal)(double)fila[0, 14];
            var impermeabilizacionFachadas = (decimal)(double)fila[0, 15];
            var cuotaMillar = (decimal)(double)fila[0, 16];
            var nombreAcreditado = fila[0, 17] as string;
            var rFC = fila[0, 18] as string;
            var nSS = fila[0, 19]?.ToString();
            var desarrollo = fila[0, 20] as string;
            var noCredito = fila[0, 21] as string;
            var cuv = fila[0, 22] as string;
            var condominio = fila[0, 23] as string;
            var calle = fila[0, 24] as string;
            var noExterior = fila[0, 25] as string;
            var noInterior = fila[0, 26] as string;
            var manzana = fila[0, 27] as string;
            var lote = fila[0, 28] as string;
            var supermanzana = fila[0, 29] as string;
            var edificio = fila[0, 30] as string;
            var nivel = fila[0, 31] as string;
            var cp = fila[0, 32] as string;
            var colonia = fila[0, 33] as string;
            var municipio = fila[0, 34] as string;
            var entidad = fila[0, 35] as string;
            var desarrollador = fila[0, 36] as string;
            var rfcDesarrollador = fila[0, 37] as string;
            var inicioGarantia = fila[0, 38] as DateTime?;
            var finObra = fila[0, 39] as DateTime?;
            var adquisicion = fila[0, 40] as DateTime?;
            var nombreNotario = fila[0, 41] as string;
            var noEscritura = fila[0, 42]?.ToString();
            var deFecha = (DateTime)fila[0, 43];
            var noNotario = (decimal?)(double?)fila[0, 44];
            var entidad2 = fila[0, 45] as string;

            return new Certificado {
                Poliza = poliza?.Trim() ?? string.Empty,
                Ubc = ubc,
                NoCertificado = noCertificado?.Trim() ?? string.Empty,
                FecIni = fecIni,
                FecFinDos = fecFinDos,
                FecFinTres = fecFinTres,
                FecFinCinco = fecFinCinco,
                FecFinDiez = fecFinDiez,
                Avaluo = avaluo,
                Anticipo = anticipo,
                PrimaNetaReal = primaNetaReal,
                PrimaNetaEndo = primaNetaEndo,
                PrimaHabitabilidad = primaHabitabilidad,
                PrimaSeguridad = primaSeguridad,
                PrimaImpermeabilizacionCubiertas = primaImpermeabilizacionCubiertas,
                ImpermeabilizacionFachadas = impermeabilizacionFachadas,
                CuotaMillar = cuotaMillar,
                NombreAcreditado = nombreAcreditado?.Trim() ?? string.Empty,
                RFC = rFC?.Trim() ?? string.Empty,
                NSS = nSS?.Trim() ?? string.Empty,
                Desarrollo = desarrollo?.Trim() ?? string.Empty,
                NoCredito = string.IsNullOrWhiteSpace(noCredito) ? null : decimal.Parse(noCredito) as decimal?,
                CUV = cuv?.Trim() ?? string.Empty,
                Condominio = condominio?.Trim() ?? string.Empty,
                Calle = calle?.Trim() ?? string.Empty,
                NoExterior = noExterior?.Trim() ?? string.Empty,
                NoInterior = noInterior?.Trim() ?? string.Empty,
                Manzana = manzana?.Trim() ?? string.Empty,
                Lote = lote?.Trim() ?? string.Empty,
                SuperManzana = supermanzana?.Trim() ?? string.Empty,
                Edificio = edificio?.Trim() ?? string.Empty,
                Nivel = nivel?.Trim() ?? string.Empty,
                CP = cp?.Trim() ?? string.Empty,
                Colonia = colonia?.Trim() ?? string.Empty,
                Municipio = municipio?.Trim() ?? string.Empty,
                Entidad = entidad?.Trim() ?? string.Empty,
                Desarrollador = desarrollador?.Trim() ?? string.Empty,
                RFCDesarrollador = rfcDesarrollador?.Trim() ?? string.Empty,
                InicioGarantia = inicioGarantia,
                FinObra = finObra,
                Adquisicion = adquisicion,
                NombreNotario = nombreNotario?.Trim() ?? string.Empty,
                NoEscritura = string.IsNullOrWhiteSpace(noEscritura) ? null : decimal.Parse(noEscritura) as decimal?,
                DeFecha = deFecha,
                NoNotario = noNotario,
                Entidad2 = entidad2?.Trim() ?? string.Empty
            };
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    excel.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}