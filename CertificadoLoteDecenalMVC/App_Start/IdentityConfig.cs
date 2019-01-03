using System.Threading.Tasks;
using System.Data.Entity.Utilities;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CertificadoLoteDecenalMVC.Models.DAL.Entities;

namespace CertificadoLoteDecenalMVC
{
    public class ApplicationUserManager : UserManager<IdentityUser>
    {
        public ApplicationUserManager(IUserStore<IdentityUser> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore());

            manager.UserLockoutEnabledByDefault = false;
            manager.UserValidator = new UserValidator<IdentityUser>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            manager.PasswordValidator = new PasswordValidator {};

            if (options.DataProtectionProvider != null) {
                var dataProtector = options.DataProtectionProvider.Create("ASP.NET Identity");
                manager.UserTokenProvider = new DataProtectorTokenProvider<IdentityUser>(dataProtector);
            }

            return manager;
        }

        public override Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            var store = Store as UserStore;

            return store.Autenticar(user.UserName, password);
        }
    }

    public class ApplicationSignInManager : SignInManager<IdentityUser, string>
    {
        public ApplicationSignInManager(UserManager<IdentityUser, string> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            if (UserManager == null) {
                return SignInStatus.Failure;
            }

            var user = await UserManager.FindByNameAsync(userName).WithCurrentCulture();

            if (user == null) {
                return SignInStatus.Failure;
            }

            if (await UserManager.CheckPasswordAsync(user, password).WithCurrentCulture()) {
                await SignInAsync(user, isPersistent, false).WithCurrentCulture();
                return SignInStatus.Success;
            }

            return SignInStatus.Failure;
        }
    }
}