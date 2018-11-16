using PMQLCX.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DAO
{
    public class PayDAO
    {
        private static PayDAO instance;

        public static PayDAO Instance
        {
            get
            {
                if (instance == null) instance = new PayDAO();
                return instance;
            }

            private set
            {
                PayDAO.instance = value;
            }
        }

        public DataTable GetAllPay()
        {
            string query = "USP_GetAllPay";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }

        public bool InsertPay(DateTime inputDate, string receiver, string payer, string describe, float money)
        {
            int data = 0;
            string query = string.Format("USP_InsertPay @inputDate {0}, @receiver N'{1}', @payer  N'{2}', @describe  N'{3}', @money {4}", inputDate, receiver, payer, describe, money);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool UpdateReceipt(int id, DateTime inputDate, string receiver, string payer, string describe, float money)
        {
            int data = 0;
            string query = string.Format("USP_UpdatePay @id = {0}, @inputDate {1}, @receiver N'{2}', @payer  N'{3}', @describe  N'{4}', @money {5}", id, inputDate, receiver, payer, describe, money);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool DeleteReceipt(int id)
        {
            int data = 0;
            string query = string.Format("USP_DeletePay @id = {0}", id);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }
    }
}
