using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(CertificadoLoteDecenalMVC.Startup))]
namespace CertificadoLoteDecenalMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
