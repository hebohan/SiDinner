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
    public class DinnerbookController : Controller
    {
        #region 订餐列表
        /// <summary>
        /// 订餐列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Index(string ID, string BookStaffNo, string startBookDatetime, string endBookDatetime, int? page)
        {
            List<dinnerbook> dinnerbookList = GetInfoManager.GetDinnerbook(ID, BookStaffNo, startBookDatetime,endBookDatetime);

            //ViewData["dinnerbookList"] = dinnerbookList;
            //ViewData.Model = dinnerbookList;

            //第几页
            int pageNumber = page ?? 1;

            //显示共多少条记录
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);

            //int totality = dinnerbookList.Count();

            //根据ID排序
            //staffList = staffList.ToList();
            dinnerbookList = dinnerbookList.OrderByDescending(x => x.ID).ToList();

            //通过ToPagedList扩展方法进行分页
            IPagedList<dinnerbook> pagedList = dinnerbookList.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View
            return View(pagedList);
        }
        #endregion

        #region 新建订餐记录
        /// <summary>
        /// 新建订餐记录
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Create()
        {
            List<group> groupList = GetInfoManager.GetGroup();
            ViewData["staffgroup"] = groupList;

            return View();
        }

        public ActionResult GetForSelect(int ID)
        {
            List<staffGroup> staffList = GetInfoManager.GetStaffGroup(ID);

            return Json(staffList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admins")]
        public ActionResult Create(decimal TotalPrices, string[] ArrStaffNo, decimal ConsumeMoney, DateTime BookDatetime, string Remark, int Num = 0)
        {
            FillDataManager.FillDinnerbook(TotalPrices, BookDatetime, Remark);
            int BookID = GetInfoManager.GetBookID();
            decimal LastConsumeMoney = TotalPrices - ConsumeMoney * (Num - 1);
            for (int i = 0; i < Num; i++)
            {
                if(i < Num-1)
                {
                    FillDataManager.FillConsume(BookID, ArrStaffNo[i], ConsumeMoney, BookDatetime, Remark);
                }
                else
                    FillDataManager.FillConsume(BookID, ArrStaffNo[i], LastConsumeMoney, BookDatetime, Remark);
            }
            //return RedirectToAction("Index");
            string script = "top.location.reload(); parent.jQuery.colorbox.close();";
            return JavaScript(script);
        }
        #endregion

        #region 订餐记录明细
        /// <summary>
        /// 订餐记录明细
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Details(string ID, int? page)
        {
            List<consume> consumeList = GetInfoManager.GetDinnerbookDetails(ID);

            //第几页
            int pageNumber = page ?? 1;

            //显示共多少条记录
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);

            //int totality = consumeList.Count();

            //根据ID排序
            //staffList = staffList.ToList();
            consumeList = consumeList.OrderByDescending(x => x.ID).ToList();

            //通过ToPagedList扩展方法进行分页
            IPagedList<consume> pagedList = consumeList.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View
            return View(pagedList);
        }
        #endregion
    }
}