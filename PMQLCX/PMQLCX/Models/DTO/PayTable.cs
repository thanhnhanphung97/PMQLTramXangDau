using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    public class PayTable
    {
        int Id { get; set; }
        DateTime InputDate { get; set; }
        string Receiver { get; set; }
        string Payer { get; set; }
        string Describe { get; set; }
        float Money { get; set; }
    }
}
