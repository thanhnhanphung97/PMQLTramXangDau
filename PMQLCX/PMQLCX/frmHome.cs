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
using DevExpress.XtraGrid.Views;
using DevExpress.XtraEditors;

namespace PMQLCX
{
    public partial class FrmHome : Form
    {
        public FrmHome()
        {
            InitializeComponent();
            Load();
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
            if (txtCurPassword.Text == account.Rows[0]["Password"].ToString() && txtNewPassword.Text != txtCurPassword.Text && txtNewPassword.Text != "" && txtConfirmPassword.Text == txtNewPassword.Text) btnApply.Enabled = true;
            else btnApply.Enabled = false;
        }
        #endregion

        private void FrmHome_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;

            btnLogin.Enabled = true;

            Load();

            CrystalReport rpt = new CrystalReport();
            rpt.SetDataSource(ReceiveDAO.Instance.GetAllReceipt());
            crystalReportViewer.ReportSource=rpt;
        }

        #region time
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
        #endregion

        #region chooseMenu

        private void tileItemLogin_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if (!status)
            {
                SelectedPage(tabLogin);
            }
            else
            {
                status = false;
                tileItemLogin.Text = "Log in";
                tileItemLogin.Image = PMQLCX.Properties.Resources.login;
                tabPane.SelectedPage = tabHome;

                tileItemChangePassword.Enabled = false;
                tileItemProductList.Enabled = false;
            }
        }

        private void tileItemChangePassword_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            SelectedPage(tabChangePassword);
        }

        private void tileItemProductList_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            SelectedPage(tabProductList);
        }

        private void tileReceipts_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            SelectedPage(tabReceipts);
            //gvReceipts.GridControl.DataSource = ReceiveDAO.Instance.GetAllReceipt();
        }


        private void tilePay_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            SelectedPage(tabPay);
        }
        
        private void tileRevenue_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            SelectedPage(tabRevenue);
            gvRevenue.GridControl.DataSource = RevenueDAO.Instance.GetAllRevenue();
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
                tileItemLogin.Text = "Welcome "+account.Rows[0]["Name"].ToString()+"\nLog Out";
                tileItemLogin.Image = PMQLCX.Properties.Resources.logout;
                tabPane.SelectedPage = tabHome;
                lbmessageLogin.Visible = false;
                txtUsername.Text = "";
                txtPassword.Text = "";

                tileItemChangePassword.Enabled = true;
                tileItemProductList.Enabled = true;
                tileReceipts.Enabled = true;
                tilePay.Enabled = true;
                tileRevenue.Enabled = true;
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

        #region productList
        private void tileItemInsertProduct_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            ProductDAO.Instance.InsertProduct(txtNameProduct.Text);
            LoadProducts();
        }

        private void tileItemUpdateProduct_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Products product = new Products();
            product.Id = Int32.Parse(txtIdProduct.Text);
            product.Name = txtNameProduct.Text;
            product.Amount = float.Parse(txtAmountProduct.Text);
            if (ProductDAO.Instance.UpdateProduct(product) == true) MessageBox.Show("Edit success!");
            LoadProducts();
        }

        private void tileItemDeleteProduct_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            if(ProductDAO.Instance.DeleteProduct(int.Parse(txtIdProduct.Text)) == true)
            {
                MessageBox.Show("Delete success!");
                LoadProducts();
            }
        }
        #endregion
        
        private void tabHome_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            tabPane.SelectedPage = tabReport;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            gvRevenue.GridControl.DataSource = RevenueDAO.Instance.GetRevenueByDate(txtDateFrom.DateTime, txtDateTo.DateTime);
        }

        private void Load()
        {
            LoadProducts();
            LoadReceive();
            LoadPay();
        }
        private void LoadProducts()
        {
            this.productsTableAdapter.Fill(this.dataTramXangDauDataSet.Products);
        }
        private void LoadReceive()
        {
            SetFormatDateEdit(txtInputDateReceive);
            this.uSP_GetAllReceiptTableAdapter.Fill(this.dataTramXangDauDataSet.USP_GetAllReceipt);
            List<string> receivers = ReceiveDAO.Instance.GetListReceiver();
            List<string> payers = ReceiveDAO.Instance.GetListPayer();
            List<string> products = ReceiveDAO.Instance.GetListNameProducts();
            cbbReceiverReceive.Properties.Items.Clear();
            cbbPayerReceive.Properties.Items.Clear();
            cbbProduct.Properties.Items.Clear();
            foreach (var item in receivers)
            {
                cbbReceiverReceive.Properties.Items.Add(item.ToString());
            }
            foreach (var item in payers)
            {
                cbbPayerReceive.Properties.Items.Add(item.ToString());
            }
            foreach (var item in products)
            {
                cbbProduct.Properties.Items.Add(item.ToString());
            }
            cbbReceiverReceive.SelectedIndex = 0;
            cbbPayerReceive.SelectedIndex = 0;
            cbbProduct.SelectedIndex = 0;
        }

        private void gvProducts_Click(object sender, EventArgs e)
        {
            foreach (int i in gvProducts.GetSelectedRows())
            {
                DataRow row = gvProducts.GetDataRow(i);
                Products product = ProductDAO.Instance.GetProductById((int)row[0]);
                txtIdProduct.Text = product.Id.ToString();
                txtNameProduct.Text = product.Name;
                txtAmountProduct.Text = product.Amount.ToString();
            }
        }

        private void gvReceive_Click(object sender, EventArgs e)
        {
            foreach (int i in gvReceive.GetSelectedRows())
            {
                DataRow row = gvReceive.GetDataRow(i);
                txtIdReceive.Text = row[0].ToString();
                txtInputDateReceive.DateTime = DateTime.Parse(row[1].ToString());
                cbbReceiverReceive.EditValue = row[2].ToString();
                cbbPayerReceive.EditValue = row[3].ToString();
                cbbProduct.EditValue = row[4].ToString();
                txtExportReceive.Text = row[5].ToString();
                txtPriceInDayReceive.Text = row[6].ToString();
                txtDescribeReceive.Text = row[7].ToString();
                //txtMoneyReceive.Text = row[8].ToString();
            }
        }

        private void btnRefreshReceipt_Click(object sender, EventArgs e)
        {
            txtIdReceive.Text = "";
            txtDescribeReceive.Text = "";
            txtInputDateReceive.DateTime = DateTime.Today;
            txtExportReceive.Text = "0";
            txtPriceInDayReceive.Text = "0";
            txtMoneyReceive.Text = "0";
            cbbPayerReceive.SelectedIndex = 0;
            cbbReceiverReceive.SelectedIndex = 0;
            cbbProduct.SelectedIndex = 0;
        }

        private void SetFormatDateEdit(DateEdit txtdate)
        {
            txtdate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
            txtdate.Properties.Mask.EditMask = "MM/dd/yyyy";
            txtdate.Properties.Mask.UseMaskAsDisplayFormat = true;
        }
        
        private void txtExportReceive_EditValueChanged(object sender, EventArgs e)
        {
            MoneyReceiveUpdate();
        }

        private void txtPriceInDayReceive_EditValueChanged(object sender, EventArgs e)
        {
            MoneyReceiveUpdate();
        }

        private void MoneyReceiveUpdate()
        {
            try
            {
                float sum = float.Parse(txtExportReceive.Text) * Int32.Parse(txtPriceInDayReceive.Text);
                txtMoneyReceive.Text = sum.ToString();
            }
            catch (Exception)
            {

            }
        }

        private void btnInsertReceipt_Click(object sender, EventArgs e)
        {
            if(isDigit(txtPriceInDayReceive.Text) == true)
            {
                if (isDigit(txtExportReceive.Text) == true)
                {
                    if (txtDescribeReceive.Text.Trim() != "")
                    {
                        ReceiveTable receive = new ReceiveTable();
                        //receive.Id = Int32.Parse(txtIdReceive.Text);
                        receive.InputDate = DateTime.Parse(txtInputDateReceive.Text);
                        receive.Describe = txtDescribeReceive.Text;
                        receive.IdReceiver = ReceiveDAO.Instance.GetIdReceiverByName(cbbReceiverReceive.Text);
                        receive.IdPayer = ReceiveDAO.Instance.GetIdPayerByName(cbbPayerReceive.Text);
                        receive.IdProduct = ReceiveDAO.Instance.GetIdProductByName(cbbProduct.Text);
                        receive.ExportProduct = float.Parse(txtExportReceive.Text);
                        receive.PriceInDay = Int32.Parse(txtPriceInDayReceive.Text);
                        receive.Money = float.Parse(txtMoneyReceive.Text);
                        if (ReceiveDAO.Instance.InsertReceipt(receive) == true) { MessageBox.Show("Insert success!"); LoadReceive(); }
                        else MessageBox.Show("Insert fail!");
                    }
                    else MessageBox.Show("Lí Do Thu Tiền Không Được Để Trống!");
                }
                else MessageBox.Show("Số Lượng Xuất (Lít) Phải Là Số!");
            }
            else MessageBox.Show("Giá Trong Ngày Phải Là Số!");
        }

        private void btnUpdateReceipt_Click(object sender, EventArgs e)
        {
            if (isDigit(txtPriceInDayReceive.Text) == true)
            {
                if (isDigit(txtExportReceive.Text) == true)
                {
                    if (txtDescribeReceive.Text.Trim() != "")
                    {
                        ReceiveTable receive = new ReceiveTable();
                        receive.Id = Int32.Parse(txtIdReceive.Text);
                        receive.InputDate = DateTime.Parse(txtInputDateReceive.Text);
                        receive.Describe = txtDescribeReceive.Text;
                        receive.IdReceiver = ReceiveDAO.Instance.GetIdReceiverByName(cbbReceiverReceive.Text);
                        receive.IdPayer = ReceiveDAO.Instance.GetIdPayerByName(cbbPayerReceive.Text);
                        receive.IdProduct = ReceiveDAO.Instance.GetIdProductByName(cbbProduct.Text);
                        receive.ExportProduct = float.Parse(txtExportReceive.Text);
                        receive.PriceInDay = Int32.Parse(txtPriceInDayReceive.Text);
                        receive.Money = float.Parse(txtMoneyReceive.Text);
                        if (ReceiveDAO.Instance.UpdateReceipt(receive) == true) { MessageBox.Show("Update success!"); LoadReceive(); }
                        else MessageBox.Show("Update fail!");
                    }
                    else MessageBox.Show("Lí Do Thu Tiền Không Được Để Trống!");
                }
                else MessageBox.Show("Số Lượng Xuất (Lít) Phải Là Số!");
            }
            else MessageBox.Show("Giá Trong Ngày Phải Là Số!");
        }

        private void btnDeleteReceipt_Click(object sender, EventArgs e)
        {
            if (ReceiveDAO.Instance.DeleteReceipt(Int32.Parse(txtIdReceive.Text)) == true) { MessageBox.Show("Delete success!"); LoadReceive(); }
            else MessageBox.Show("Delete fail!");
        }

        private bool isDigit(string test)
        {
            for(int i=0;i<test.Length;i++)
            {
                if ((int)test[i] < 48 || (int)test[i] > 57) return false;
            }
            return true;
        }

        private void LoadPay()
        {
            SetFormatDateEdit(txtInputDatePay);
            this.uSP_GetAllPayTableAdapter.Fill(this.dataTramXangDauDataSet.USP_GetAllPay);
            List<string> payer = ReceiveDAO.Instance.GetListReceiver();
            List<string> reiceive = ReceiveDAO.Instance.GetListPayer();
            cbbReceiverPay.Properties.Items.Clear();
            cbbPayerPay.Properties.Items.Clear();
            foreach (var item in reiceive)
            {
                cbbReceiverPay.Properties.Items.Add(item.ToString());
            }
            foreach (var item in payer)
            {
                cbbPayerPay.Properties.Items.Add(item.ToString());
            }
            cbbReceiverPay.SelectedIndex = 0;
            cbbPayerPay.SelectedIndex = 0;
        }

        private void btnInsertPay_Click(object sender, EventArgs e)
        {
            if (isDigit(txtMoneyPay.Text) == true)
            {
                if (txtDescribePay.Text.Trim() != "")
                {
                    //ReceiveTable receive = new ReceiveTable();
                    //receive.Id = Int32.Parse(txtIdReceive.Text);
                    //receive.InputDate = DateTime.Parse(txtInputDateReceive.Text);
                    //receive.Describe = txtDescribeReceive.Text;
                    //receive.IdReceiver = ReceiveDAO.Instance.GetIdReceiverByName(cbbReceiverReceive.Text);
                    //receive.IdPayer = ReceiveDAO.Instance.GetIdPayerByName(cbbPayerReceive.Text);
                    //receive.IdProduct = ReceiveDAO.Instance.GetIdProductByName(cbbProduct.Text);
                    //receive.ExportProduct = float.Parse(txtExportReceive.Text);
                    //receive.PriceInDay = Int32.Parse(txtPriceInDayReceive.Text);
                    //receive.Money = float.Parse(txtMoneyReceive.Text);
                    //if (ReceiveDAO.Instance.UpdateReceipt(receive) == true) { MessageBox.Show("Update success!"); LoadReceive(); }
                    //else MessageBox.Show("Update fail!");
                }
                else MessageBox.Show("Lí Do Chi Tiền Không Được Để Trống!");
            }
            else MessageBox.Show("Số Tiền Chi Phải Là Số!");
        }

        private void btnUpdatePay_Click(object sender, EventArgs e)
        {

        }

        private void btnDeletePay_Click(object sender, EventArgs e)
        {

        }

        private void btnRefreshPay_Click(object sender, EventArgs e)
        {
            txtIdPay.Text = "";
            txtDescribePay.Text = "";
            txtInputDatePay.DateTime = DateTime.Today;
            cbbPayerPay.SelectedIndex = 0;
            cbbReceiverPay.SelectedIndex = 0;
            txtMoneyPay.Text = "0";
        }
    }
}
