using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    public class PayTable
    {
        public int Id { get; set; }
        public DateTime InputDate { get; set; }
        public int IdReceiver { get; set; }
        public int IdPayer { get; set; }
        public string Describe { get; set; }
        public float Money { get; set; }
    }
}
