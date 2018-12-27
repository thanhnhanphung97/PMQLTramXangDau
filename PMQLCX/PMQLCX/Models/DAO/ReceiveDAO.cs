using PMQLCX.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DAO
{
    public class ReceiveDAO
    {
        private static ReceiveDAO instance;

        public static ReceiveDAO Instance
        {
            get
            {
                if (instance == null) instance = new ReceiveDAO();
                return instance;
            }

            private set
            {
                ReceiveDAO.instance = value;
            }
        }

        public DataTable GetAllReceipt()
        {
            string query = "USP_GetAllReceipt";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }

        public bool InsertReceipt(ReceiveTable receive)
        {
            int data = 0;
            string query = string.Format("USP_InsertReceipt @inputDate = N'{0}', @idReceiver = {1}, @idPayer = {2}, @idProduct = {3}, @exportProduct = {4}, @priceInDay = {5}, @describe =  N'{6}', @money = {7}", receive.InputDate, receive.IdReceiver, receive.IdPayer, receive.IdProduct, receive.ExportProduct, receive.PriceInDay, receive.Describe, receive.Money);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool UpdateReceipt(ReceiveTable receive)
        {
            int data = 0;
            string query = string.Format("USP_UpdateReceipt @id = {0}, @inputDate = N'{1}', @idReceiver = {2}, @idPayer = {3}, @idProduct = {4}, @exportProduct = {5}, @priceInDay = {6}, @describe =  N'{7}', @money = {8}", receive.Id, receive.InputDate, receive.IdReceiver, receive.IdPayer, receive.IdProduct, receive.ExportProduct, receive.PriceInDay, receive.Describe, receive.Money);
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

        public ReceiveTable GetReceiptById(int id)
        {
            ReceiveTable receive = new ReceiveTable();
            DataTable data = DataProvider.Instance.ExecuteQuery(string.Format("USP_GetReceiptById {0}", id));
            receive.Id = Int32.Parse(data.Rows[0][0].ToString());
            receive.InputDate = DateTime.Parse(data.Rows[0][1].ToString());
            receive.IdReceiver = Int32.Parse(data.Rows[0][2].ToString());
            receive.IdPayer = Int32.Parse(data.Rows[0][3].ToString());
            receive.IdProduct = Int32.Parse(data.Rows[0][4].ToString());
            receive.ExportProduct = Int32.Parse(data.Rows[0][5].ToString());
            receive.PriceInDay = Int32.Parse(data.Rows[0][6].ToString());
            receive.Describe = data.Rows[0][7].ToString();
            receive.Money = Int32.Parse(data.Rows[0][8].ToString());
            return receive;
        }

        public List<string> GetListReceiver()
        {
            List<string> receiver = new List<string>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetListReceiver");
            foreach (DataRow item in data.Rows)
            {
                receiver.Add(item[0].ToString());
            }
            return receiver;
        }

        public List<string> GetListPayer()
        {
            List<string> payer = new List<string>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetListPayer");
            foreach (DataRow item in data.Rows)
            {
                payer.Add(item[0].ToString());
            }
            return payer;
        }

        public List<string> GetListNameProducts()
        {
            List<string> products = new List<string>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetListNameProducts");
            foreach (DataRow item in data.Rows)
            {
                products.Add(item[0].ToString());
            }
            return products;
        }

        public int GetIdReceiverByName(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(string.Format("USP_GetIdReceiverByName {0}", name));
            return Int32.Parse(data.Rows[0][0].ToString());
        }
        public int GetIdPayerByName(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(string.Format("USP_GetIdPayerByName {0}", name));
            return Int32.Parse(data.Rows[0][0].ToString());
        }
        public int GetIdProductByName(string name)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(string.Format("USP_GetIdProductByName {0}", name));
            return Int32.Parse(data.Rows[0][0].ToString());
        }
    }
}