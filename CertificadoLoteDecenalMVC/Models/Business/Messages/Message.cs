namespace CertificadoLoteDecenalMVC.Models.Business.Messages
{
    /// <summary>
    /// Representa el resultado de una operación con un estatus de True o False
    /// y un mensaje con información adicional.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Representa si la operación realizada fue exitosa o no.
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// Incluye información adicional acerca del resultado de la operación.
        /// </summary>
        public string Mensaje { get; protected set; }

        /// <summary>
        /// Crea un nuevo mensaje con su respectivo estatus.
        /// </summary>
        /// <param name="success">El estatus de la operación realizada.</param>
        /// <param name="mensaje">Información adicional acerca de la operación.</param>
        public Message(bool success, string mensaje)
        {
            Success = success;
            Mensaje = mensaje;
        }
    }

    /// <summary>
    /// Representa el resultado de una operación con un estatus de True o False,
    /// un mensaje con información complementaria y un valor adicional.
    /// </summary>
    /// <typeparam name="T">EL tipo de valor adicional que incluye este mensaje.</typeparam>
    public class MessageWithValue<T> : Message
    {
        /// <summary>
        /// Valor adicional al mensaje.
        /// </summary>
        public T Valor { get; private set; }

        /// <summary>
        /// Crea un nuevo mensaje con su respectivo estatus y valor(es) adicional(es).
        /// </summary>
        /// <param name="status">El estatus de la operación realizada.</param>
        /// <param name="mensaje">Información adicional acerca de la operación.</param>
        /// <param name="valor">Valor adicional al mensaje.</param>
        public MessageWithValue(bool success, string mensaje, T valor) : base(success, mensaje)
        {
            Valor = valor;
        }
    }
}