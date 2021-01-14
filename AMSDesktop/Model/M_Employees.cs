
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMSDesktop.Model
{
    public class M_Employees
    {
    
        public long ID { get; set; }
        public string REFID { get; set; }
        public string ADID { get; set; }
        public string EmpNo { get; set; }
        public string Family_Name_Suffix { get; set; }
        public string Family_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Date_Hired { get; set; }
        public string Date_Resigned { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }
        public string RFID { get; set; }
        public string EmployeePhoto { get; set; }
        public string ModifiedStatus { get; set; }
        public string ModifiedPosition { get; set; }
        public string CostCenter_AMS { get; set; }
        public string Schedule { get; set; }
    }
}
