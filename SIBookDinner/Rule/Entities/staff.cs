using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rule.Entities
{
    public class staff
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "工号")]
        public string StaffNo { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "姓名")]
        public string StaffName { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        public string StaffPwd { get; set; }

        [Required(ErrorMessage = "必须输入{0}")]
        [Display(Name = "权限")]
        public int StaffPower { get; set; }

        [Display(Name = "余额")]
        public Nullable<decimal> Balance { get; set; }


        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "创建人工号")]
        public string CreateStaffNo { get; set; }

        [Display(Name = "创建时间")]
        public Nullable<System.DateTime> CreateDatetime { get; set; }

        [Display(Name = "更新时间")]
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public int State { get; set; }

        [Display(Name = "上一次登录时间")]
        public Nullable<System.DateTime> LoginTime { get; set; }

        [Display(Name = "上一次登陆IP")]
        public string LoginIP { get; set; }
    }
}
