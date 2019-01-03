using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using Microsoft.AspNet.Identity;

namespace CertificadoLoteDecenalMVC.Models.DAL.Entities
{
    /// <summary>
    /// UserStore utilizado para la correcta implementación de Identity
    /// utilizando ActiveDirectory como capa de persistencia. Este store
    /// implementa únicamente los métodos para las funciones de autenticación
    /// y autorización. NO PUEDE realizar modificaciones por medio de LDAP.
    /// </summary>
    public class UserStore : IUserStore<IdentityUser>, IUserRoleStore<IdentityUser>
    {
        /// <summary>
        /// Conexión principal al LDAP de GMX.
        /// </summary>
        private PrincipalContext activeDirectory;

        /// <summary>
        /// Inicializa una nueva conexión al ActiveDirectory de GMX.
        /// </summary>
        public UserStore()
        {
            activeDirectory = new PrincipalContext(ContextType.Domain, "GMX.COM.MX");
        }

        /// <summary>
        /// Verifica que el nombre de usuario y su contraseña sean correctas para su
        /// respectiva cuenta de LDAP.
        /// </summary>
        /// <param name="userName">El nombre de usuario único.</param>
        /// <param name="password">Su contraseña.</param>
        /// <returns>True si las credenciales son correctas. False si el nombre de usuario o la contraseña son erróneas.</returns>
        public Task<bool> Autenticar(string userName, string password)
        {
            bool autenticado = activeDirectory.ValidateCredentials(userName, password);

            return Task.FromResult(autenticado);
        }

        #region IUserStore

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            IdentityUser user = null;
            var userPrincipal = UserPrincipal.FindByIdentity(activeDirectory, IdentityType.Guid, userId);

            if (userPrincipal != null) {
                user = new IdentityUser {
                    Id = userPrincipal.Guid.ToString(),
                    UserName = userPrincipal.SamAccountName,
                    Nombre = userPrincipal.Name,
                    Email = userPrincipal.EmailAddress
                };
            }

            return Task.FromResult(user);
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            IdentityUser user = null;
            var userPrincipal = UserPrincipal.FindByIdentity(activeDirectory, IdentityType.SamAccountName, userName);

            if (userPrincipal != null) {
                user = new IdentityUser {
                    Id = userPrincipal.Guid.ToString(),
                    UserName = userPrincipal.SamAccountName,
                    Nombre = userPrincipal.Name,
                    Email = userPrincipal.EmailAddress
                };
            }

            return Task.FromResult(user);
        }

        /// <summary>
        /// Implementación NO NECESARIA. Esta aplicación
        /// utiliza Identity únicamente para autenticación
        /// y autorización de usuarios.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementación NO NECESARIA. Esta aplicación
        /// utiliza Identity únicamente para autenticación
        /// y autorización de usuarios.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementación NO NECESARIA. Esta aplicación
        /// utiliza Identity únicamente para autenticación
        /// y autorización de usuarios.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task UpdateAsync(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        #endregion        

        #region IUserRoleStore

        /// <summary>
        /// Implementación NO NECESARIA. Esta aplicación
        /// utiliza Identity únicamente para autenticación
        /// y autorización de usuarios.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implementación NO NECESARIA. Esta aplicación
        /// utiliza Identity únicamente para autenticación
        /// y autorización de usuarios.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            var roles = UserPrincipal
                .FindByIdentity(activeDirectory, IdentityType.Guid, user.Id)
                .GetGroups()
                .Select(group => group.SamAccountName)
                .ToList();

            return Task.FromResult(roles as IList<string>);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            bool isInRole = UserPrincipal
                .FindByIdentity(activeDirectory, IdentityType.Guid, user.Id)
                .IsMemberOf(GroupPrincipal.FindByIdentity(activeDirectory, roleName));

            return Task.FromResult(isInRole);
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    activeDirectory.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}