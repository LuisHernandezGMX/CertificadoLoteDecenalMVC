using Microsoft.AspNet.Identity;

namespace CertificadoLoteDecenalMVC.Models.DAL.Entities
{
    /// <summary>
    /// Representa a un usuario dentro del LDAP de GMX. Es equivalente
    /// a la clase <see cref="System.DirectoryServices.AccountManagement.UserPrincipal"/>.
    /// </summary>
    public class IdentityUser : IUser
    {
        /// <summary>
        /// Id único del usuario. Dentro de LDAP este campo
        /// es su [GUID].
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nombre de usuario único. Dentro de LDAP este campo
        /// es su [SamAccountName].
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Nombre completo del usuario. Dentro de LDAP este campo
        /// es su [Name].
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Correo electrónico del usuario. Dentro de LDAP este campo
        /// es su [EmailAddress].
        /// </summary>
        public string Email { get; set; }
    }
}