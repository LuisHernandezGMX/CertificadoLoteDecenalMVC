using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CertificadoLoteDecenalMVC.Models.Business.Logging
{
    /// <summary>
    /// Logger nulo. No implementa ninguna funcionalidad; solamente
    /// imprime mensajes en el flujo de salida.
    /// Usarse únicamente con fines de prueba.
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Imprime un mensaje al flujo de depuración.
        /// </summary>
        /// <param name="log">Ignorado.</param>
        public void Log(Log log)
        {
            Debug.WriteLine("Método Log() del logger nulo.");
        }

        /// <summary>
        /// Imprime un mensaje al flujo de depuración
        /// y regresa una colección vacía.
        /// </summary>
        /// <returns>Una colección vacía.</returns>
        public IEnumerable<string> GetLogDays()
        {
            Debug.WriteLine("Método GetLogDays() del logger nulo.");
            return new string[] { };
        }

        /// <summary>
        /// Imprime un mensaje al flujo de depuración
        /// y regresa una colección vacía.
        /// </summary>
        /// <returns>Una colección vacía.</returns>
        public IEnumerable<Log> GetLogs(string fecha)
        {
            Debug.WriteLine("Método GetLogs() del logger nulo.");
            return new Log[] { };
        }

        /// <summary>
        /// Imprime un mensaje al flujo de depuración
        /// y regresa una colección vacía.
        /// </summary>
        /// <param name="predicado">Ignorado</param>
        /// <returns>Una colección vacía.</returns>
        public IEnumerable<Log> GetLogs(string fecha, Func<Log, bool> predicado)
        {
            Debug.WriteLine("Método GetLogs(Func<Log, bool>) del logger nulo.");
            return new Log[] { };
        }
    }
}