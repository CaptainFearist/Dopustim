using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMOGUSIK.Entities
{
    public class ServiceOrders
    {
        public int OrderID { get; set; }
        public int CarID { get; set; }
        public int ServiceTypeID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }



}
