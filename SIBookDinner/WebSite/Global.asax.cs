using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;//从验证票据获取Cookie的名字。
            HttpCookie authCookie = Context.Request.Cookies[cookieName];
            if (null == authCookie)
            {
                return;
            }
            //获取验证票据。
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null)
            {
                return;
            }
            string[] roles = authTicket.UserData.Split(new char[] { ',' });
            FormsIdentity id = new FormsIdentity(authTicket);
            System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(id, roles);//把生成的验证票信息和角色信息赋给当前用户．
            Context.User = principal;
        }
    }
}
