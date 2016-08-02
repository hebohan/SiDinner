using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Entities
{
    public class staffGroup
    {
        public int? ID { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
        public string StaffGroup { get; set; }
        public string Remark { get; set; }
        public string CreateStaffNo { get; set; }
        public DateTime CreateDatetime { get; set; }
    }
}
