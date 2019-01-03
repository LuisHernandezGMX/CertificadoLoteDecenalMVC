using Microsoft.AspNet.Identity;

namespace CertificadoLoteDecenalMVC.Models.DAL.Entities
{
    /// <summary>
    /// Representa a los grupos LDAP a los que pertenecen los
    /// usuarios de GMX. Es equivalente a la clase
    /// <see cref="System.DirectoryServices.AccountManagement.Principal"/>.
    /// </summary>
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Identificador único del rol. Dentro de LDAP este
        /// campo es su [GUID].
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// El nombre del rol. Dentro de LDAP este campo
        /// es su [SamAccountName].
        /// </summary>
        public string Name { get; set; }
    }
}