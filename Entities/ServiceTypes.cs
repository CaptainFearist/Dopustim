using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AMOGUSIK.Entities
{
    public class ServiceTypes
    {
        [Key]
        public int ServiceTypeID { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
    }
}
