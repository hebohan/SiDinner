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
    public class ConsumeController : Controller
    {
        #region 消费列表
        /// <summary>
        /// 消费列表
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public ActionResult Index(string StaffNo, string startConsumeoney, string endConsumeMoney, string startConsumeDatetime, string endConsumeDatetime, int? page)
        {
            List<consume> consumeList = GetInfoManager.GetConsume(StaffNo, startConsumeoney, endConsumeMoney, startConsumeDatetime, endConsumeDatetime);

            //ViewData["consumeList"] = consumeList;
            //ViewData.Model = consumeList;

            //第几页
            int pageNumber = page ?? 1;

            //显示共多少条记录
            int pageSize = int.Parse(ConfigurationManager.AppSettings["pageSize"]);

            //int TotalItemCount = consumeList.Count();

            //根据ID排序
            consumeList = consumeList.OrderByDescending(x => x.ID).ToList();

            //通过ToPagedList扩展方法进行分页
            IPagedList<consume> pagedList = consumeList.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View
            return View(pagedList);
        }
        #endregion
    }
}