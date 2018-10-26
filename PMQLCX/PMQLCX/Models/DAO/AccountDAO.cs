using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMQLCX.Models.DTO;
using PMQLCX.Models.DAO;
using System.Data;

namespace PMQLCX.Models.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO();
                return AccountDAO.instance;
            }

            private set
            {
                AccountDAO.instance = value;
            }
        }

        public bool LogIn(string userName,string password)
        {
            string query = "EXEC USP_LogIn @UserName, @Password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, password });

            return result.Columns.Count > 0;
        }

    }
}
