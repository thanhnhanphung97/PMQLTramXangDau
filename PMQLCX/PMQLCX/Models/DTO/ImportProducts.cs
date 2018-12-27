using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    public class ImportProducts
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int IdPartner { get; set; }
        public int Product { get; set; }
        public float Amount { get; set; }
        public float UnitPrice { get; set; }
        public float IntoMoney { get; set; }
    }
}
