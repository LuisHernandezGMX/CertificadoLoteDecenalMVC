using System;
using System.Collections.Generic;

namespace CertificadoLoteDecenalMVC.Models.Business.Logging
{
    public enum TipoResultado
    {
        Success,
        Info,
        Warning,
        Error
    }

    public class Log
    {
        public class LogUbicacion
        {
            public string Clase { get; set; }
            public LogMetodo Metodo { get; set; }
        }

        public class LogMetodo
        {
            public string Nombre { get; set; }
            public IEnumerable<LogArgumento> Argumentos { get; set; }
        }

        public class LogArgumento
        {
            public string TipoDato { get; set; }
            public string Parametro { get; set; }
            public string Valor { get; set; }
        }

        public class LogResultado
        {
            public string Mensaje { get; set; }
            public TipoResultado Tipo { get; set; }
        }

        public string Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public LogUbicacion Ubicacion { get; set; }
        public LogResultado Resultado { get; set;  }
    }

    public class LogList
    {
        public List<Log> Logs { get; set; }
    }
}