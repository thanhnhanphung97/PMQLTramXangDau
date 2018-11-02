using PMQLCX.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance;

        public static ProductDAO Instance
        {
            get
            {
                if (instance == null) instance = new ProductDAO();
                return instance;
            }

            private set
            {
                ProductDAO.instance = value;
            }
        }

        public DataTable GetAllProduct()
        {
            string query = "USP_GetAllProduct";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }

        public bool InsertProduct(string Name)
        {
            int data = 0;
            string query = string.Format("USP_InsertProduct @name = N'{0}'", Name);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool UpdateProduct(int id,string name,float amount)
        {
            int data = 0;
            string query = string.Format("USP_UpdateProduct @id = {0}, @name = N'{1}', @amount = N{2}",id, name, amount);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }

        public bool DeleteProduct(int id)
        {
            int data = 0;
            string query = string.Format("USP_DeleteProduct @id = {0}", id);
            data = DataProvider.Instance.ExecuteNonQuery(query);
            return data > 0;
        }
    }
}
