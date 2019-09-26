using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessProjectTracking.Models
{

  
    public class oMenuAccess
    {
        public int MenuId { get; set; }
        public string mumenuname { get; set; }
        public bool roAccessgrant { get; set; }
    }

    public class Authorize
    {
        public List<oMenuAccess> list { get; set; }
    }
}