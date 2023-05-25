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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        SqlConnection sqlcon = null;
        string sqltring = "Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password = mylan@2016";
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBoxshowpass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxshowpass.Checked == true)
            {
                tbpassword.PasswordChar = (char)0;
            }
            else
            {
                tbpassword.PasswordChar = '*';
            }
        }

        private void bttdangnhap_Click(object sender, EventArgs e)
        {
            try
            {
                sqlcon = new SqlConnection(sqltring);
                sqlcon.Open();
                string dangnhap = " select * from dangnhap where user1 = '" + tbuser.Text + "'and password = '" + tbpassword.Text + "'";
                SqlCommand cmd = new SqlCommand(dangnhap, sqlcon);
                SqlDataReader data = cmd.ExecuteReader();
                if (data.Read() == true)
                {
                    this.Hide();
                    Logsx form2 = new Logsx(tbuser.Text);
                    form2.ShowDialog();
                    form2 = null;
                    this.Show();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại !","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void Login_Load(object sender, EventArgs e)
        {
            tbuser.Text = Properties.Settings.Default.user;
            tbpassword.Text = Properties.Settings.Default.password;
            if (Properties.Settings.Default.user != "")
            {
                checkBox_save_tk.Checked = true;
            }
        }
        private void btnchangepassword_Click(object sender, EventArgs e)
        {
            changepassword formchange = new changepassword();
            formchange.ShowDialog();
        }

        private void tbpassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try
                {
                    sqlcon = new SqlConnection(sqltring);
                    sqlcon.Open();
                    string dangnhap = " select * from dangnhap where user1 = '" + tbuser.Text + "'and password = '" + tbpassword.Text + "'";
                    SqlCommand cmd = new SqlCommand(dangnhap, sqlcon);
                    SqlDataReader data = cmd.ExecuteReader();
                    if (data.Read() == true)
                    {
                        this.Hide();
                        Logsx form2 = new Logsx(tbuser.Text);
                        form2.ShowDialog();
                        form2 = null;
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Đăng nhập thất bại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void checkBox_save_tk_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_save_tk.Checked == true)
            {
                if(tbuser.Text=="" || tbpassword.Text == "")
                {
                    MessageBox.Show("Chưa nhập tài khoản hoặc mật khẩu !");
                    checkBox_save_tk.Checked = false;
                }
                else
                {
                    Properties.Settings.Default.user = tbuser.Text;
                    Properties.Settings.Default.password = tbpassword.Text;
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                Properties.Settings.Default.Reset();
            }
        }
    }
}
