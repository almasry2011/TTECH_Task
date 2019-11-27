using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTech.Models;
using TTech.Paging;

namespace TTech.ViewModels
{
    public class EmployeeViewModel
    {
        public IEnumerable< Employee >employees { get; set; }
        public Paginghelper paging { get; set; }
        public string OrderVal { get; set; }
        public string SearchVal { get; set; }
        //public string URL_Query { get; set; }
    }
}
