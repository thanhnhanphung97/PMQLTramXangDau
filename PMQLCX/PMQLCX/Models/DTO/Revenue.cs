using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    public class Revenue
    {
        int Id { get; set; }
        DateTime Date { get; set;}
        float Income { get; set; }
        float Spend { get; set; }
        float Inventory { get; set; }
    }
}
