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

        private void FrmHome_Load(object sender, EventArgs e)
        {
            /*SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=.;Initial Catalog=DataTramXangDau;Integrated Security=True";
            con.Open();*/
        }

        private void tileItem1_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (tabPane.SelectedPage != tabLogin)
                tabPane.SelectedPage = tabLogin;
            else tabPane.SelectedPage = tabHome;
        }

        private void tileItem2_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {

        }

        private void enableAcceptLogin()
        {
            if (txtPassword.Text != "" && txtUsername.Text != "") btnLogin.Enabled = true;
            else btnLogin.Enabled = false;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            enableAcceptLogin();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            enableAcceptLogin();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void tileViewProduct_ItemCustomize(object sender, DevExpress.XtraGrid.Views.Tile.TileViewItemCustomizeEventArgs e)
        {
            e.Item.Elements[0].Text = "a";
            e.Item.Elements[1].Text = "b";
            e.Item.Elements[2].Text = "c";
            e.Item.Elements[3].Text = "d";
        }

        private void tileItem3_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (tabPane.SelectedPage != tabProductList)
                tabPane.SelectedPage = tabProductList;
            else tabPane.SelectedPage = tabHome;
        }
    }
}
