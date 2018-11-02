using DevExpress.XtraBars.Navigation;
using PMQLCX.Models.DAO;
using PMQLCX.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQLCX
{
    public partial class FrmHome : Form
    {
        public FrmHome()
        {
            InitializeComponent();
        }

        bool status = false;
        DataTable account;

        #region function
        private void SelectedPage(TabNavigationPage tabPage)
        {
            if (tabPane.SelectedPage != tabPage)
                tabPane.SelectedPage = tabPage;
            else tabPane.SelectedPage = tabHome;
        }

        private void EnableLogin()
        {
            if (txtPassword.Text != "" && txtUsername.Text != "") btnLogin.Enabled = true;
            else btnLogin.Enabled = false;
        }

        private void EnableApply()
        {
            if (txtCurPassword.Text == account.Rows[0]["Password"].ToString()) pbCurrentPassword.Visible = true;
            else pbCurrentPassword.Visible = false;
            if (txtNewPassword.Text != txtCurPassword.Text&&txtNewPassword.Text!="") pbNewPassword.Visible = true;
            else pbNewPassword.Visible = false;
            if (txtConfirmPassword.Text == txtNewPassword.Text&& txtConfirmPassword.Text!="") pbConfirmNewPassword.Visible = true;
            else pbConfirmNewPassword.Visible = false;

            if (txtCurPassword.Text == account.Rows[0]["Password"].ToString() && txtNewPassword.Text != txtCurPassword.Text && txtNewPassword.Text != "" && txtConfirmPassword.Text == txtNewPassword.Text)
                btnApply.Enabled = true;
            else btnApply.Enabled = false;
        }
        #endregion

        private void FrmHome_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;
        }

        #region chooseMenu
        private void tileItem1_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if(!status)
            {
                SelectedPage(tabLogin);
            }
            else
            {
                status = false;
                tileItem1.Text = "Log in";
                tileItem1.Image = PMQLCX.Properties.Resources.login;
                tabPane.SelectedPage = tabHome;

                tileItem2.Enabled = false;
                tileItem3.Enabled = false;
            }
        }

        private void tileItem2_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            SelectedPage(tabChangePassword);
        }

        private void tileItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            SelectedPage(tabProductList);
        }
        #endregion

        #region login
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            EnableLogin();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            EnableLogin();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            account = AccountDAO.GetAccount(txtUsername.Text, txtPassword.Text);
            if (account.Rows.Count>0)
            {
                status = true;
                tileItem1.Text = "Welcome "+account.Rows[0]["Name"].ToString()+"\nLog out";
                tileItem1.Image = PMQLCX.Properties.Resources.logout;
                tabPane.SelectedPage = tabHome;
                lbmessageLogin.Visible = false;
                txtUsername.Text = "";
                txtPassword.Text = "";

                tileItem2.Enabled = true;
                tileItem3.Enabled = true;
            }
            else
            {
                lbmessageLogin.Visible = true;
                timer.Enabled = true;
            }
        }
        #endregion

        #region changePassword
        private void txtCurPassword_TextChanged(object sender, EventArgs e)
        {
            EnableApply();
        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            EnableApply();
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            EnableApply();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (AccountDAO.ChangePassword(account.Rows[0]["Username"].ToString(), txtNewPassword.Text))
            {
                account.Rows[0]["Password"] = txtNewPassword.Text;
                lbmessageChangePassword.Visible = true;
                timer.Enabled = true;
                txtCurPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
            }
        }
        #endregion

        private void tileViewProduct_ItemCustomize(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemCustomizeEventArgs e)
        {
            e.Item.Elements[0].Text = "a";
            e.Item.Elements[1].Text = "b";
            e.Item.Elements[2].Text = "c";
            e.Item.Elements[3].Text = "d";
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (lbmessageChangePassword.Visible)
            {
                lbmessageChangePassword.Visible = false;
                timer.Enabled = false;
            }
            if (lbmessageLogin.Visible)
            {
                lbmessageLogin.Visible = false;
                timer.Enabled = false;
            }
        }

        private void tabHome_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
