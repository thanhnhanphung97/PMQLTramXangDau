using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    class ImportProduct
    {
        int Id { get; set; }
        DateTime DateTime { get; set; }
        string Partners { get; set; }
        int Product { get; set; }
        float Amount { get; set; }
        float UnitPrice { get; set; }
        float IntoMoney { get; set; }
    }
}
