using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rule.Entities
{
    public class recharge
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "工号")]
        public string StaffNo { get; set; }

        [Display(Name = "姓名")]
        public string StaffName { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "充值金额")]
        public Nullable<decimal> RechargeMoney { get; set; }

        //[Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "充值时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> RechargeDatetime { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        //[Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "创建人工号")]
        public string CreateStaffNo { get; set; }

        public Nullable<System.DateTime> CreateDatetime { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<int> State { get; set; }
    }
}
