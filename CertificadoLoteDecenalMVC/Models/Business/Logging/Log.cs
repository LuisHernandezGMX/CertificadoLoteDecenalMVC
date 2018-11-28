using System;
using System.Collections.Generic;

namespace CertificadoLoteDecenalMVC.Models.Business.Logging
{
    /// <summary>
    /// Indica el tipo de resultado del evento del log.
    /// </summary>
    public enum TipoResultado
    {
        /// <summary>
        /// Una operación de negocio se realizó con éxito.
        /// </summary>
        Success,

        /// <summary>
        /// Información de interés sobre algún proceso no esencial. 
        /// </summary>
        Info,

        /// <summary>
        /// Representa una advertencia de negocio al realizar un proceso.
        /// </summary>
        Warning,

        /// <summary>
        /// Ocurrió una excepción al realizar un proceso.
        /// </summary>
        Error
    }

    /// <summary>
    /// Representa una entrada del log.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Representa la ubicación en código en donde ocurrió el evento.
        /// </summary>
        public class LogUbicacion
        {
            /// <summary>
            /// El nombre de la clase donde ocurrió el evento.
            /// </summary>
            public string Clase { get; set; }

            /// <summary>
            /// El método donde ocurrió el evento.
            /// </summary>
            public LogMetodo Metodo { get; set; }
        }

        /// <summary>
        /// Representa el método donde ocurrió el evento.
        /// </summary>
        public class LogMetodo
        {
            /// <summary>
            /// El nombre del método.
            /// </summary>
            public string Nombre { get; set; }

            /// <summary>
            /// Los argumentos del método.
            /// </summary>
            public IEnumerable<LogArgumento> Argumentos { get; set; }
        }

        /// <summary>
        /// Representa uno de los argumentos del método donde ocurrió el evento.
        /// </summary>
        public class LogArgumento
        {
            /// <summary>
            /// EL tipo de dato de este argumento.
            /// </summary>
            public string TipoDato { get; set; }

            /// <summary>
            /// El nombre de este argumento.
            /// </summary>
            public string Parametro { get; set; }

            /// <summary>
            /// El valor del argumento al momento del evento.
            /// </summary>
            public string Valor { get; set; }
        }

        /// <summary>
        /// Representa el resultado del evento.
        /// </summary>
        public class LogResultado
        {
            /// <summary>
            /// Información adicional acerca del evento.
            /// </summary>
            public string Mensaje { get; set; }

            /// <summary>
            /// El tipo de evento ocurrido.
            /// </summary>
            public TipoResultado Tipo { get; set; }
        }

        /// <summary>
        /// El Id único de este evento.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// La fecha en que ocurrió este evento.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Breve descripción acerca del evento.
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// La ubicación donde ocurrió este evento.
        /// </summary>
        public LogUbicacion Ubicacion { get; set; }

        /// <summary>
        /// El tipo de resultado de este evento.
        /// </summary>
        public LogResultado Resultado { get; set;  }
    }

    /// <summary>
    /// Clase auxiliar para la serialización de logs.
    /// </summary>
    public class LogList
    {
        /// <summary>
        /// Incluye una porción o todos los logs.
        /// </summary>
        public List<Log> Logs { get; set; }
    }
}