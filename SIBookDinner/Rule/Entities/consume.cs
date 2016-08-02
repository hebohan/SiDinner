using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Entities
{
    public class consume
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
        public decimal ConsumeMoney { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public System.DateTime ConsumeDatetime { get; set; }
        public string Remark { get; set; }
        public string CreateStaffNo { get; set; }
        public Nullable<System.DateTime> CreateDatetime { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<int> State { get; set; }
    }
}
