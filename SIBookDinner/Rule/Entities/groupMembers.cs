using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.Entities
{
    public class groupMembers
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public string StaffNo { get; set; }
        public string Remark { get; set; }
        public string CreateStaffNo { get; set; }
        public Nullable<System.DateTime> CreateDatetime { get; set; }
        public Nullable<System.DateTime> UpdateDatetime { get; set; }
        public Nullable<int> State { get; set; }
    }
}
