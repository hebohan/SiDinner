using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Rule;
using Rule.Entities;
using PagedList;
using System.Collections;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        #region 职员信息列表
        /// <summary>
        /// 职员信息列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Index(string StaffNo, string StaffName, string startBalance, string endBalance, int? page)
        {
            List<staff> staffList = GetInfoManager.GetStaff(StaffNo, StaffName, startBalance, endBalance);

            //ViewData["staffList"] = staffList;
            //ViewData.Model = staffList;

            //第几页
            int pageNumber = page ?? 1;

            //显示共多少条记录
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);

            //int totality = staffList.Count();

            //根据ID排序
            //staffList = staffList.ToList();
            staffList = staffList.OrderByDescending(x => x.ID).ToList();

            //通过ToPagedList扩展方法进行分页
            IPagedList<staff> pagedList = staffList.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View
            return View(pagedList);
        }
        #endregion

        #region 新建职员信息
        /// <summary>
        /// 新建职员信息
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "admins")]
        //public ActionResult Create(string StaffNo, string StaffName, string StaffPwd,string StaffPower, string Remark)
        //{
        //    FillDataManager.FillStaff(StaffNo, StaffName, StaffPwd, StaffPower, Remark);
        //    return RedirectToAction("Index");
        //}

        public ActionResult CreateAjax(string StaffNo, string StaffName, string StaffPwd, string StaffPower, string Remark)
        {
            FillDataManager.FillStaff(StaffNo, StaffName, StaffPwd, StaffPower, Remark);

            return Json(JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 删除职员信息
        /// <summary>
        /// 删除职员信息
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Delete(int ID)
        {
            UpdateManager.DeleteStaff(ID);
            return RedirectToAction("Index");
        }
        #endregion

        #region 职员信息详情
        /// <summary>
        /// 职员信息详情
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Details(int ID)
        {
            List<staff> staffList = GetInfoManager.GetStaffInfo(ID);
            ViewData["staffList"] = staffList;
            ViewData.Model = staffList;
            return View();
        }

        #endregion

        #region 编辑职员信息
        /// <summary>
        /// 编辑职员信息
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Edit(int ID)
        {
            List<staff> staffList = GetInfoManager.GetStaffInfo(ID);
            ViewData["staffList"] = staffList;
            int staffpower = GetInfoManager.GetStaffPower(ID);
            SelectListItem[] itemList = new SelectListItem[]
            {
                new SelectListItem() { Value = "1" ,Text = "管理员"},
                new SelectListItem() { Value = "2", Text = "普通职员"}
            };
            ViewData["powerList"] = new SelectList(itemList as IEnumerable, "Value", "Text", staffpower);
            ViewData.Model = staffList;
            return View();
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admins")]
        public ActionResult EditConfirmed(int ID, string StaffNo, string StaffName, int StaffPower, string Remark)
        {
            UpdateManager.UpdateStaff(ID, StaffNo, StaffName, StaffPower, Remark);
            return RedirectToAction("Index");
        }
        #endregion

        [Authorize(Roles = "admins,staffs")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "admins,staffs")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}