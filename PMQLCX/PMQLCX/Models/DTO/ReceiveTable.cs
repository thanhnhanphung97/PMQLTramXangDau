using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    public class ReceiveTable
    {
        public int Id { get; set; }
        public DateTime InputDate { get; set; }
        public int IdReceiver { get; set; }
        public int IdPayer { get; set; }
        public int IdProduct { get; set; }
        public float ExportProduct { get; set; }
        public int PriceInDay { get; set; }
        public string Describe { get; set; }
        public float Money { get; set; }
    }
}
