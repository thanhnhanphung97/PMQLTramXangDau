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

        public static DataTable GetAccount(string username, string password)
        {
            string query = "EXEC USP_LogIn @Username , @Password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { username, password });
            return result;
        }

        public static bool ChangePassword(string username, string newPassword)
        {
            string query = "EXEC USP_ChangePassword @Username , @NewPassword";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { username, newPassword });
            return result>0;
        }

    }
}
