using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Entities
{
    public class dinnerbook
    {
        public int ID { get; set; }
        public Nullable<decimal> TotalPrices { get; set; }
        public string BookStaffNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public Nullable<System.DateTime> BookDatetime { get; set; }
        public string Remark { get; set; }
        public string CreateStaffNo { get; set; }
        public Nullable<System.DateTime> CreateDatetime { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<int> State { get; set; }
    }
}
