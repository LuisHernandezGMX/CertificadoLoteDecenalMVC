using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CertificadoLoteDecenalMVC.Models.Business.Logging
{
    /// <summary>
    /// Registra los nuevos logs dentro de un archivo Json.
    /// Esta clase puede escribir en el log de forma sincronizada (Thread-safe).
    /// </summary>
    public class JsonLogger : ILogger
    {
        private readonly object syncObj = new object();
        private readonly string rutaLogs;

        /// <summary>
        /// Crea un nuevo logger con la ruta al archivo de logs indicado.
        /// </summary>
        /// <param name="rutaLogs">La ruta donde se almacenan los logs en formato JSON.</param>
        public JsonLogger(string rutaLogs)
        {
            this.rutaLogs = rutaLogs;
        }

        /// <summary>
        /// Representa las operaciones del logger de la aplicación.
        /// </summary>
        public void Log(Log log)
        {
            lock (syncObj) {
                var fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
                var rutaArchivo = Path.Combine(rutaLogs, $"{fechaActual}.json");

                if (File.Exists(rutaArchivo)) {
                    var json = File.ReadAllText(rutaArchivo);
                    var logList = JsonConvert.DeserializeObject<LogList>(json);

                    logList.Logs.Add(log);
                    json = JsonConvert.SerializeObject(logList, Formatting.Indented);
                    File.WriteAllText(rutaArchivo, json);
                } else {
                    var json = JsonConvert.SerializeObject(new {
                        Logs = new[] { log }
                    }, Formatting.Indented);

                    File.WriteAllText(rutaArchivo, json);
                }
            }
        }

        /// <summary>
        /// Regresa una colección de todos los días en los que se han generado registros.
        /// Las fechas se regresan en el formato "yyyy-MM-dd".
        /// </summary>
        /// <returns>Una colección de fechas en forma de cadenas.</returns>
        public IEnumerable<string> GetLogDays()
        {
            return Directory
                .EnumerateFiles(rutaLogs)
                .Where(log => !log.Contains("PseudoEsquema"))
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        /// <summary>
        /// Regresa una colección con todos logs almacenados de la fecha indicada.
        /// La fecha debe tener el formato "yyyy-MM-dd".
        /// </summary>
        /// <param name="fecha">La fecha del log que se quiere consultar.</param>
        /// <returns>Una colección de objetos Log.</returns>
        public IEnumerable<Log> GetLogs(string fecha)
        {
            var rutaArchivo = Path.Combine(rutaLogs, $"{fecha}.json");

            if (File.Exists(rutaArchivo)) {
                var json = File.ReadAllText(rutaArchivo);
                return JsonConvert.DeserializeObject<LogList>(json).Logs;
            }

            return new Log[] { };
        }

        /// <summary>
        /// Regresa una colección de todos los logs almacenados de la fecha indicada
        /// que cumplan con el predicado indicado. La fecha debe tener el formato "yyyy-MM-dd".
        /// </summary>
        /// <param name="predicado">Condición que deben cumplir los logs que serán regresados.</param>
        /// <returns>Una colección filtrada de objetos Log.</returns>
        public IEnumerable<Log> GetLogs(string fecha, Func<Log, bool> predicado)
        {
            return GetLogs(fecha)
                .Where(predicado);
        }
    }
}