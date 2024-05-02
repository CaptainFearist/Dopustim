using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMOGUSIK.Entities
{
    public partial class Customers
    {
        [Key]
        public int CustomerID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }


        public string Password { get; set; }

        public int? RoleID { get; set; } // Сделано nullable, так как это внешний ключ
        public Roles Role { get; set; }

        public virtual ICollection<Cars> Cars { get; set; }
    }

}
