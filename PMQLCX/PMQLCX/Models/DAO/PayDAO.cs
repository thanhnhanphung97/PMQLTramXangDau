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

        public bool InsertPay(PayTable pay)
        {
            int data = 0;
            string query = string.Format("USP_InsertPay @inputDate = {0}, @idReceiver = {1}, @idPayer = {2}, @describe = N'{3}', @money = {4}", pay.InputDate, pay.IdReceiver, pay.IdPayer, pay.Describe, pay.Money);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool UpdatePay(PayTable pay)
        {
            int data = 0;
            string query = string.Format("USP_UpdatePay @id = {0}, @inputDate = {1}, @idReceiver = {2}, @idPayer = {3}, @describe = N'{4}', @money = {5}", pay.Id, pay.InputDate, pay.IdReceiver, pay.IdPayer, pay.Describe, pay.Money);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool DeletePay(int id)
        {
            int data = 0;
            string query = string.Format("USP_DeletePay @id = {0}", id);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }
    }
}
