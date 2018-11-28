using System;
using System.Collections.Generic;

namespace CertificadoLoteDecenalMVC.Models.Business.Logging
{
    /// <summary>
    /// Representa las operaciones del logger de la aplicación. El logger
    /// genera un nuevo conjunto de logs por día.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Añade una nueva entrada al log.
        /// </summary>
        /// <param name="log">El nuevo registro a guardar.</param>
        void Log(Log log);

        /// <summary>
        /// Regresa una colección de todas las fechas en los que se han generado registros.
        /// Las fechas se regresan en el formato "yyyy-MM-dd".
        /// </summary>
        /// <returns>Una colección de fechas en forma de cadenas.</returns>
        IEnumerable<string> GetLogDays();

        /// <summary>
        /// Regresa una colección con todos logs almacenados de la fecha indicada.
        /// La fecha debe tener el formato "yyyy-MM-dd".
        /// </summary>
        /// <param name="fecha">La fecha del log que se quiere consultar.</param>
        /// <returns>Una colección de objetos Log.</returns>
        IEnumerable<Log> GetLogs(string fecha);

        /// <summary>
        /// Regresa una colección de todos los logs almacenados de la fecha indicada
        /// que cumplan con el predicado indicado. La fecha debe tener el formato "yyyy-MM-dd".
        /// </summary>
        /// <param name="predicado">Condición que deben cumplir los logs que serán regresados.</param>
        /// <returns>Una colección filtrada de objetos Log.</returns>
        IEnumerable<Log> GetLogs(string fecha, Func<Log, bool> predicado);
    }
}