using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMOGUSIK.Entities
{
    public partial class Roles
    {
        public Roles()
        {
            Customers = new HashSet<Customers>();
            Employees = new HashSet<Employees>();
        }

        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<Customers> Customers { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }

        public static Roles Admin = new Roles { RoleID = 1, RoleName = "Администратор" };
    }

}
