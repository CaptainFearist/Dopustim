using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMOGUSIK.Entities
{
    public class ServiceOrders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }
        public int CarID { get; set; }
        public Cars Car { get; set; }
        public int ServiceTypeID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }
}