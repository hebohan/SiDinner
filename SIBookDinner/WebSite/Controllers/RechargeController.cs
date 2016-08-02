using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using PagedList;
using Rule;
using Rule.Entities;

namespace WebSite.Controllers
{
    public class RechargeController : Controller
    {
        #region 充值记录列表
        /// <summary>
        /// 充值记录列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Index(string StaffNo, string startRechargeMoney, string endRechargeMoney, string startRechargeDatetime, string endRechargeDatetime, int? page)
        {
            List<recharge> rechargeList = GetInfoManager.GetRecharge(StaffNo, startRechargeMoney, endRechargeMoney, startRechargeDatetime, endRechargeDatetime);

            //ViewData["rechargeList"] = rechargeList;
            //ViewData.Model = rechargeList;

            //第几页
            int pageNumber = page ?? 1;

            //显示共多少条记录
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);

            //int totality = rechargeList.Count();

            //根据ID排序
            //staffList = staffList.ToList();
            rechargeList = rechargeList.OrderByDescending(x => x.ID).ToList();

            //通过ToPagedList扩展方法进行分页
            IPagedList<recharge> pagedList = rechargeList.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View
            return View(pagedList);
        }
        #endregion

        #region 新建充值记录
        /// <summary>
        /// 新建充值记录
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Create(int ID)
        {
            List<recharge> rechargeList = GetInfoManager.GetStaffNo(ID);
            ViewData["rechargeList"] = rechargeList;
            ViewData.Model = rechargeList;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admins")]
        public ActionResult Create(string StaffNo, string RechargeMoney, string Remark)
        {
            FillDataManager.FillRecharge(StaffNo, RechargeMoney, Remark);

            //return RedirectToAction("Index");
            string script = "parent.jQuery.colorbox.close();window.location.href='../Home/Index'";
            return JavaScript(script);
        }
        #endregion
    }
}