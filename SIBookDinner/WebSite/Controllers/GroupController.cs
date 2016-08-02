using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Common;
using PagedList;
using Rule;
using Rule.Entities;

namespace WebSite.Controllers
{
    public class GroupController : Controller
    {
        #region 显示分组成员信息
        /// <summary>
        /// 移除分组成员
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Indexs(int? page)
        {
            List<group> groupList = GetInfoManager.GetGroup();

            ViewData["staffgroup"] = groupList;

            return View();
        }
        public ActionResult GetStaffGroupList(int ID, int? page)
        {
            List<staffGroup> staffGroupList = GetInfoManager.GetStaffGroup(ID);

            //ViewData["staffGroupList"] = staffGroupList;
            //ViewData.Model = staffGroupList;

            //第几页
            int pageNumber = page ?? 1;

            //显示共多少条记录
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);

            //int totality = staffGroupList.Count();

            //根据ID排序
            //staffList = staffList.ToList();
            staffGroupList = staffGroupList.OrderByDescending(x => x.StaffNo).ToList();

            //通过ToPagedList扩展方法进行分页
            IPagedList<staffGroup> pagedList = staffGroupList.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View
            return Json(pagedList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 移除分组成员
        /// <summary>
        /// 移除分组成员
        /// </summary>
        /// <returns></returns>
        //[Authorize(Roles = "admins")]
        public ActionResult Delete(int ID)
        {
            UpdateManager.DeleteGroupStaff(ID);
            //return RedirectToAction("Indexs");
            return Json(JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新建分组记录
        /// <summary>
        /// 新建分组记录
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admins")]
        public ActionResult Create(string GroupName, string Remark)
        {
            FillDataManager.FillGroup(GroupName, Remark);
            //return RedirectToAction("Indexs");
            string script = "top.location.reload();parent.jQuery.colorbox.close();";
            return JavaScript(script);
        }
        #endregion

        #region 添加分组成员
        /// <summary>
        /// 添加分组成员
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Add(int ID)
        {
            List<groupMembers> groupMembersList = GetInfoManager.GetGroupInfo(ID);
            ViewData["groupMembersList"] = groupMembersList;
            ViewData.Model = groupMembersList;

            List<staff> staffList = GetInfoManager.GetStaff(ID);
            ViewData["staffList"] = staffList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admins")]
        public ActionResult Add(int GroupID, string StaffNo, string Remark)
        {
            FillDataManager.FillGroupMembers(GroupID, StaffNo, Remark);
            return RedirectToAction("Add/"+GroupID);
            //return View();
        }
        #endregion

        #region 移除分组
        /// <summary>
        /// 移除分组
        /// </summary>
        /// <returns></returns>
       
        [Authorize(Roles = "admins")]
        public ActionResult DeleteGroup(int ID)
        {
            UpdateManager.DeleteGroup(ID);
            return RedirectToAction("Indexs");
        }
        #endregion

        #region 编辑分组信息
        /// <summary>
        /// 编辑分组信息
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "admins")]
        public ActionResult Edit(int ID)
        {
            List<group> groupMembersList = GetInfoManager.GetGroupInfoEdit(ID);
            ViewData["groupMembersList"] = groupMembersList;
            ViewData.Model = groupMembersList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admins")]
        public ActionResult Edit(int ID, string GroupName, string Remark)
        {
            UpdateManager.UpdateGroup(ID, GroupName, Remark);
            //return RedirectToAction("Indexs");
            string script = "top.location.reload();parent.jQuery.colorbox.close();";
            return JavaScript(script);
        }
        #endregion
    }
}