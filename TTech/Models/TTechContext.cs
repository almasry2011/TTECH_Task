using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTech.Models
{
    public class TTechContext:DbContext
    {
        public TTechContext(DbContextOptions<TTechContext> options) : base(options)
        {  }
        public DbSet<Employee> Employees { get; set; }
    }
}
