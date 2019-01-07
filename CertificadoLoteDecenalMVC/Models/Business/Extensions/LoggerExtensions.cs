using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using CertificadoLoteDecenalMVC.Models.Business.Logging;

namespace CertificadoLoteDecenalMVC.Models.Business.Extensions
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// Genera un nuevo registro de error en el Log. Utiliza <see cref="MethodBase"/> para obtener los argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="mensajeResultado">Información acerca del resultado del evento.</param>
        /// <param name="metodo">El método donde se produjo el evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void ErrorLog(this ILogger logger, string descripcion, string clase, string mensajeResultado, MethodBase metodo, params object[] argumentos)
        {
            NewLog(logger, descripcion, clase, new Log.LogResultado {
                Tipo = TipoResultado.Error,
                Mensaje = mensajeResultado
            }, metodo, argumentos);
        }

        /// <summary>
        /// Genera un nuevo registro de advertencia en el Log. Utiliza <see cref="MethodBase"/> para obtener los argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="mensajeResultado">Información acerca del resultado del evento.</param>
        /// <param name="metodo">El método donde se produjo el evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void WarningLog(this ILogger logger, string descripcion, string clase, string mensajeResultado, MethodBase metodo, params object[] argumentos)
        {
            NewLog(logger, descripcion, clase, new Log.LogResultado {
                Tipo = TipoResultado.Warning,
                Mensaje = mensajeResultado
            }, metodo, argumentos);
        }

        /// <summary>
        /// Genera un nuevo registro de advertencia en el Log. Utiliza los objetos del <see cref="IEnumerable{Log.LogArgumento}"/>
        /// como argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="metodo">El nombre del método que produjo el evento.</param>
        /// <param name="mensajeResultado">Información acerca del resultado del evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void WarningLog(this ILogger logger, string descripcion, string clase, string metodo, string mensajeResultado, IEnumerable<Log.LogArgumento> argumentos = null)
        {
            NewLog(logger, descripcion, clase, metodo, new Log.LogResultado {
                Tipo = TipoResultado.Warning,
                Mensaje = mensajeResultado
            }, argumentos);
        }

        /// <summary>
        /// Genera un nuevo registro de información en el Log. Utiliza <see cref="MethodBase"/> para obtener los argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="mensajeResultado">Información acerca del resultado del evento.</param>
        /// <param name="metodo">El método donde se produjo el evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void InfoLog(this ILogger logger, string descripcion, string clase, string mensajeResultado, MethodBase metodo, params object[] argumentos)
        {
            NewLog(logger, descripcion, clase, new Log.LogResultado {
                Tipo = TipoResultado.Info,
                Mensaje = mensajeResultado
            }, metodo, argumentos);
        }

        /// <summary>
        /// Genera un nuevo registro de información en el Log. Utiliza los objetos del <see cref="IEnumerable{Log.LogArgumento}"/>
        /// como argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="metodo">El nombre del método que produjo el evento.</param>
        /// <param name="mensajeResultado">Información acerca del resultado del evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void InfoLog(this ILogger logger, string descripcion, string clase, string metodo, string mensajeResultado, IEnumerable<Log.LogArgumento> argumentos = null)
        {
            NewLog(logger, descripcion, clase, metodo, new Log.LogResultado {
                Tipo = TipoResultado.Info,
                Mensaje = mensajeResultado
            }, argumentos);
        }

        /// <summary>
        /// Genera un nuevo registro de éxito en el Log. Utiliza <see cref="MethodBase"/> para obtener los argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="mensajeResultado">Información acerca del resultado del evento.</param>
        /// <param name="metodo">El método donde se produjo el evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void SuccessLog(this ILogger logger, string descripcion, string clase, string mensajeResultado, MethodBase metodo, params object[] argumentos)
        {
            NewLog(logger, descripcion, clase, new Log.LogResultado {
                Tipo = TipoResultado.Success,
                Mensaje = mensajeResultado
            }, metodo, argumentos);
        }

        /// <summary>
        /// Genera un nuevo registro en el Log. Utiliza <see cref="MethodBase"/> para obtener los argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="resultado">Información acerca del resultado del evento.</param>
        /// <param name="metodo">El método donde se produjo el evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void NewLog(this ILogger logger, string descripcion, string clase, Log.LogResultado resultado, MethodBase metodo, params object[] argumentos)
        {
            int i = 0;
            var args = metodo
                .GetParameters()
                .Where(param => !param.IsOut)
                .Select(param => new Log.LogArgumento {
                    Parametro = param.Name,
                    TipoDato = param.ParameterType.ToString(),
                    Valor = argumentos[i++]?.ToString() ?? "NULL"
                })
                .ToList();

            logger.Log(new Log {
                Id = Guid.NewGuid().ToString(),
                Fecha = DateTime.Now,
                Descripcion = descripcion,
                Ubicacion = new Log.LogUbicacion {
                    Clase = clase,
                    Metodo = new Log.LogMetodo {
                        Nombre = metodo.Name,
                        Argumentos = args
                    }
                },
                Resultado = resultado
            });
        }

        /// <summary>
        /// Genera un nuevo registro en el Log. Utiliza los objetos del <see cref="IEnumerable{Log.LogArgumento}"/>
        /// como argumentos del método.
        /// </summary>
        /// <param name="logger">El logger que almacenará el nuevo registro.</param>
        /// <param name="descripcion">La descripción del evento a registrar.</param>
        /// <param name="clase">La clase a la cual pertenece el método que produjo el evento.</param>
        /// <param name="metodo">El nombre del método que produjo el evento.</param>
        /// <param name="resultado">Información acerca del resultado del evento.</param>
        /// <param name="argumentos">Los valores de los parámetros del método al momento del evento.</param>
        public static void NewLog(this ILogger logger, string descripcion, string clase, string metodo, Log.LogResultado resultado, IEnumerable<Log.LogArgumento> argumentos = null)
        {
            logger.Log(new Log {
                Id = Guid.NewGuid().ToString(),
                Fecha = DateTime.Now,
                Descripcion = descripcion,
                Ubicacion = new Log.LogUbicacion {
                    Clase = clase,
                    Metodo = new Log.LogMetodo {
                        Nombre = metodo,
                        Argumentos = argumentos ?? Enumerable.Empty<Log.LogArgumento>()
                    }
                },
                Resultado = resultado
            });
        }
    }
}