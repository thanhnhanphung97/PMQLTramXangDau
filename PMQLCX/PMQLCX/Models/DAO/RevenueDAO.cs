using PMQLCX.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DAO
{
    public class RevenueDAO
    {
        private static RevenueDAO instance;

        public static RevenueDAO Instance
        {
            get
            {
                if (instance == null) instance = new RevenueDAO();
                return instance;
            }

            private set
            {
                RevenueDAO.instance = value;
            }
        }

        public DataTable GetAllRevenue()
        {
            string query = "USP_GetAllRevenue";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }

        /*
        public bool InsertReceipt(DateTime inputDate, string receiver, string payer, string describe, float money)
        {
            int data = 0;
            string query = string.Format("USP_InsertReceipt @inputDate {0}, @receiver N'{1}', @payer  N'{2}', @describe  N'{3}', @money {4}", inputDate, receiver, payer, describe, money);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool UpdateReceipt(int id, DateTime inputDate, string receiver, string payer, string describe, float money)
        {
            int data = 0;
            string query = string.Format("USP_UpdateProduct @id = {0}, @inputDate {1}, @receiver N'{2}', @payer  N'{3}', @describe  N'{4}', @money {5}", id, inputDate, receiver, payer, describe, money);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool DeleteReceipt(int id)
        {
            int data = 0;
            string query = string.Format("USP_DeleteReceipt @id = {0}", id);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }
        */

    }
}
