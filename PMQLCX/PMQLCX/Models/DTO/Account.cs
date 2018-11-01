using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMQLCX.Models.DTO
{
    public class Account
    {
        string Username { get; set; }
        string Name { get; set; }
        string Password { get; set; }

        public Account(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public Account(DataRow row)
        {
            this.Name = row["Name"].ToString();
            this.Username = row["Username"].ToString();
            this.Password = row["Password"].ToString();
        }
    }
}
