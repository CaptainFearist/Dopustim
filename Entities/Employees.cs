using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMOGUSIK.Entities
{
    public partial class Employees
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }

        public virtual Roles Role { get; set; }
    }


}
