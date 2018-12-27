using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    public class Employees
    {
        int Id { get; set; }
        string Name { get; set; }
        DateTime Birth { get; set; }
        bool Gender { get; set; }
        string Phone { get; set; }
        string Email { get; set; }
        string Address { get; set; }
        int Salary { get; set; }
    }
}
