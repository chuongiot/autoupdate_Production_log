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
    public partial class Change_LOT : Form
    {
        public Change_LOT()
        {
            InitializeComponent();
        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lot_old.Text != "" && tb_dotsx_old.Text != "" && tb_so_me_old.Text != "" && tb_thiet_bi_old.Text != "")
                {
                    string sqlupdate = "UPDATE nhatkysanxuat set LOT = '" + tb_lot_new.Text + "' WHERE dot_sx ='" + tb_dotsx_old.Text + "' AND me ='" + tb_so_me_old.Text + "' AND thiet_bi ='" + tb_thiet_bi_old.Text + "'";
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand cmd = new SqlCommand(sqlupdate, sqlcon);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập Nhật Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlcon.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
