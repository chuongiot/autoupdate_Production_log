using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace NhatKySanXuat
{
    public partial class changepassword : Form
    {
        public changepassword()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (tbnewpass.Text == tballowpass.Text)
            {
                try
                {
                    string sqltring = "Data Source = 192.168.23.48,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016";
                    SqlConnection sqlcon = new SqlConnection(sqltring);
                    SqlCommand cmd = new SqlCommand();
                    sqlcon.Open();
                    if (sqlcon.State == ConnectionState.Open)
                    {
                        cmd = sqlcon.CreateCommand();
                        cmd.CommandText = "update dangnhap set password ='" + tbnewpass.Text + "' where user1 = '" + tbuser.Text + "'";
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void checkBoxshowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxshowpassword.Checked == true)
            {
                tboldpassword.PasswordChar = (char)0;
                tbnewpass.PasswordChar = (char)0;
                tballowpass.PasswordChar = (char)0;
            }
            else
            {
                tboldpassword.PasswordChar = '*';
                tbnewpass.PasswordChar = '*';
                tballowpass.PasswordChar = '*';
            }
        }
    }
}
