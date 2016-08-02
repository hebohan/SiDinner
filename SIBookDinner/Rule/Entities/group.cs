using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Entities
{
    public class group
    {
        public int ID { get; set; }

        [Display(Name = "分组名称")]
        public string GroupName { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }
        public string CreateStaffNo { get; set; }
        public Nullable<System.DateTime> CreateDatetime { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<int> State { get; set; }
    }
}
