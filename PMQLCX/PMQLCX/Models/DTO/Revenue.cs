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
        float OutOfGas92 { get; set; }
        int PriceOfGas92 { get; set; }

        float OutOfGas95 { get; set; }
        int PriceOfGas95 { get; set; }

        float OutOfOil { get; set; }
        int PriceOfOil { get; set; }
        float Income { get; set; }
        float Spend { get; set; }
        float Inventory { get; set; }
    }
}
