using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Entities
{
    public class changePwd
    {
        public int ID { get; set; }

        [Display(Name = "旧密码")]
        [Required(ErrorMessage = "必须输入{0}")]
        [DataType(DataType.Password)]
        public string OldPwd { get; set; }

        [Display(Name = "新密码")]
        [Required(ErrorMessage = "必须输入{0}")]
        [DataType(DataType.Password)]
        public string NewPwd { get; set; }

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "必须输入{0}")]
        [DataType(DataType.Password)]
        public string ConfirmPwd { get; set; }
    }
}
