﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMOGUSIK.Entities
{
    public partial class Roles
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public ICollection<Customers> Customers { get; set; }
    }

}

