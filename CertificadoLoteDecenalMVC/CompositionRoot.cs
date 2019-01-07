using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CertificadoLoteDecenalMVC.Controllers;
using CertificadoLoteDecenalMVC.Models.Business.Logging;

namespace CertificadoLoteDecenalMVC
{
    /// <summary>
    /// Tipo de logger a utilizar.
    /// </summary>
    public enum LoggerType
    {
        /// <summary>
        /// Logger nulo. Solamente para fines de prueba.
        /// </summary>
        Null,
        /// <summary>
        /// Logger basado en archivos JSON.
        /// </summary>
        Json
    }

    /// <summary>
    /// Implementación del patrón Inyección de Dependencias Puro (Pure DI).
    /// Referencias: https://stackoverflow.com/questions/32032771/using-dependency-injection-without-any-di-library
    /// </summary>
    public sealed class CompositionRoot : DefaultControllerFactory
    {
        private static ILogger logger = GetLogger(LoggerType.Json);

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == typeof(HomeController))
                return new HomeController(logger);

            if (controllerType == typeof(LogController))
                return new LogController(logger);

            if (controllerType == typeof(AccountController))
                return new AccountController(logger);

            return base.GetControllerInstance(requestContext, controllerType);
        }

        /// <summary>
        /// Regresa una instancia que implementa la interfaz ILogger.
        /// </summary>
        /// <param name="loggerType">El tipo de logger a utilizar</param>
        /// <returns>Un objeto ILogger.</returns>
        private static ILogger GetLogger(LoggerType loggerType)
        {
            ILogger logger = null;

            switch (loggerType) {
                case LoggerType.Json:
                    var rutaLog = HttpContext.Current.Server.MapPath("~/Logs");
                    logger = new JsonLogger(rutaLog);
                    break;
                case LoggerType.Null: default:
                    logger = new NullLogger();
                    break;
            }

            return logger;
        }
    }
}