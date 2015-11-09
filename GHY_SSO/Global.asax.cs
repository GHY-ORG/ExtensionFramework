using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Web;
using System.Web.Http;

namespace GHY_SSO
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SiteConfig.SiteUrl = "http://ghy.cn/GHY_SSO";
            SiteConfig.SitePath = "E:\\web\\GHY_SSO\\";// HttpContext.Current.Server.MapPath("~");

            //设置MEF依赖注入容器
            DirectoryCatalog catalog = new DirectoryCatalog(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            MefDependencySolver solver = new MefDependencySolver(catalog);
            DependencyResolver.SetResolver(solver);
        }
    }
}