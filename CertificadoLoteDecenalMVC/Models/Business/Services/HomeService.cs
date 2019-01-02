using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using CertificadoLoteDecenalMVC.Models.DAL.Home;
using CertificadoLoteDecenalMVC.Models.Business.Excel;
using CertificadoLoteDecenalMVC.Models.Business.Logging;
using CertificadoLoteDecenalMVC.Models.Business.Messages;
using CertificadoLoteDecenalMVC.Models.Business.Extensions;
using CertificadoLoteDecenalMVC.Models.Business.FileRecords;

namespace CertificadoLoteDecenalMVC.Models.Business.Services.Home
{
    public class HomeService
    {
        private ILogger logger;
        private ISet<string> excelMimeTypes = new HashSet<string> {
            "application/vnd.ms-excel",                                            // [.xls, .xlt, .xla]
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",   // [.xlsx]
            "application/vnd.openxmlformats-officedocument.spreadsheetml.template" // [.xltx]
        };

        /// <summary>
        /// Crea una nueva instancia con el logger indicado.
        /// </summary>
        /// <param name="logger">Una instancia de la interfaz ILogger.</param>
        public HomeService(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Lee y valida el archivo de Excel desde el cliente y regresa los registros del archivo como una colección de objetos.
        /// Si el archivo es nulo, no es del tipo MIME indicado para Excel o el proceso arroja alguna excepción regresa un mensaje
        /// de error y con estatus False. Si el proceso fue exitoso, regresa un mensaje vacío con estatus True. En cualquier caso
        /// regresa una colección, vacía si el proceso falló, o de lo contrario rellenada con los registros del archivo 
        /// </summary>
        /// <param name="archivo">El archivo enviado desde el cliente.</param>
        /// <param name="tempDir">El directorio temporal de la aplicación para poder escribir el archivo.</param>
        /// <param name="resultado">Contiene el resultado (exitoso o fallido) de la operación de lectura.</param>
        /// <returns>Una colección de objectos Registro y un mensaje con el resultado de la operación.</returns>
        public IEnumerable<Certificado> LeerArchivo(HttpPostedFileBase archivo, string tempDir, out Message resultado)
        {
            var validadoMessage = ValidarArchivo(archivo);
            var clase = GetType().FullName;
            var metodo = MethodBase.GetCurrentMethod();

            if (!validadoMessage.Success) {
                logger.WarningLog("Archivo de certificados Excel no validado.", clase, validadoMessage.Mensaje, metodo, archivo?.FileName, tempDir);

                resultado = validadoMessage;
                return new Certificado[] { };
            }

            try {
                var registros = LeerArchivo(archivo, tempDir);
                resultado = new Message(true, string.Empty);
                logger.InfoLog("Archivo de certificados Excel leído con éxito.", clase, "Ejecución realizada con éxito.", metodo, archivo.FileName, tempDir);

                return registros;
            } catch {
                resultado = new Message(false, "Ocurrió un error al leer el archivo. Por favor, verifique su archivo e intente de nuevo.");
                return new Certificado[] { };
            }
        }

        /// <summary>
        /// Lee y valida el archivo de Excel desde el cliente y ejecuta los procedimientos necesarios para la generación
        /// del lote de Decenal en forma de un nuevo archivo de Excel, el cual se almacena en el directorio ~/LotesGenerados.
        /// Si el archivo es nulo, no es del tipo MIME indicado para Excel o el proceso arroja alguna excepción regresa un mensaje
        /// de error y con estatus False. Si el proceso fue exitoso, regresa un mensaje vacío con estatus True y el nombre del nuevo
        /// archivo de Excel generado.
        /// </summary>
        /// <param name="archivo">El archivo Excel enviado desde el cliente.</param>
        /// <param name="tempDir">El directorio temporal de la aplicación para poder escribir el archivo.</param>
        /// <param name="lotesDir">El directorio para almacenar el lote generado.</param>
        /// <returns>Un mensaje con el resultado de la operación.</returns>
        public Message LeerYProcesarRegistros(HttpPostedFileBase archivo, string tempDir, string lotesDir)
        {
            var clase = GetType().FullName;
            var metodo = MethodBase.GetCurrentMethod();

            try {
                Message message;
                var registros = LeerArchivo(archivo, tempDir, out message);

                if (!message.Success) {
                    return message;
                }

                var resultado = ProcesarYEscribirRegistros(registros, Path.GetFileNameWithoutExtension(archivo.FileName), lotesDir);
                logger.SuccessLog("Certificados de Excel procesados con éxito",
                    clase, $"{resultado.Mensaje} => {resultado.Valor}", metodo, archivo.FileName, tempDir, lotesDir);

                return resultado;
            } catch {
                return new Message(false, "Ocurrió un error al leer el archivo. Por favor, verifique su archivo e intente de nuevo.");
            }
        }

        /// <summary>
        /// Valida que el archivo indicado no sea nulo y sea del tipo MIME indicado por <code>excelMimeTypes.</code>
        /// Si es válido, regresa un mensaje vacío y un status de True. De lo contrario, regresa un
        /// mensaje de error y un status de False.
        /// </summary>
        /// <param name="archivo">El archivo a validar.</param>
        /// <returns>Un mensaje y estatus dependiendo de la validez del archivo.</returns>
        private Message ValidarArchivo(HttpPostedFileBase archivo)
        {
            if (archivo == null)
                return new Message(false, "Por favor, seleccione un archivo.");

            if (!excelMimeTypes.Contains(archivo.ContentType.ToLowerInvariant()))
                return new Message(false, "Solo se aceptan archivos de Excel.");

            return new Message(true, string.Empty);
        }

        /// <summary>
        /// Lee el archivo de Excel desde el cliente y regresa los registros del archivo como una
        /// colección de objetos Registro. Arroja excepción ocurre algún error al leer
        /// o escribir el archivo.
        /// </summary>
        /// <param name="archivo">El archivo enviado desde el cliente.</param>
        /// <param name="tempDir">El directorio temporal de la aplicación para poder escribir el archivo.</param>
        /// <returns>Una colección de objetos <code>Registro</code>.</returns>
        private IEnumerable<Certificado> LeerArchivo(HttpPostedFileBase archivo, string tempDir)
        {
            try {
                IEnumerable<Certificado> registros;
                var deletableDir = Path.Combine(tempDir, Guid.NewGuid().ToString());

                Directory.CreateDirectory(deletableDir);
                var tempFile = Path.Combine(deletableDir, archivo.FileName);
                archivo.SaveAs(tempFile);

                using (IExcelIO lector = ObtenerLector(archivo.ContentType.ToLowerInvariant(), tempFile)) {
                    registros = lector.LeerArchivo();
                }

                Directory.Delete(deletableDir, true);
                return registros;
            } catch (Exception e) {
                var clase = GetType().FullName;
                var metodo = MethodBase.GetCurrentMethod();

                logger.ErrorLog(
                    "Error en lectura de archivo de certificados Excel",
                    clase,
                    e.ToString(),
                    metodo,
                    archivo?.FileName,
                    tempDir);

                throw;
            }
        }

        /// <summary>
        /// Regresa un lector apropiado para el tipo de archivo Excel que se maneje.
        /// </summary>
        /// <param name="contentType">El tipo MIME del archivo de Excel.</param>
        /// <param name="rutaArchivo">La ruta completa hacia el archivo de Excel.</param>
        /// <returns>Una nueva instancia de <see cref="IExcelIO"/>.</returns>
        private IExcelIO ObtenerLector(string contentType, string rutaArchivo)
        {
            if (contentType == "application/vnd.ms-excel") {
                return new OleDbReader(rutaArchivo);
            }

            return new EPPlusReaderWriter(rutaArchivo);
        }

        /// <summary>
        /// Procesa los registros leídos del archivo de Excel y escribe el lote generado en el directorio de lotes.
        /// Regresa un mensaje de éxito con el nombre y extensión del nuevo lote generado.
        /// </summary>
        /// <param name="registros">Los registros leídos del archivo de Excel.</param>
        /// <param name="nombreArchivo">EL nombre del archivo sin extensión.</param>
        /// <param name="lotesDir">El directorio de la aplicación para escribir el lote de certificados generado.</param>
        /// <returns>Un mensaje con el resultado de la ejecución.</returns>
        private MessageWithValue<string> ProcesarYEscribirRegistros(IEnumerable<Certificado> registros, string nombreArchivo, string lotesDir)
        {
            var fechaInicio = DateTime.Now;
            IEnumerable<PolizaEndosoLote> lote;

            TimeSpan tiempoProcesamiento = MedirTiempoProcesamiento(regs => {
                var agrupados = regs.ToLookup(registro => registro.Poliza);
                return new HomeDao(logger).ObtenerNumerosLote(agrupados);
            }, registros, out lote);

            var fechaTermino = fechaInicio.Add(tiempoProcesamiento);
            var nuevoNombreArchivo = $"{nombreArchivo}_LOTE_{fechaTermino.ToString("dd-MM-yyyy hh-mm-ss tt")}.xlsx";

            using (var escritor = new EPPlusReaderWriter(logger)) {
                escritor.EscribirArchivo(
                    Path.Combine(lotesDir, nuevoNombreArchivo),
                    lote,
                    fechaInicio,
                    fechaTermino,
                    tiempoProcesamiento);
            }

            return new MessageWithValue<string>(
                true,
                "Los certificados se procesaron con éxito y su respectivo lote ha sido generado.",
                nuevoNombreArchivo);
        }

        /// <summary>
        /// Mide el tiempo de procesamiento de una determinada función y regresa este tiempo junto al resultado
        /// de la ejecución de la función.
        /// </summary>
        /// <typeparam name="T">EL tipo de dato del parámetro de la función.</typeparam>
        /// <typeparam name="TResult">El tipo de dato del resultado de la ejecución de la función.</typeparam>
        /// <param name="procesamiento">La función a medir.</param>
        /// <param name="parametro">El parámetro que recibe la función.</param>
        /// <param name="resultadoProcesamiento">Almacena el resultado de la ejecución de la función.</param>
        /// <returns></returns>
        private TimeSpan MedirTiempoProcesamiento<T, TResult>(Func<T, TResult> procesamiento, T parametro, out TResult resultadoProcesamiento)
        {
            var watch = Stopwatch.StartNew();
            resultadoProcesamiento = procesamiento(parametro);
            watch.Stop();

            return watch.Elapsed;
        }
    }
}