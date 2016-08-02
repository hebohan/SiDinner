using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WebSite.Models;
using Rule;

namespace WebSite.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }
        #endregion

        #region 登录页面
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {

            return View();
        }
        #endregion

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Login")]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.StaffName, model.StaffPwd))
                {
                    FormsService.SignIn(model.StaffName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (Convert.ToInt32(Session["StaffPower"]) == 1)
                            return RedirectToAction("Index", "Home");
                        else
                            return RedirectToAction("About", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "提供的用户名或密码不正确。");
                }
            }
            return View(model);
        }
        #endregion

        #region 注销
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 编辑用户信息中的修改密码
        /// </summary>
        public ActionResult ChangePassword(int ID)
        {
            return View();
        }

        [HttpPost, ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admins")]
        public ActionResult ChangePasswordConfirmed(int ID, string OldPwd, string NewPwd, string ConfirmPwd)
        {
            string Password = GetInfoManager.GetPwd(ID);
            string Pwd = SIStudio.Framework.SISecurity.MD5(OldPwd);
            if (string.Compare(Password, Pwd, true)==0)
            {
                if(NewPwd==ConfirmPwd)
                {
                    UpdateManager.UpdateStaffPwd(ID, NewPwd);
                    //return Content("<script >alert('密码修改成功！');window.location.href='/Home/Index'</script >", "text/html");
                    string script = "alert('密码修改成功！');parent.jQuery.colorbox.close();";
                    return JavaScript(script);
                }
                else
                {
                    //string text = "<script >alert('两次输入的密码不相同，请重新输入！');window.location.href='/Account/ChangePassword/" + ID.ToString() + "'</script >";
                    //return Content(text, "text/html");
                    string script = "alert('两次输入的密码不相同，请重新输入！');";
                    return JavaScript(script);
                }
            }
            else
            {
                //string text = "<script >alert('密码输入有误，请重新输入！');window.location.href='/Account/ChangePassword/" + ID.ToString() + "'</script >";
                //return Content(text, "text/html");
                //return Content("<script >alert('密码输入有误！')", "text/javascript");
                string script = "alert('密码输入有误！');";
                return JavaScript(script);

            }
            //return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}