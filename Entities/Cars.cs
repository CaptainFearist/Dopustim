using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMOGUSIK.Entities
{
    public class Cars
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarID { get; set; }

        public int CustomerID { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string VIN { get; set; }

        public virtual Customers Customers { get; set; }
        public virtual ICollection<ServiceOrders> ServiceOrders { get; set; }
    }
}