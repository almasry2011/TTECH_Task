using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTech.Paging
{
    public class Paginghelper
    {
            public int TotalEmps { get; set; }
            public int EmpsPerPages { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPages { get {   return (int)Math.Ceiling((decimal)TotalEmps / EmpsPerPages);  } }
    }
}
