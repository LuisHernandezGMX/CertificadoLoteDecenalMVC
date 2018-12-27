using System;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using CertificadoLoteDecenalMVC.Models.Business.FileRecords;

namespace CertificadoLoteDecenalMVC.Models.Business.Excel
{
    /// <summary>
    /// Lee archivos de Excel por medio de la API OLE DB.
    /// Funciona únicamente para archivos XLS (Office 2003 y anteriores).
    /// </summary>
    public class OleDbReader : IExcelIO
    {
        /// <summary>
        /// La conexión OleDb hacia el archivo de Excel.
        /// </summary>
        private OleDbConnection conn;

        /// <summary>
        /// Abre el archivo de Excel indicado para su lectura.
        /// </summary>
        /// <param name="rutaArchivo">La ruta completa del archivo de Excel.</param>
        public OleDbReader(string rutaArchivo)
        {
            var connectionString = ObtenerCadenaConexion(rutaArchivo);
            conn = new OleDbConnection(connectionString);
            conn.Open();
        }

        /// <summary>
        /// Lee el archivo de Excel y regresa los registros como una colección.
        /// </summary>
        /// <returns>Una colección de registros.</returns>
        public IEnumerable<Certificado> LeerArchivo()
        {
            var registros = new List<Certificado>();
            var command = new OleDbCommand { Connection = conn };
            var sheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            var dataRow = sheet.Rows[0];
            var sheetName = dataRow["TABLE_NAME"].ToString();

            if (sheetName.EndsWith("$")) {
                var dataTable = new DataTable();
                command.CommandText = $"SELECT * FROM [{sheetName}]";
                new OleDbDataAdapter(command).Fill(dataTable);

                registros = dataTable
                    .Rows
                    .Cast<DataRow>()
                    .Select(row => ParseFila(row.ItemArray))
                    .ToList();
            }

            return registros;   
        }

        /// <summary>
        /// Implementación NO NECESARIA. La funcionalidad para escribir los
        /// archivos de Excel ya está cubierta por <see cref="EPPlusReaderWriter"/>.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        /// <param name="registros"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaTermino"></param>
        /// <param name="tiempoProcesamiento"></param>
        /// <exception cref="NotImplementedException">Implementación no necesaria.</exception>
        public void EscribirArchivo(string rutaArchivo, IEnumerable<PolizaEndosoLote> registros, DateTime fechaInicio, DateTime fechaTermino, TimeSpan tiempoProcesamiento)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Genera la cadena de conexión de OleDb a partir de la ruta
        /// completa del archivo.
        /// </summary>
        /// <param name="rutaArchivo">La ruta completa del archivo de Excel.</param>
        /// <returns>Una cadena de conexión para Excel &lt; 2003</returns>
        private string ObtenerCadenaConexion(string rutaArchivo)
        {
            var builder = new StringBuilder();
            var props = new Dictionary<string, string> {
                ["Provider"] = "Microsoft.Jet.OLEDB.4.0",
                ["Extended Properties"] = "Excel 8.0",
                ["Data Source"] = rutaArchivo
            };

            foreach (var prop in props) {
                builder
                    .Append(prop.Key)
                    .Append('=')
                    .Append(prop.Value)
                    .Append(';');
            }

            return builder.ToString();
        }

        /// <summary>
        /// Transforma una fila del archivo de Excel en un objeto.
        /// </summary>
        /// <param name="fila">La fila a transformar.</param>
        /// <returns>Un objeto Registro.</returns>
        private Certificado ParseFila(object[] fila)
        {
            var poliza = fila[0] as string;
            var ubc = (int)(double)fila[1];
            var noCertificado = fila[2] as string;
            var fecIni = (DateTime)fila[3];
            var fecFinDos = fila[4] as DateTime?;
            var fecFinTres = fila[5] as DateTime?;
            var fecFinCinco = (DateTime)fila[6];
            var fecFinDiez = (DateTime)fila[7];
            var avaluo = (decimal)(double)fila[8];
            var anticipo = (decimal)(double)fila[9];
            var primaNetaReal = (decimal)(double)fila[10];
            var primaNetaEndo = (decimal)(double)fila[11];
            var primaHabitabilidad = (decimal)(double)fila[12];
            var primaSeguridad = (decimal)(double)fila[13];
            var primaImpermeabilizacionCubiertas = (decimal)(double)fila[14];
            var impermeabilizacionFachadas = (decimal)(double)fila[15];
            var cuotaMillar = (decimal)(double)fila[16];
            var nombreAcreditado = fila[17] as string;
            var rFC = fila[18] as string;
            var nSS = fila[19]?.ToString();
            var desarrollo = fila[20] as string;
            var noCredito = fila[21] as string;
            var cuv = fila[22] as string;
            var condominio = fila[23] as string;
            var calle = fila[24] as string;
            var noExterior = fila[25] as string;
            var noInterior = fila[26] as string;
            var manzana = fila[27] as string;
            var lote = fila[28] as string;
            var supermanzana = fila[29] as string;
            var edificio = fila[30] as string;
            var nivel = fila[31] as string;
            var cp = fila[32] as string;
            var colonia = fila[33] as string;
            var municipio = fila[34] as string;
            var entidad = fila[35] as string;
            var desarrollador = fila[36] as string;
            var rfcDesarrollador = fila[37] as string;
            var inicioGarantia = fila[38] as DateTime?;
            var finObra = fila[39] as DateTime?;
            var adquisicion = fila[40] as DateTime?;
            var nombreNotario = fila[41]?.ToString();
            var noEscritura = fila[42]?.ToString();
            var deFecha = (DateTime)fila[43];
            var noNotario = fila[44] is DBNull ? null : Convert.ToDecimal(fila[44]) as decimal?;
            var entidad2 = fila[45] as string;

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
                    conn.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}