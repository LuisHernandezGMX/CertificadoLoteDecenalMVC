using System.Web.Optimization;

namespace CertificadoLoteDecenalMVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /***************************************************** GENERAL *****************************************************/
            bundles.Add(new ScriptBundle("~/bundles/General").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/fontawesome/all.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables").Include(
                "~/Scripts/datatables.js"));

            bundles.Add(new StyleBundle("~/Content/General").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/fontawesome/all.css"));

            bundles.Add(new StyleBundle("~/Content/DataTables").Include(
                "~/Content/datatables.css"));
            /***************************************************** GENERAL *****************************************************/

            /***************************************************** CONTROLLER: Home *****************************************************/
            bundles.Add(new ScriptBundle("~/bundles/Home/LayoutCertificados").Include(
                "~/Scripts/Home/LayoutCertificados.js"));
            /***************************************************** CONTROLLER: Home *****************************************************/

            /***************************************************** CONTROLLER: Log *****************************************************/
            bundles.Add(new ScriptBundle("~/bundles/Log/Details").Include(
                "~/Scripts/datatables.js",
                "~/Scripts/bootstrap-dialog.js",
                "~/Scripts/Log/Details.js"));

            bundles.Add(new StyleBundle("~/Content/Log/Details").Include(
                "~/Content/datatables.css",
                "~/Content/bootstrap-dialog.css"));
            /***************************************************** CONTROLLER: Log *****************************************************/
        }
    }
}
