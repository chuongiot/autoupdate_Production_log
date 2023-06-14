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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Data.OleDb;

namespace NhatKySanXuat
{
    public partial class Logsx : Form
    {
        string file_name = "";
        int status_search = 0;
        public Logsx(string username)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            lbuser.Text = username;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageblogsx;
            pnloading.Visible = false;
            button_search.Enabled = false;
            //ThreadStart threadStart = new ThreadStart(load_data_with_date);
            //Thread thread = new Thread(threadStart);
            //thread.Start();
            //thread.IsBackground = true;
            load_data_with_date();
            load_log();
            LoadQLSX("Select DOT_SX,ME_THU,SO_LOT,MA_TB,TG_BD,TG_KT,LOAI_SP,KL_NL,NV_VH,TRUONG_CA from DataSX_RSF WHERE MA_TB = 'S1' ORDER BY TG_BD DESC");
            loadcbbma_BTP();
            loadcbbma_NVL();
            loadcbb_Loai();
            //this.reportViewer_xuatkho.RefreshReport();
            //this.reportViewer_xuatkho.LocalReport.Refresh();
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Red;
        }
        private void btthem_Click(object sender, EventArgs e)
        {
            /*DialogResult dialogResult = MessageBox.Show("Bạn có muốn thêm LOT : " + tblot.Text + "", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.OK)
            {
                insert_data();
            }*/
            Form3 form3 = new Form3("");
            form3.ShowDialog();
        }
        private void btcapnhat_Click(object sender, EventArgs e)
        {
            //DialogResult dialogResult = MessageBox.Show("Bạn có muốn cập nhật", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            //if (dialogResult == DialogResult.OK)
           // {
            //    update();
           // }
        }
        private void btxoa_Click(object sender, EventArgs e)
        {
            //delete();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult dlr = MessageBox.Show("Bạn Có Muốn Thoát", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //if (dlr == DialogResult.No) e.Cancel = true;
        }
        private void btxuat_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file_name = saveFileDialog1.FileName;
                pnloading.Visible = true;
                ThreadStart threadStart = new ThreadStart(export);
                Thread thread = new Thread(threadStart);
                thread.Start();
                thread.IsBackground = true;
            }
        }
        private void Logsx_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        public void export()
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                worksheet.Name = "Nhật ký sản xuất";
                try
                {
                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                    {
                        worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                    }
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                worksheet.Cells[i + 2, j + 1] = "";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                workbook.SaveAs(file_name);
                MessageBox.Show("Xuất dữ liệu ra Excel thành công!");
                workbook.Close();
                excel.Quit();
                pnloading.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }
        public void insert_blogtruycap(string hoat_dong)
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
            sqlcon.Open();
            string Thoi_gian = DateTime.Now.ToString();
            string user = lbuser.Text;
            SqlCommand cmd = sqlcon.CreateCommand();
            cmd.CommandText = "insert into logtruycap (ten_user,thoi_gian,hoat_dong) values ('" + user + "','" + Thoi_gian + "',N'" + hoat_dong + "')";
            cmd.ExecuteNonQuery();
            sqlcon.Close();
            load_log();
        }
        public void load_log()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                DataTable tb_log = new DataTable();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                cmd = sqlcon.CreateCommand();
                cmd.CommandText = "select * from logtruycap ORDER BY thoi_gian DESC";
                sqlDataAdapter.SelectCommand = cmd;
                tb_log.Clear();
                sqlDataAdapter.Fill(tb_log);
                sqlcon.Close();
                DataRow[] rows = tb_log.Select();
                dataGridViewblogtruycap.Rows.Clear();
                for (int i = 0; i < rows.Length; i++)
                {
                    dataGridViewblogtruycap.Rows.Add(rows[i]["thoi_gian"], rows[i]["ten_user"], rows[i]["hoat_dong"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnxoals_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn muốn xóa lịch sử truy cập", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.OK)
            {
                if (lbuser.Text == "admin")
                {
                    try
                    {
                        SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd = sqlcon.CreateCommand();
                        cmd.CommandText = "delete from logtruycap";
                        cmd.ExecuteNonQuery();
                        sqlcon.Close();
                        load_log();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("không được quyền xóa", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    try
                    {
                        insert_blogtruycap("Đang cố xóa lịch sử truy cập !!!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void panel_coat_s1_Click(object sender, EventArgs e)
        {

            panel_coat_s1.BorderStyle = BorderStyle.Fixed3D;
            panel_coat_s1.BackColor = Color.Lime;
            label_coat_s1.ForeColor = Color.White;
            ///
            panel_coat_02.BorderStyle = BorderStyle.FixedSingle;
            panel_coat_02.BackColor = Color.Silver;
            label_coat_02.ForeColor = Color.Black;
            ///
            ///
            tabControl1.SelectedTab = tabPageblogsx;
            string sql_query = "Select DOT_SX,ME_THU,SO_LOT,MA_TB,TG_BD,TG_KT,LOAI_SP,KL_NL,NV_VH,TRUONG_CA from DataSX_RSF WHERE MA_TB = 'S1' ORDER BY TG_BD DESC";
            LoadQLSX(sql_query);
        }
        private void panel_coat_02_Click(object sender, EventArgs e)
        {

            panel_coat_02.BorderStyle = BorderStyle.Fixed3D;
            panel_coat_02.BackColor = Color.Lime;
            label_coat_02.ForeColor = Color.White;
            ////
            panel_coat_s1.BorderStyle = BorderStyle.FixedSingle;
            panel_coat_s1.BackColor = Color.Silver;
            label_coat_s1.ForeColor = Color.Black;
            ///
            ///
            tabControl1.SelectedTab = tabPageblogsx;
            string sql_query = "Select DOT_SX,ME_THU,SO_LOT,MA_TB,TG_BD,TG_KT,LOAI_SP,KL_NL,NV_VH,TRUONG_CA from DataSX_RSF WHERE MA_TB = '02' ORDER BY TG_BD DESC";
            LoadQLSX(sql_query);
        }
        public void LoadQLSX(string query)
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=QL_SX;User ID=sa;Password=rynan2020");
                sqlcon.Open();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                cmd = sqlcon.CreateCommand();
                cmd.CommandText = query;
                sqlDataAdapter.SelectCommand = cmd;
                DataTable dt_a = new DataTable();
                dt_a.Clear();
                sqlDataAdapter.Fill(dt_a);
                dgv_coater_s1.DataSource = dt_a;
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgv_coater_s1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Form3 form3 = new Form3(dgv_coater_s1.SelectedRows[0].Cells[2].Value.ToString());
                form3.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pn_history_Click(object sender, EventArgs e)
        {
            pn_nksx_button.BackColor = Color.Silver;
            pn_nksx_button.BorderStyle = BorderStyle.FixedSingle;
            lb_nksx.ForeColor = Color.Black;

            panel_nhap_release.BackColor = Color.Silver;
            panel_nhap_release.BorderStyle = BorderStyle.FixedSingle;
            lb_nhap_release.ForeColor = Color.Black;

            pn_history.BorderStyle = BorderStyle.Fixed3D;
            pn_history.BackColor = Color.Lime;
            lb_history.ForeColor = Color.White;
            ///
            pn_import.BackColor = Color.Silver;
            pn_import.BorderStyle = BorderStyle.FixedSingle;
            lb_import.ForeColor = Color.Black;
            tabControl1.SelectedTab = tabPagehistorylogin;

            pnxuatkhonvl.BackColor = Color.Silver;
            pnxuatkhonvl.BorderStyle = BorderStyle.FixedSingle;
            lb_xuatkhonvl.ForeColor = Color.Black;
            load_log();
        }
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {  
            try
            {
                Form3 form3 = new Form3(dataGridView1.SelectedRows[0].Cells[7].Value.ToString());
                form3.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void load_data_with_date_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC ";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_date()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC ";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_loai_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND loai = '" + cbb_search_loai.Text + "' ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 11, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_loai()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND loai = '" + cbb_search_loai.Text + "' ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 11, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_dotsx()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", row.Length.ToString(), "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Math.Round(Tong_N1_KL, 4), "", "", Math.Round(Tong_N2_KL, 4), "", "", Math.Round(Tong_N3_KL, 4), "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_dotsx_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND dot_sx = '" + tb_dotsx_search.Text + "' ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", row.Length.ToString(), "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_phan_bon_nvl()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_phan_bon_nvl_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_ma_BTP()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_ma_BTP_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_ALL()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' dot_sx = '" + tb_dotsx_search.Text + "' AND loai = '" + cbb_search_loai.Text + "' AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_ALL_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND dot_sx = '" + tb_dotsx_search.Text + "' AND loai = '" + cbb_search_loai.Text + "' AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_dotsx_loai()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where loai = '" + cbb_search_loai.Text + "' AND dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", row.Length.ToString(), "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_dotsx_loai_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND loai = '" + cbb_search_loai.Text + "' AND dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", row.Length.ToString(), "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_dotsx_BTP()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", row.Length.ToString(), "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_dotsx_BTP_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_dotsx_NVL()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_dotsx_NVL_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where thiet_bi = '" + cbb_thietbi_search.Text + "' AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY me ASC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "",row.Length.ToString(), "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_loai_ma_BTP_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND loai = '" + cbb_search_loai.Text + "' ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_loai_ma_BTP()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND loai = '" + cbb_search_loai.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_loai_NVL_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND loai = '" + cbb_search_loai.Text + "' ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_loai_NVL()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND loai = '" + cbb_search_loai.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_BTP_NVL_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_BTP_NVL()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' AND ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_dotsx_loai_BTP_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where dot_sx = '" + tb_dotsx_search.Text + "' AND loai = '" + cbb_search_loai.Text + "' AND thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' ORDER BY me DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_dotsx_loai_BTP()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where dot_sx = '" + tb_dotsx_search.Text + "' AND loai = '" + cbb_search_loai.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' ORDER BY me DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_dotsx_loai_NVL_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where dot_sx = '" + tb_dotsx_search.Text + "' AND loai = '" + cbb_search_loai.Text + "' AND thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' ORDER BY me DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", row.Length.ToString(), "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_dotsx_loai_NVL()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where dot_sx = '" + tb_dotsx_search.Text + "' AND loai = '" + cbb_search_loai.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' ORDER BY me DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_LOAI_BTP_NVL_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND loai = '" + cbb_search_loai.Text + "' AND thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_LOAI_BTP_NVL()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND loai = '" + cbb_search_loai.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' ORDER BY dot_sx DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }

        public void load_data_with_DOTSX_BTP_NVL_S1_02()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND dot_sx = '" + tb_dotsx_search.Text + "' AND thiet_bi = '" + cbb_thietbi_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' ORDER BY me DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        public void load_data_with_DOTSX_BTP_NVL()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                sqlcon.Open();
                command = sqlcon.CreateCommand();
                command.CommandText = "select * from nhatkysanxuat where ma_BTP LIKE '%" + cbb_ma_BTP_search.Text + "%' AND dot_sx = '" + tb_dotsx_search.Text + "' AND ngay_sx between cast('" + dateTimePickerFrom.Text + "' as date) and cast('" + dateTimePickerTo.Text + "' as date) AND phanbon_nvl LIKE '%" + cbb_phanbonnvl_search.Text + "%' ORDER BY me DESC";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                dataGridView1.Rows.Clear();
                double TONG_KLSP = 0;
                double TONG_KL_DONGKHOI = 0;
                double TONG_KHOILUONG_KHONG_DONG_KHOI = 0;
                double KHOI_LUONG_NVL = 0;
                double TONG_KL_LT = 0;
                double Tong_N1_KL = 0;
                double Tong_N2_KL = 0;
                double Tong_N3_KL = 0;
                double Tong_ga3 = 0;
                double Tong_borax = 0;
                double Tong_Naa = 0;
                double Tong_sodium = 0;
                double Tong_citric = 0;
                double Tong_naoh = 0;
                double Tong_solubo = 0;
                double Tong_edtazn = 0;
                double Tong_red = 0;
                double Tong_violet = 0;
                double Tong_blue = 0;
                double Tong_yellow = 0;
                double Tong_black = 0;
                double Tong_prev = 0;
                double Tong_thancam = 0;
                double Tong_dien = 0;
                double Tong_nuocro = 0;
                double Tong_nuocthuycuc = 0;
                double Hieu_suat_thu_tb = 0;
                double Hieu_suat_release_tb = 0;
                double tb_0ngay = 0;
                int count_0 = 0;
                double tb_7ngay = 0;
                int count_7 = 0;
                double tb_14ngay = 0;
                int count_14 = 0;
                double tb_21ngay = 0;
                int count_21 = 0;
                double tb_28ngay = 0;
                int count_28 = 0;
                double tb_42ngay = 0;
                int count_42 = 0;
                double tb_49ngay = 0;
                int count_49 = 0;
                double tb_56ngay = 0;
                int count_56 = 0;
                double tb_70ngay = 0;
                int count_70 = 0;
                double tb_84ngay = 0;
                int count_84 = 0;
                double tb_98ngay = 0;
                int count_98 = 0;
                double tb_112ngay = 0;
                int count_112 = 0;
                double tb_126ngay = 0;
                int count_126 = 0;
                double tb_140ngay = 0;
                int count_140 = 0;
                double tb_do_am = 0;
                int count_doam = 0;
                double tb_coating = 0;
                int count_coating = 0;
                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i]["ngay_0"].ToString() != "" && row[i]["ngay_0"].ToString() != "0")
                    {
                        count_0++;
                        tb_0ngay += Convert.ToDouble(row[i]["ngay_0"].ToString());
                    }
                    if (row[i]["ngay_7"].ToString() != "" && row[i]["ngay_7"].ToString() != "0")
                    {
                        count_7++;
                        tb_7ngay += Convert.ToDouble(row[i]["ngay_7"].ToString());
                    }
                    if (row[i]["ngay_14"].ToString() != "" && row[i]["ngay_14"].ToString() != "0")
                    {
                        count_14++;
                        tb_14ngay += Convert.ToDouble(row[i]["ngay_14"].ToString());
                    }
                    if (row[i]["ngay_21"].ToString() != "" && row[i]["ngay_21"].ToString() != "0")
                    {
                        count_21++;
                        tb_21ngay += Convert.ToDouble(row[i]["ngay_21"].ToString());
                    }
                    if (row[i]["ngay_28"].ToString() != "" && row[i]["ngay_28"].ToString() != "0")
                    {
                        count_28++;
                        tb_28ngay += Convert.ToDouble(row[i]["ngay_28"].ToString());

                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_42"].ToString() != "" && row[i]["ngay_42"].ToString() != "0")
                    {
                        count_42++;
                        tb_42ngay += Convert.ToDouble(row[i]["ngay_42"].ToString());
                    }
                    if (row[i]["ngay_49"].ToString() != "" && row[i]["ngay_49"].ToString() != "0")
                    {
                        count_49++;
                        tb_49ngay += Convert.ToDouble(row[i]["ngay_49"].ToString());
                    }
                    if (row[i]["ngay_56"].ToString() != "" && row[i]["ngay_56"].ToString() != "0")
                    {
                        count_56++;
                        tb_56ngay += Convert.ToDouble(row[i]["ngay_56"].ToString());
                    }
                    if (row[i]["ngay_70"].ToString() != "" && row[i]["ngay_70"].ToString() != "0")
                    {
                        count_70++;
                        tb_70ngay += Convert.ToDouble(row[i]["ngay_70"].ToString());
                    }
                    if (row[i]["ngay_84"].ToString() != "" && row[i]["ngay_84"].ToString() != "0")
                    {
                        count_84++;
                        tb_84ngay += Convert.ToDouble(row[i]["ngay_84"].ToString());
                    }
                    if (row[i]["ngay_98"].ToString() != "" && row[i]["ngay_98"].ToString() != "0")
                    {
                        count_98++;
                        tb_98ngay += Convert.ToDouble(row[i]["ngay_98"].ToString());
                    }
                    if (row[i]["ngay_112"].ToString() != "" && row[i]["ngay_112"].ToString() != "0")
                    {
                        count_112++;
                        tb_112ngay += Convert.ToDouble(row[i]["ngay_112"].ToString());
                    }
                    if (row[i]["ngay_126"].ToString() != "" && row[i]["ngay_126"].ToString() != "0")
                    {
                        count_126++;
                        tb_126ngay += Convert.ToDouble(row[i]["ngay_126"].ToString());
                    }
                    if (row[i]["ngay_140"].ToString() != "" && row[i]["ngay_140"].ToString() != "0")
                    {
                        count_140++;
                        tb_140ngay += Convert.ToDouble(row[i]["ngay_140"].ToString());
                    }
                    if (row[i]["do_am"].ToString() != "" && row[i]["do_am"].ToString() != "0")
                    {
                        count_doam++;
                        tb_do_am += Convert.ToDouble(row[i]["do_am"].ToString());
                    }
                    if (row[i]["coating_layer"].ToString() != "" && row[i]["coating_layer"].ToString() != "0")
                    {
                        count_coating++;
                        tb_coating += Convert.ToDouble(row[i]["coating_layer"].ToString());
                    }
                    string Nguoi_nhap = row[i]["name"].ToString();
                    string LOT = row[i]["LOT"].ToString();
                    string Dot_sx = row[i]["dot_sx"].ToString();
                    string Ngay_sx = row[i]["ngay_sx"].ToString();
                    string Thiet_bi = row[i]["thiet_bi"].ToString();
                    string Ma_btp = row[i]["ma_BTP"].ToString();
                    string Ten_btp = row[i]["ten_BTP"].ToString();
                    string Me = row[i]["me"].ToString();
                    string Kl_nvl = row[i]["klnl_sudung"].ToString();
                    string Toc_do_release = row[i]["tocdo_release"].ToString();
                    string Ngay_release = row[i]["ngay_release"].ToString();
                    string Loai = row[i]["loai"].ToString();
                    string Tong_klsp_thuduoc = row[i]["tong_klspsx"].ToString();
                    if (Tong_klsp_thuduoc == "")
                        Tong_klsp_thuduoc = "0";
                    TONG_KLSP += Convert.ToDouble(Tong_klsp_thuduoc);
                    string Kl_dongkhoi = row[i]["kl_dongkhoi"].ToString();
                    if (Kl_dongkhoi == "")
                        Kl_dongkhoi = "0";
                    TONG_KL_DONGKHOI += Convert.ToDouble(Kl_dongkhoi);
                    string Khongdongkhoi = row[i]["kl_khongdongkhoi"].ToString();
                    if (Khongdongkhoi == "")
                        Khongdongkhoi = "0";
                    TONG_KHOILUONG_KHONG_DONG_KHOI += Convert.ToDouble(Khongdongkhoi);
                    string Kl_lythuyet = row[i]["kl_lythuyet"].ToString();
                    if (Kl_lythuyet == "")
                        Kl_lythuyet = "0";
                    TONG_KL_LT += Convert.ToDouble(Kl_lythuyet);
                    string Hieusuatthu = row[i]["hieuxuat_thu"].ToString();
                    if (Hieusuatthu == "")
                        Hieusuatthu = "0";
                    Hieu_suat_thu_tb += Convert.ToDouble(Hieusuatthu);
                    string Hieusuatrelease = row[i]["hieuxuat_release"].ToString();
                    if (Hieusuatrelease == "")
                        Hieusuatrelease = "0";
                    Hieu_suat_release_tb += Convert.ToDouble(Hieusuatrelease);
                    string Thoigiancb = row[i]["thoigian_cb"].ToString();
                    string Thoigiansx = row[i]["thoigian_sx"].ToString();
                    string Phanbon_nvl = row[i]["phanbon_nvl"].ToString();
                    string KL_phan_nvl = row[i]["kl_nvl"].ToString();
                    if (KL_phan_nvl == "")
                        KL_phan_nvl = "0";
                    KHOI_LUONG_NVL += Convert.ToDouble(KL_phan_nvl);
                    string Barcode_nvl = row[i]["barcode_nvl"].ToString();
                    string LOT_nvl = row[i]["lot_nvl"].ToString();
                    string N1_khoiluong = row[i]["N1"].ToString();
                    if (N1_khoiluong == "")
                        N1_khoiluong = "0";
                    Tong_N1_KL += Convert.ToDouble(N1_khoiluong);
                    string N1_barcode = row[i]["barcode_n1"].ToString();
                    string N1_LOT = row[i]["lot_n1"].ToString();
                    string N2_khoiluong = row[i]["N2"].ToString();
                    if (N2_khoiluong == "")
                        N2_khoiluong = "0";
                    Tong_N2_KL += Convert.ToDouble(N2_khoiluong);
                    string N2_barcode = row[i]["barcode_n2"].ToString();
                    string N2_LOT = row[i]["lot_n2"].ToString();
                    string n3_khoiluong = row[i]["N3"].ToString();
                    if (n3_khoiluong == "")
                        n3_khoiluong = "0";
                    Tong_N3_KL += Convert.ToDouble(n3_khoiluong);
                    string N3_barcode = row[i]["barcode_n3"].ToString();
                    string N3_LOT = row[i]["lot_n3"].ToString();
                    string GA3 = row[i]["Ga3"].ToString();
                    if (GA3 == "")
                        GA3 = "0";
                    Tong_ga3 += Convert.ToDouble(GA3);
                    string GA3_barcode = row[i]["barcode_ga3"].ToString();
                    string Borax = row[i]["Borax"].ToString();
                    if (Borax == "")
                        Borax = "0";
                    Tong_borax += Convert.ToDouble(Borax);
                    string Borax_barcode = row[i]["bacode_borax"].ToString();
                    string NAA = row[i]["Naa"].ToString();
                    if (NAA == "")
                        NAA = "0";
                    Tong_Naa += Convert.ToDouble(NAA);
                    string NAA_barcode = row[i]["barcode_naa"].ToString();
                    string Sodium = row[i]["Sodium"].ToString();
                    if (Sodium == "")
                        Sodium = "0";
                    Tong_sodium += Convert.ToDouble(Sodium);
                    string Sodium_barcode = row[i]["barcode_sodium"].ToString();
                    string Citric = row[i]["Citric"].ToString();
                    if (Citric == "")
                        Citric = "0";
                    Tong_citric += Convert.ToDouble(Citric);
                    string Barcode_Citric = row[i]["barcode_citric"].ToString();
                    string Naoh = row[i]["Naoh"].ToString();
                    if (Naoh == "")
                        Naoh = "0";
                    Tong_naoh += Convert.ToDouble(Naoh);
                    string Barcode_Naoh = row[i]["barocde_naoh"].ToString();
                    string Solubo = row[i]["solubo"].ToString();
                    if (Solubo == "")
                        Solubo = "0";
                    Tong_solubo += Convert.ToDouble(Solubo);
                    string Barcode_Solubo = row[i]["barocde_solubo"].ToString();
                    string Edtazn = row[i]["Edta"].ToString();
                    if (Edtazn == "")
                        Edtazn = "0";
                    Tong_edtazn += Convert.ToDouble(Edtazn);
                    string Barcode_Edta = row[i]["barcode_edta"].ToString();
                    string Red = row[i]["Red"].ToString();
                    if (Red == "")
                        Red = "0";
                    Tong_red += Convert.ToDouble(Red);
                    string Barcode_red = row[i]["barcode_red"].ToString();
                    string Violet = row[i]["violet"].ToString();
                    if (Violet == "")
                        Violet = "0";
                    Tong_violet += Convert.ToDouble(Violet);
                    string Barcode_violet = row[i]["barcode_violet"].ToString();
                    string Blue = row[i]["blue"].ToString();
                    if (Blue == "")
                        Blue = "0";
                    Tong_blue += Convert.ToDouble(Blue);
                    string Barcode_blue = row[i]["barocde_blue"].ToString();
                    string Yellow = row[i]["yellow"].ToString();
                    if (Yellow == "")
                        Yellow = "0";
                    Tong_yellow += Convert.ToDouble(Yellow);
                    string Barcode_yellow = row[i]["barcode_yellow"].ToString();
                    string Black = row[i]["black"].ToString();
                    if (Black == "")
                        Black = "0";
                    Tong_black += Convert.ToDouble(Black);
                    string Barcode_black = row[i]["barcode_back"].ToString();
                    string Prev = row[i]["prev"].ToString();
                    if (Prev == "")
                        Prev = "0";
                    Tong_prev += Convert.ToDouble(Prev);
                    string Barcode_Prev = row[i]["barcode_prev"].ToString();
                    string Than_cam = row[i]["thancam"].ToString();
                    if (Than_cam == "")
                        Than_cam = "0";
                    Tong_thancam += Convert.ToDouble(Than_cam);
                    string Dien = row[i]["dien"].ToString();
                    if (Dien == "")
                        Dien = "0";
                    Tong_dien += Convert.ToDouble(Dien);
                    string Nuoc_RO = row[i]["nuocRo"].ToString();
                    if (Nuoc_RO == "")
                        Nuoc_RO = "0";
                    Tong_nuocro += Convert.ToDouble(Nuoc_RO);
                    string Nuoc_thuycuc = row[i]["nuocthuycuc"].ToString();
                    if (Nuoc_thuycuc == "")
                        Nuoc_thuycuc = "0";
                    Tong_nuocthuycuc += Convert.ToDouble(Nuoc_thuycuc);
                    string BHLD = row[i]["BHLD"].ToString();
                    string Ghi_chu = row[i]["ghi_chu"].ToString();
                    string Vitri_tongspthuduoc = row[i]["vitri_spthuduoc"].ToString();
                    string Vitri_spdongkhoi = row[i]["vitri_spdongkhoi"].ToString();
                    string Vitri_spkhongdongkhoi = row[i]["vitri_spkhongdongkhoi"].ToString();
                    string do_am = row[i]["do_am"].ToString();
                    string coating_layer = row[i]["coating_layer"].ToString();
                    string thoigian_ondinh = row[i]["thoigian_ondinh"].ToString();
                    string ngay0 = row[i]["ngay_0"].ToString();
                    string ngay7 = row[i]["ngay_7"].ToString();
                    string ngay14 = row[i]["ngay_14"].ToString();
                    string ngay21 = row[i]["ngay_21"].ToString();
                    string ngay28 = row[i]["ngay_28"].ToString();
                    string ngay42 = row[i]["ngay_42"].ToString();
                    string ngay49 = row[i]["ngay_49"].ToString();
                    string ngay56 = row[i]["ngay_56"].ToString();
                    string ngay70 = row[i]["ngay_70"].ToString();
                    string ngay84 = row[i]["ngay_84"].ToString();
                    string ngay98 = row[i]["ngay_98"].ToString();
                    string ngay112 = row[i]["ngay_112"].ToString();
                    string ngay126 = row[i]["ngay_126"].ToString();
                    string ngay140 = row[i]["ngay_140"].ToString();
                    dataGridView1.Rows.Add(Nguoi_nhap, Dot_sx, Ngay_sx, Thiet_bi, Ma_btp,
                        Ten_btp, Me, LOT, Toc_do_release, Ngay_release, Loai, Tong_klsp_thuduoc,
                        Vitri_tongspthuduoc, Kl_dongkhoi, Vitri_spdongkhoi, Khongdongkhoi,
                        Vitri_spkhongdongkhoi, Kl_lythuyet, Hieusuatthu, Hieusuatrelease, Thoigiancb,
                        Thoigiansx, Phanbon_nvl, KL_phan_nvl, Barcode_nvl, LOT_nvl, N1_khoiluong, N1_barcode,
                        N1_LOT, N2_khoiluong, N2_barcode, N2_LOT, n3_khoiluong, N3_barcode, N3_LOT, GA3, GA3_barcode,
                        Borax, Borax_barcode, NAA, NAA_barcode, Sodium, Sodium_barcode, Citric, Barcode_Citric, Naoh,
                        Barcode_Naoh, Solubo, Barcode_Solubo, Edtazn, Barcode_Edta, Red, Barcode_red, Violet, Barcode_violet,
                        Blue, Barcode_blue, Yellow, Barcode_yellow, Black, Barcode_black, Prev, Barcode_Prev, Than_cam, Dien,
                        Nuoc_RO, Nuoc_thuycuc, BHLD, Ghi_chu, do_am, coating_layer, thoigian_ondinh, ngay0, ngay7, ngay14, ngay21,
                        ngay28, ngay42, ngay49, ngay56, ngay70, ngay84, ngay98, ngay112, ngay126, ngay140);
                }
                dataGridView1.Rows.Add("Tổng", "", "", "", "", "", "", "", "", "", "", TONG_KLSP, "", TONG_KL_DONGKHOI, "", TONG_KHOILUONG_KHONG_DONG_KHOI,
                                "", Math.Round(TONG_KL_LT, 4), Math.Round(Hieu_suat_thu_tb / dataGridView1.Rows.Count, 4), Math.Round(Hieu_suat_release_tb / dataGridView1.Rows.Count, 4),
                                "", "", "", KHOI_LUONG_NVL, "", "", Tong_N1_KL, "", "", Tong_N2_KL, "", "", Tong_N3_KL, "", "", Tong_ga3, "", Tong_borax, "", Tong_Naa, "", Tong_sodium, "", Tong_citric, "", Tong_naoh,
                                "", Tong_solubo, "", Tong_edtazn, "", Tong_red, "", Tong_violet, "", Tong_blue, "", Tong_yellow, "", Tong_black, "", Tong_prev, "", Tong_thancam, Tong_dien, Tong_nuocro, Tong_nuocthuycuc,
                                "", "", Math.Round(tb_do_am / count_doam, 4), Math.Round(tb_coating / count_coating, 4), "",
                                Math.Round(tb_0ngay / count_0, 4), Math.Round(tb_7ngay / count_7, 4), Math.Round(tb_14ngay / count_14, 4),
                                Math.Round(tb_21ngay / count_21, 4), Math.Round(tb_28ngay / count_28, 4), Math.Round(tb_42ngay / count_42, 4),
                                Math.Round(tb_49ngay / count_49, 4), Math.Round(tb_56ngay / count_56, 4), Math.Round(tb_70ngay / count_70, 4),
                                Math.Round(tb_84ngay / count_84, 4), Math.Round(tb_98ngay / count_98, 4), Math.Round(tb_112ngay / count_112, 4),
                                Math.Round(tb_126ngay / count_126, 4), Math.Round(tb_140ngay / count_140, 4));
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Orange;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            pnloading.Visible = false;
            button_search.Enabled = true;
        }
        private void pn_import_Click(object sender, EventArgs e)
        {
            pn_nksx_button.BackColor = Color.Silver;
            pn_nksx_button.BorderStyle = BorderStyle.FixedSingle;
            lb_nksx.ForeColor = Color.Black;

            pn_history.BackColor = Color.Silver;
            pn_history.BorderStyle = BorderStyle.FixedSingle;
            lb_history.ForeColor = Color.Black;

            pn_import.BackColor = Color.Lime;
            pn_import.BorderStyle = BorderStyle.Fixed3D;
            lb_import.ForeColor = Color.White;
            tabControl1.SelectedTab = tabPageImportexcel;

            panel_nhap_release.BackColor = Color.Silver;
            panel_nhap_release.BorderStyle = BorderStyle.FixedSingle;
            lb_nhap_release.ForeColor = Color.Black;

            pnxuatkhonvl.BackColor = Color.Silver;
            pnxuatkhonvl.BorderStyle = BorderStyle.FixedSingle;
            lb_xuatkhonvl.ForeColor = Color.Black;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opn = new OpenFileDialog();
            if (opn.ShowDialog() == DialogResult.OK)
            {
                this.textBox_path.Text = opn.FileName;
                ThreadStart thrstart = new ThreadStart(load_file_excel);
                Thread thrd = new Thread(thrstart);
                thrd.Start();
                thrd.IsBackground = true;
            }
        }
        public void load_file_excel()
        {
            try
            {
                string path = @"Provider = Microsoft.ACE.OLEDB.12.0 ; Data Source = '" + textBox_path.Text + "' ; Extended Properties" + "= 'Excel 12.0; HDR = YES';";
                OleDbConnection cnn = new OleDbConnection(path);
                OleDbDataAdapter adptr = new OleDbDataAdapter("select * from [Sheet1$]", cnn);
                DataTable dt = new DataTable();
                adptr.Fill(dt);
                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void insert_from_excel()
        {
            if (E_tb_lot.Text == "")
            {
                MessageBox.Show("Chưa Nhập LOT", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                string Nguoi_nhap = E_cbb_nguoi_nhap.Text;
                string LOT = E_tb_lot.Text;
                string Dotsx = E_tb_dotsx.Text;
                string Ngaysx = E_ngaysx.Text;
                string Thietbi = E_thietbi.Text;
                string Mabtp = E_ma_btp.Text;
                string Tenbtp = E_ten_btp.Text;
                string Me = E_so_me.Text;
                string Klnvl = "";
                string Tocdo_release = E_tocdo_release.Text;
                string Ngayrelease = E_ngay_release.Text;
                string Loai = E_loai.Text;
                string Tongklsp_thuduoc = E_tong_sp_thu.Text;
                string Kldongkhoi = E_sp_dongkhoi.Text;
                string Khongdongkhoi = E_sp_khong_dong_khoi.Text;
                string Kl_lythuyet = E_kl_ly_thuyet.Text;
                string Hieusuatthu = E_hieusuatthu.Text;
                string Hieusuatrelease = "";
                string Thoigiancb = E_thoigiancbsx.Text;
                string Thoigiansx = E_thoigiansx.Text;
                string Phanbon_nvl = E_phanbon_nvl.Text;
                string KL_phan_nvl = E_kl_phanbon_nvl.Text;
                string Barcode_nvl = E_barcode_phanbonnvl.Text;
                string LOT_nvl = E_lot_phanbonnvl.Text;
                string N1_khoiluong = E_n1.Text;
                string N1_barcode = E_n1_barcode.Text;
                string N1_LOT = E_n1_lot.Text;
                string N2_khoiluong = E_n2.Text;
                string N2_barcode = E_n2_barcode.Text;
                string N2_LOT = E_n2_lot.Text;
                string n3_khoiluong = E_n3.Text;
                string N3_barcode = E_n3_barcode.Text;
                string N3_LOT = E_n3_lot.Text;
                string GA3 = "";
                string GA3_barcode = "";
                string Borax = "";
                string Borax_barcode = "";
                string NAA = "";
                string NAA_barcode = "";
                string Sodium = "";
                string Sodium_barcode = "";
                string Citric = "";
                string Barcode_Citric = "";
                string Naoh = "";
                string Barcode_Naoh = "";
                string Solubo = "";
                string Barcode_Solubo = "";
                string Edtazn = "";
                string Barcode_Edta = "";
                string Red = "";
                string Barcode_red = "";
                string Violet = "";
                string Barcode_violet = "";
                string Blue = "";
                string Barcode_blue = "";
                string Yellow = "";
                string Barcode_yellow = "";
                string Black = "";
                string Barcode_black = "";
                string Prev = "";
                string Barcode_Prev = "";
                string Than_cam = "";
                string Dien = "";
                string Nuoc_RO = "";
                string Nuoc_thuycuc = "";
                string BHLD = "";
                string Ghi_chu = "";
                string Vitri_spthuduoc = "";
                string Vitri_spdongkhoi = "";
                string Vitri_spkhongdongkhoi = "";
                try
                {
                    sqlcon.Open();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "insert into nhatkysanxuat (name,dot_sx,ngay_sx,thiet_bi,ma_BTP,ten_BTP,me,LOT ,tocdo_release," +
                        "ngay_release,loai,klnl_sudung,tong_klspsx,kl_dongkhoi,kl_khongdongkhoi,kl_lythuyet,hieuxuat_thu,hieuxuat_release," +
                        "thoigian_cb,thoigian_sx,phanbon_nvl,kl_nvl,barcode_nvl,lot_nvl,N1,barcode_n1," +
                        "lot_n1,N2,barcode_n2,lot_n2,N3,barcode_n3,lot_n3,Ga3,barcode_ga3,Borax,bacode_borax,Naa,barcode_naa,solubo,barocde_solubo," +
                        "Edta,barcode_edta,Red,barcode_red,violet,barcode_violet,blue,barocde_blue,yellow,barcode_yellow,black,barcode_back,prev," +
                        "barcode_prev,thancam,dien,nuocRO,nuocthuycuc,BHLD,Sodium,barcode_sodium,Citric,barcode_citric,Naoh,barocde_naoh,ghi_chu," +
                        "vitri_spthuduoc,vitri_spdongkhoi,vitri_spkhongdongkhoi)" +
                        "values (N'" + Nguoi_nhap + "','" + Dotsx + "','" + Ngaysx + "','" + Thietbi + "','" + Mabtp + "','" + Tenbtp + "','" + Me + "','" + LOT + "','" + Tocdo_release + "'," +
                        "'" + Ngayrelease + "','" + Loai + "','" + Klnvl + "','" + Tongklsp_thuduoc + "','" + Kldongkhoi + "','" + Khongdongkhoi + "','" + Kl_lythuyet + "','" + Hieusuatthu + "'," +
                        "'" + Hieusuatrelease + "','" + Thoigiancb + "','" + Thoigiansx + "'," +
                        "'" + Phanbon_nvl + "','" + KL_phan_nvl + "','" + Barcode_nvl + "','" + LOT_nvl + "','" + N1_khoiluong + "','" + N1_barcode + "','" + N1_LOT + "','" + N2_khoiluong + "'," +
                        "'" + N2_barcode + "','" + N2_LOT + "','" + n3_khoiluong + "','" + N3_barcode + "','" + N3_LOT + "','" + GA3 + "','" + GA3_barcode + "','" + Borax + "','" + Borax_barcode + "'," +
                        "'" + NAA + "','" + NAA_barcode + "','" + Solubo + "','" + Barcode_Solubo + "','" + Edtazn + "','" + Barcode_Edta + "','" + Red + "','" + Barcode_red + "'," +
                        "'" + Violet + "','" + Barcode_violet + "','" + Blue + "','" + Barcode_blue + "','" + Yellow + "','" + Barcode_yellow + "','" + Black + "','" + Barcode_black + "'," +
                        "'" + Prev + "','" + Barcode_Prev + "','" + Than_cam + "','" + Dien + "','" + Nuoc_RO + "','" + Nuoc_thuycuc + "','" + BHLD + "','" + Sodium + "','" + Sodium_barcode + "','" + Citric + "'," +
                        "'" + Barcode_Citric + "','" + Naoh + "','" + Barcode_Naoh + "',N'" + Ghi_chu + "','" + Vitri_spthuduoc + "','" + Vitri_spdongkhoi + "','" + Vitri_spkhongdongkhoi + "')";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    insert_blogtruycap("Đã thêm LOT : " + E_tb_lot.Text);
                    sqlcon.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                E_tb_lot.Text = dataGridView2.SelectedRows[0].Cells[7].Value.ToString();
                E_tb_dotsx.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                string ngay = dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
                E_ngaysx.Text = ngay.Substring(6, 4) + '-' + ngay.Substring(3, 2) + '-' + ngay.Substring(0, 2);
                if (E_tb_lot.Text.Substring(0, 2) == "02")
                {
                    E_thietbi.Text = "02";
                }
                else
                {
                    E_thietbi.Text = "S1";
                }
                E_ten_btp.Text = dataGridView2.SelectedRows[0].Cells[5].Value.ToString();
                E_ma_btp.Text = dataGridView2.SelectedRows[0].Cells[4].Value.ToString();
                E_so_me.Text = dataGridView2.SelectedRows[0].Cells[6].Value.ToString();
                E_loai.Text = dataGridView2.SelectedRows[0].Cells[10].Value.ToString();
                E_tong_sp_thu.Text = dataGridView2.SelectedRows[0].Cells[12].Value.ToString();
                E_sp_dongkhoi.Text = dataGridView2.SelectedRows[0].Cells[13].Value.ToString();
                E_sp_khong_dong_khoi.Text = dataGridView2.SelectedRows[0].Cells[14].Value.ToString();
                E_kl_ly_thuyet.Text = dataGridView2.SelectedRows[0].Cells[15].Value.ToString();
                if (dataGridView2.SelectedRows[0].Cells[16].Value.ToString() == "")
                {
                    E_hieusuatthu.Text = "";
                }
                else
                {
                    double hieu_suat_thu = Math.Round(Convert.ToDouble(dataGridView2.SelectedRows[0].Cells[16].Value), 4);
                    E_hieusuatthu.Text = hieu_suat_thu.ToString();
                }
                E_kl_phanbon_nvl.Text = dataGridView2.SelectedRows[0].Cells[11].Value.ToString();
                E_tocdo_release.Text = dataGridView2.SelectedRows[0].Cells[8].Value.ToString();
                E_ngay_release.Text = dataGridView2.SelectedRows[0].Cells[9].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btt_insert_tt_database_Click(object sender, EventArgs e)
        {
            insert_from_excel();
        }

        private void button_search_loại_Click(object sender, EventArgs e)
        {
            if (tb_dotsx_search.Text == "" && cbb_search_loai.Text == "" && cbb_phanbonnvl_search.Text == "" && cbb_ma_BTP_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_date);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 0;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_date_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 1;
                }
            }
            else if (tb_dotsx_search.Text == "" && cbb_search_loai.Text == "" && cbb_phanbonnvl_search.Text == "" && cbb_ma_BTP_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_ma_BTP);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 2;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_ma_BTP_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 3;
                }
            }
            else if (tb_dotsx_search.Text == "" && cbb_search_loai.Text != "" && cbb_phanbonnvl_search.Text == "" && cbb_ma_BTP_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_loai);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 4;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_loai_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 5;
                }
            }
            else if (cbb_phanbonnvl_search.Text != "" && tb_dotsx_search.Text == "" && cbb_search_loai.Text == "" && cbb_ma_BTP_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_phan_bon_nvl);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 6;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_phan_bon_nvl_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 7;
                }
            }
            else if (cbb_phanbonnvl_search.Text == "" && tb_dotsx_search.Text != "" && cbb_search_loai.Text == "" && cbb_ma_BTP_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_dotsx);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 8;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 9;
                }
            }
            else if (tb_dotsx_search.Text != "" && cbb_search_loai.Text != "" && cbb_ma_BTP_search.Text != "" && cbb_phanbonnvl_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_ALL);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 10;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_ALL_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 11;
                }
            }
            else if (tb_dotsx_search.Text != "" && cbb_search_loai.Text != "" && cbb_ma_BTP_search.Text == "" && cbb_phanbonnvl_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_dotsx_loai);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 12;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_dotsx_loai_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 13;
                }
            }
            else if (tb_dotsx_search.Text != "" && cbb_search_loai.Text == "" && cbb_ma_BTP_search.Text != "" && cbb_phanbonnvl_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_dotsx_BTP);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 14;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_dotsx_BTP_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 15;
                }
            }
            else if (tb_dotsx_search.Text != "" && cbb_search_loai.Text == "" && cbb_ma_BTP_search.Text == "" && cbb_phanbonnvl_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_dotsx_NVL);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 16;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_dotsx_NVL_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 17;
                }
            }
            else if (tb_dotsx_search.Text == "" && cbb_search_loai.Text != "" && cbb_ma_BTP_search.Text != "" && cbb_phanbonnvl_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_loai_ma_BTP);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 18;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_loai_ma_BTP_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 19;
                }
            }
            else if (tb_dotsx_search.Text == "" && cbb_search_loai.Text != "" && cbb_ma_BTP_search.Text == "" && cbb_phanbonnvl_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_loai_NVL);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 20;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_loai_NVL_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 21;
                }
            }
            else if (tb_dotsx_search.Text == "" && cbb_search_loai.Text == "" && cbb_ma_BTP_search.Text != "" && cbb_phanbonnvl_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_BTP_NVL);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 22;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_BTP_NVL_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 23;
                }
            }
            else if (tb_dotsx_search.Text != "" && cbb_search_loai.Text != "" && cbb_ma_BTP_search.Text != "" && cbb_phanbonnvl_search.Text == "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_BTP);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 24;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_BTP_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 25;
                }
            }
            else if (tb_dotsx_search.Text != "" && cbb_search_loai.Text != "" && cbb_ma_BTP_search.Text == "" && cbb_phanbonnvl_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_NVL);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 26;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_NVL_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 27;
                }
            }
            else if (tb_dotsx_search.Text == "" && cbb_search_loai.Text != "" && cbb_ma_BTP_search.Text != "" && cbb_phanbonnvl_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_LOAI_BTP_NVL);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 28;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_LOAI_BTP_NVL_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 29;
                }
            }
            else if (tb_dotsx_search.Text != "" && cbb_search_loai.Text == "" && cbb_ma_BTP_search.Text != "" && cbb_phanbonnvl_search.Text != "")
            {
                if (cbb_thietbi_search.Text == "")
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_DOTSX_BTP_NVL);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 30;
                }
                else
                {
                    button_search.Enabled = false;
                    pnloading.Visible = true;
                    ThreadStart threadStart = new ThreadStart(load_data_with_DOTSX_BTP_NVL_S1_02);
                    Thread thread = new Thread(threadStart);
                    thread.Start();
                    thread.IsBackground = true;
                    status_search = 31;
                }
            }
            else
            {
                MessageBox.Show("Tìm kiếm chưa được thiết lập");
            }
        }
        private void panel_nhap_release_Click(object sender, EventArgs e)
        {
            pn_history.BackColor = Color.Silver;
            pn_history.BorderStyle = BorderStyle.FixedSingle;
            lb_history.ForeColor = Color.Black;

            pn_import.BackColor = Color.Silver;
            pn_import.BorderStyle = BorderStyle.FixedSingle;
            lb_import.ForeColor = Color.Black;
            tabControl1.SelectedTab = tabPageNhaprelease;

            panel_nhap_release.BackColor = Color.Lime;
            panel_nhap_release.BorderStyle = BorderStyle.Fixed3D;
            lb_nhap_release.ForeColor = Color.White;

            pn_nksx_button.BackColor = Color.Silver;
            pn_nksx_button.BorderStyle = BorderStyle.FixedSingle;
            lb_nksx.ForeColor = Color.Black;
            //pn_loading_release.Visible = true;
            //ThreadStart threadStart1 = new ThreadStart(load_data_release);
            //Thread thread1 = new Thread(threadStart1);
            //thread1.Start();
            //thread1.IsBackground = true;
            load_data_release();
            pnxuatkhonvl.BackColor = Color.Silver;
            pnxuatkhonvl.BorderStyle = BorderStyle.FixedSingle;
            lb_xuatkhonvl.ForeColor = Color.Black;
        }
        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                /*tb_ngaysx_tabrelease.Text = dataGridView3.SelectedRows[0].Cells[0].Value.ToString();
                tb_dotsx_release.Text = dataGridView3.SelectedRows[0].Cells[1].Value.ToString();
                tb_lot_release.Text = dataGridView3.SelectedRows[0].Cells[2].Value.ToString();
                tb_do_am.Text = dataGridView3.SelectedRows[0].Cells[4].Value.ToString();
                tb_coating.Text = dataGridView3.SelectedRows[0].Cells[5].Value.ToString();
                tb_thoigianondinh.Text = dataGridView3.SelectedRows[0].Cells[6].Value.ToString();
                tb_ngay0.Text = dataGridView3.SelectedRows[0].Cells[7].Value.ToString();
                tb_ngay7.Text = dataGridView3.SelectedRows[0].Cells[8].Value.ToString();
                tb_ngay14.Text = dataGridView3.SelectedRows[0].Cells[9].Value.ToString();
                tb_ngay21.Text = dataGridView3.SelectedRows[0].Cells[10].Value.ToString();
                tb_ngay28.Text = dataGridView3.SelectedRows[0].Cells[11].Value.ToString();
                tb_ngay42.Text = dataGridView3.SelectedRows[0].Cells[12].Value.ToString();
                tb_ngay49.Text = dataGridView3.SelectedRows[0].Cells[13].Value.ToString();
                tb_ngay56.Text = dataGridView3.SelectedRows[0].Cells[14].Value.ToString();
                tb_ngay70.Text = dataGridView3.SelectedRows[0].Cells[15].Value.ToString();
                tb_ngay84.Text = dataGridView3.SelectedRows[0].Cells[16].Value.ToString();
                tb_ngay98.Text = dataGridView3.SelectedRows[0].Cells[17].Value.ToString();
                tb_ngay112.Text = dataGridView3.SelectedRows[0].Cells[18].Value.ToString();
                tb_ngay126.Text = dataGridView3.SelectedRows[0].Cells[19].Value.ToString();
                tb_ngay140.Text = dataGridView3.SelectedRows[0].Cells[20].Value.ToString();
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void E_tong_sp_thu_Click(object sender, EventArgs e)
        {
            try
            {
                E_tong_sp_thu.Text = (Convert.ToDouble(E_sp_dongkhoi.Text) + Convert.ToDouble(E_sp_khong_dong_khoi.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btt_updata_release_Click(object sender, EventArgs e)
        {

        }
        private void pn_nksx_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageblogsx;
            pn_nksx_button.BackColor = Color.Lime;
            pn_nksx_button.BorderStyle = BorderStyle.Fixed3D;
            lb_nksx.ForeColor = Color.White;

            pn_history.BackColor = Color.Silver;
            pn_history.BorderStyle = BorderStyle.FixedSingle;
            lb_history.ForeColor = Color.Black;

            pn_import.BackColor = Color.Silver;
            pn_import.BorderStyle = BorderStyle.FixedSingle;
            lb_import.ForeColor = Color.Black;

            panel_nhap_release.BackColor = Color.Silver;
            panel_nhap_release.BorderStyle = BorderStyle.FixedSingle;
            lb_nhap_release.ForeColor = Color.Black;

            pnxuatkhonvl.BackColor = Color.Silver;
            pnxuatkhonvl.BorderStyle = BorderStyle.FixedSingle;
            lb_xuatkhonvl.ForeColor = Color.Black;
        }
        public void load_data_polymer_fill_textbox()
        {
            try
            {
                /*SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select N1_1,N1_2,N1_3,N1_1_barcode,N1_2_barcode,N1_3_barcode,N1_1_lot,N1_2_lot,N1_3_lot,N2_1,N2_2,N2_1_barcode,N2_2_barcode,N2_1_lot,N2_2_lot,N3_1,N3_1_barcode,N3_1_lot,N1_4_lot,N2_3,N2_3_barcode,N2_3_lot,N3_2,N3_2_barcode,N3_2_lot,N3_3,N3_3_barcode,N3_3_lot,N1_4,N1_4_barcode,N1_4_lot,N2_3,N2_3_barcode,N2_3_lot,N3_2,N3_2_barcode,N3_2_lot,N3_3,N3_3_barcode,N3_3_lot from nhatkysanxuat where LOT = '" + tblot.Text + "' ORDER BY dot_sx DESC ";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                sqlcon.Close();
                DataRow[] row = tb_buff.Select();
                tb_n1_1_kl.Text = row[0]["N1_1"].ToString();
                tb_n1_2_kl.Text = row[0]["N1_2"].ToString();
                tb_n1_3_kl.Text = row[0]["N1_3"].ToString();
                tb_n1_4_kl.Text = row[0]["N1_4"].ToString();
                tb_n1_1_code.Text = row[0]["N1_1_barcode"].ToString();
                tb_n1_2_code.Text = row[0]["N1_2_barcode"].ToString();
                tb_n1_3_code.Text = row[0]["N1_3_barcode"].ToString();
                tb_n1_4_code.Text = row[0]["N1_4_barcode"].ToString();
                tb_n1_1_lot.Text = row[0]["N1_1_lot"].ToString();
                tb_n1_2_lot.Text = row[0]["N1_2_lot"].ToString();
                tb_n1_3_lot.Text = row[0]["N1_3_lot"].ToString();
                tb_n1_4_lot.Text = row[0]["N1_4_lot"].ToString();

                tb_n2_1_kl.Text = row[0]["N2_1"].ToString();
                tb_n2_2_kl.Text = row[0]["N2_2"].ToString();
                tb_n2_3_kl.Text = row[0]["N2_3"].ToString();
                tb_n2_1_code.Text = row[0]["N2_1_barcode"].ToString();
                tb_n2_2_code.Text = row[0]["N2_2_barcode"].ToString();
                tb_n2_3_code.Text = row[0]["N2_3_barcode"].ToString();
                tb_n2_1_lot.Text = row[0]["N2_1_lot"].ToString();
                tb_n2_2_lot.Text = row[0]["N2_2_lot"].ToString();
                tb_n2_3_lot.Text = row[0]["N2_3_lot"].ToString();

                tb_n3_1_kl.Text = row[0]["N3_1"].ToString();
                tb_n3_2_kl.Text = row[0]["N3_2"].ToString();
                tb_n3_3_kl.Text = row[0]["N3_3"].ToString();
                tb_n3_1_code.Text = row[0]["N3_1_barcode"].ToString();
                tb_n3_2_code.Text = row[0]["N3_2_barcode"].ToString();
                tb_n3_3_code.Text = row[0]["N3_3_barcode"].ToString();
                tb_n3_1_lot.Text = row[0]["N3_1_lot"].ToString();
                tb_n3_2_lot.Text = row[0]["N3_2_lot"].ToString();
                tb_n3_3_lot.Text = row[0]["N3_3_lot"].ToString();
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void load_data_release()
        {
            if (tb_timkiem_dotsx_release.Text == "" && cbb_tb_release.Text == "")
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select ngay_sx,dot_sx,LOT,thiet_bi,do_am,coating_layer,thoigian_ondinh,ngay_0,ngay_7,ngay_14,ngay_21,ngay_28,ngay_42,ngay_49,ngay_56,ngay_70,ngay_84,ngay_98,ngay_112,ngay_126,ngay_140 from nhatkysanxuat ORDER BY dot_sx DESC ";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                dataGridView3.DataSource = tb_buff;
                sqlcon.Close();
                pn_loading_release.Visible = false;
            }
            else if (cbb_tb_release.Text != "" && tb_timkiem_dotsx_release.Text == "")
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select ngay_sx,dot_sx,LOT,thiet_bi,do_am,coating_layer,thoigian_ondinh,ngay_0,ngay_7,ngay_14,ngay_21,ngay_28,ngay_42,ngay_49,ngay_56,ngay_70,ngay_84,ngay_98,ngay_112,ngay_126,ngay_140 from nhatkysanxuat where thiet_bi = '" + cbb_tb_release.Text + "' ORDER BY dot_sx DESC ";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                dataGridView3.DataSource = tb_buff;
                sqlcon.Close();
                pn_loading_release.Visible = false;
            }
            else if (cbb_tb_release.Text == "" && tb_timkiem_dotsx_release.Text != "")
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select ngay_sx,dot_sx,LOT,thiet_bi,do_am,coating_layer,thoigian_ondinh,ngay_0,ngay_7,ngay_14,ngay_21,ngay_28,ngay_42,ngay_49,ngay_56,ngay_70,ngay_84,ngay_98,ngay_112,ngay_126,ngay_140 from nhatkysanxuat where dot_sx = '" + tb_timkiem_dotsx_release.Text + "' ORDER BY dot_sx DESC ";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                dataGridView3.DataSource = tb_buff;
                sqlcon.Close();
                pn_loading_release.Visible = false;
            }
            else
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable tb_buff = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "select ngay_sx,dot_sx,LOT,thiet_bi,do_am,coating_layer,thoigian_ondinh,ngay_0,ngay_7,ngay_14,ngay_21,ngay_28,ngay_42,ngay_49,ngay_56,ngay_70,ngay_84,ngay_98,ngay_112,ngay_126,ngay_140 from nhatkysanxuat where thiet_bi = '" + cbb_tb_release.Text + "' AND dot_sx = '" + tb_timkiem_dotsx_release.Text + "' ORDER BY dot_sx DESC ";
                adapter.SelectCommand = command;
                tb_buff.Clear();
                adapter.Fill(tb_buff);
                dataGridView3.DataSource = tb_buff;
                sqlcon.Close();
                pn_loading_release.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load_data_release();
        }
        public void loadcbbma_BTP()
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
            sqlcon.Open();
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            command = sqlcon.CreateCommand();
            command.CommandText = "SELECT DISTINCT ma_BTP from nhatkysanxuat";
            adapter.SelectCommand = command;
            dt.Clear();
            adapter.Fill(dt);
            sqlcon.Close();
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["ma_BTP"].ToString() != "")
                {
                    cbb_ma_BTP_search.Items.Add(dataRow["ma_BTP"].ToString());
                }
            }
        }
        public void loadcbbma_NVL()
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
            sqlcon.Open();
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            command = sqlcon.CreateCommand();
            command.CommandText = "SELECT DISTINCT phanbon_nvl from nhatkysanxuat";
            adapter.SelectCommand = command;
            dt.Clear();
            adapter.Fill(dt);
            sqlcon.Close();
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["phanbon_nvl"].ToString() != "")
                {
                    cbb_phanbonnvl_search.Items.Add(dataRow["phanbon_nvl"].ToString());
                }
            }
        }
        public void loadcbb_Loai()
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
            sqlcon.Open();
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable dt = new DataTable();
            command = sqlcon.CreateCommand();
            command.CommandText = "SELECT DISTINCT loai from nhatkysanxuat";
            adapter.SelectCommand = command;
            dt.Clear();
            adapter.Fill(dt);
            sqlcon.Close();
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["loai"].ToString() != "")
                {
                    cbb_search_loai.Items.Add(dataRow["loai"].ToString());
                }
            }
        }
        private void tbkhoiluongdongkhoi_Leave(object sender, EventArgs e)
        {
            /*double kl_dongkhoi = 0;
            double kl_khongdongkhoi = 0;
            if (tbkhoiluongdongkhoi.Text == "")
            {
                kl_dongkhoi = 0;
            }
            else
            {
                kl_dongkhoi = Convert.ToDouble(tbkhoiluongdongkhoi.Text);
            }
            if (tbspkhongbidongkhoi.Text == "")
            {
                kl_khongdongkhoi = 0;
            }
            else
            {
                kl_khongdongkhoi = Convert.ToDouble(tbspkhongbidongkhoi.Text);
            }
            tbtongklspthuduoc.Text = (kl_dongkhoi + kl_khongdongkhoi).ToString();
            KL_lythuyet();
            hieu_suat_release();
            hieu_suat_thu();
            */
        }

        private void tb_n1_1_kl_Leave(object sender, EventArgs e)
        {
            /*double N1_1 = 0;
            double N1_2 = 0;
            double N1_3 = 0;
            double N1_4 = 0;
            if (tb_n1_1_kl.Text == "")
            {
                N1_1 = 0;
            }
            else
            {
                N1_1 = Convert.ToDouble(tb_n1_1_kl.Text);
            }
            if (tb_n1_2_kl.Text == "")
            {
                N1_2 = 0;
            }
            else
            {
                N1_2 = Convert.ToDouble(tb_n1_2_kl.Text);
            }
            if (tb_n1_3_kl.Text == "")
            {
                N1_3 = 0;
            }
            else
            {
                N1_3 = Convert.ToDouble(tb_n1_3_kl.Text);
            }
            if (tb_n1_4_kl.Text == "")
            {
                N1_4 = 0;
            }
            else
            {
                N1_4 = Convert.ToDouble(tb_n1_4_kl.Text);
            }
            tbn1157.Text = (N1_1 + N1_2 + N1_3 + N1_4).ToString();
            */
        }
        private void tb_n2_1_kl_Leave(object sender, EventArgs e)
        {
            /*double N2_1 = 0;
            double N2_2 = 0;
            double N2_3 = 0;
            if (tb_n2_1_kl.Text == "")
            {
                N2_1 = 0;
            }
            else
            {
                N2_1 = Convert.ToDouble(tb_n2_1_kl.Text);
            }
            if (tb_n2_2_kl.Text == "")
            {
                N2_2 = 0;
            }
            else
            {
                N2_2 = Convert.ToDouble(tb_n2_2_kl.Text);
            }
            if (tb_n2_3_kl.Text == "")
            {
                N2_3 = 0;
            }
            else
            {
                N2_3 = Convert.ToDouble(tb_n2_3_kl.Text);
            }
            tbn221.Text = (N2_1 + N2_2 + N2_3).ToString();
            */
        }
        private void tb_n3_1_kl_Leave(object sender, EventArgs e)
        {
            /*double N3_1 = 0;
            double N3_2 = 0;
            double N3_3 = 0;
            if (tb_n3_1_kl.Text == "")
            {
                N3_1 = 0;
            }
            else
            {
                N3_1 = Convert.ToDouble(tb_n3_1_kl.Text);
            }
            if (tb_n3_2_kl.Text == "")
            {
                N3_2 = 0;
            }
            else
            {
                N3_2 = Convert.ToDouble(tb_n3_2_kl.Text);
            }
            if (tb_n3_3_kl.Text == "")
            {
                N3_3 = 0;
            }
            else
            {
                N3_3 = Convert.ToDouble(tb_n3_3_kl.Text);
            }
            tbn3190.Text = (N3_1 + N3_2 + N3_3).ToString();
            */
        }
        public void KL_lythuyet()
        {
            /*try
            {
                tbkhoiluonglythuyet.Text = (Convert.ToDouble(tbkhoiluongphanbonnvl.Text) + (Convert.ToDouble(tbn1157.Text) + Convert.ToDouble(tbn221.Text) + Convert.ToDouble(tbn3190.Text)) / 4).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            */
        }
        public void hieu_suat_thu()
        {
            /*try
            {
                tbhieusuatthu.Text = Math.Round(((Convert.ToDouble(tbtongklspthuduoc.Text) / Convert.ToDouble(tbkhoiluonglythuyet.Text)) * 100), 4).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            */
        }
        public void hieu_suat_release()
        {
            /*try
            {
                if (tbloai.Text == "WA" || tbloai.Text == "A")
                {
                    tbhieusuatrelease.Text = Math.Round((Convert.ToDouble(tbspkhongbidongkhoi.Text) / Convert.ToDouble(tbtongklspthuduoc.Text)) * 100, 4).ToString();
                }
                else
                {
                    tbhieusuatrelease.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            */
        }
        private void tbkhoiluonglythuyet_Click(object sender, EventArgs e)
        {
            KL_lythuyet();
        }

        private void tbhieusuatthu_Click(object sender, EventArgs e)
        {
            hieu_suat_thu();
        }

        private void tbhieusuatrelease_Click(object sender, EventArgs e)
        {
            hieu_suat_release();
        }
        public void convert_polymer()
        {
            /*double cl1 = 0;
            double cl2 = 0;
            double cl3 = 0;
            double n1 = 0;
            double n2 = 0;
            double n3 = 0;
            cl1 = Convert.ToDouble(tblot.Text.Substring(3, 2)) / 10;
            cl2 = Convert.ToDouble(tblot.Text.Substring(5, 2)) / 10;
            cl3 = Convert.ToDouble(tblot.Text.Substring(7, 2)) / 10;
            n1 = cl1 / 100 * (Convert.ToDouble(tbkhoiluongphanbonnvl.Text)) * 4;
            n2 = cl2 / 100 * (Convert.ToDouble(tbkhoiluongphanbonnvl.Text)) * 4;
            n3 = cl3 / 100 * (Convert.ToDouble(tbkhoiluongphanbonnvl.Text)) * 4;
            tbn1157.Text = n1.ToString();
            tbn221.Text = n2.ToString();
            tbn3190.Text = n3.ToString();
            */
        }

        private void pnxuatkhonvl_Click(object sender, EventArgs e)
        {
            pn_nksx_button.BackColor = Color.Silver;
            pn_nksx_button.BorderStyle = BorderStyle.FixedSingle;
            lb_nksx.ForeColor = Color.Black;

            pn_history.BackColor = Color.Silver;
            pn_history.BorderStyle = BorderStyle.FixedSingle;
            lb_history.ForeColor = Color.Black;

            pn_import.BackColor = Color.Silver;
            pn_import.BorderStyle = BorderStyle.FixedSingle;
            lb_import.ForeColor = Color.Black;

            panel_nhap_release.BackColor = Color.Silver;
            panel_nhap_release.BorderStyle = BorderStyle.FixedSingle;
            lb_nhap_release.ForeColor = Color.Black;

            pnxuatkhonvl.BackColor = Color.Lime;
            pnxuatkhonvl.BorderStyle = BorderStyle.Fixed3D;
            lb_xuatkhonvl.ForeColor = Color.White;
            tabControl1.SelectedTab = tabPage_xuatkho;
            load_data_xuatkho();
        }
        public void load_data_xuatkho()
        {
            try
            {
                if (cbb_search_tb_xuatkho.Text == "" && tb_search_dotsx_xuatkho.Text == "")
                {
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand command = new SqlCommand();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable tb_buff = new DataTable();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "select dot_sx,thiet_bi,ngay_sx,LOT,N1,N1_1,N1_1_barcode,N1_1_lot,N1_2,N1_2_barcode,N1_2_lot,N1_3,N1_3_barcode,N1_3_lot,N2,N2_1," +
                        "N2_1_barcode,N2_1_lot,N2_2,N2_2_barcode,N2_2_lot,N2_3,N2_3_barcode,N2_3_lot,N3,N3_1,N3_1_barcode,N3_1_lot,N3_2,N3_2_barcode,N3_2_lot," +
                        "N3_3,N3_3_barcode,N3_3_lot from nhatkysanxuat ORDER BY dot_sx DESC ";
                    adapter.SelectCommand = command;
                    tb_buff.Clear();
                    adapter.Fill(tb_buff);
                    dgv_xuatkhonvl.DataSource = tb_buff;
                    sqlcon.Close();
                }
                else if (cbb_search_tb_xuatkho.Text != "" && tb_search_dotsx_xuatkho.Text == "")
                {
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand command = new SqlCommand();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable tb_buff = new DataTable();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "select dot_sx,thiet_bi,ngay_sx,LOT,N1,N1_1,N1_1_barcode,N1_1_lot,N1_2,N1_2_barcode,N1_2_lot,N1_3,N1_3_barcode,N1_3_lot,N2,N2_1," +
                        "N2_1_barcode,N2_1_lot,N2_2,N2_2_barcode,N2_2_lot,N2_3,N2_3_barcode,N2_3_lot,N3,N3_1,N3_1_barcode,N3_1_lot,N3_2,N3_2_barcode,N3_2_lot," +
                        "N3_3,N3_3_barcode,N3_3_lot from nhatkysanxuat where thiet_bi ='" + cbb_search_tb_xuatkho.Text + "' ORDER BY dot_sx DESC ";
                    adapter.SelectCommand = command;
                    tb_buff.Clear();
                    adapter.Fill(tb_buff);
                    dgv_xuatkhonvl.DataSource = tb_buff;
                    sqlcon.Close();
                }
                else
                {
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand command = new SqlCommand();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable tb_buff = new DataTable();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "select dot_sx,thiet_bi,ngay_sx,LOT,N1,N1_1,N1_1_barcode,N1_1_lot,N1_2,N1_2_barcode,N1_2_lot,N1_3,N1_3_barcode,N1_3_lot,N2,N2_1," +
                        "N2_1_barcode,N2_1_lot,N2_2,N2_2_barcode,N2_2_lot,N2_3,N2_3_barcode,N2_3_lot,N3,N3_1,N3_1_barcode,N3_1_lot,N3_2,N3_2_barcode,N3_2_lot," +
                        "N3_3,N3_3_barcode,N3_3_lot from nhatkysanxuat where dot_sx ='" + tb_search_dotsx_xuatkho.Text + "' ORDER BY dot_sx DESC ";
                    adapter.SelectCommand = command;
                    tb_buff.Clear();
                    adapter.Fill(tb_buff);
                    dgv_xuatkhonvl.DataSource = tb_buff;
                    sqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button_search_xuatkho_Click(object sender, EventArgs e)
        {
            load_data_xuatkho();
        }

        private void tb_n1_1_code_Leave(object sender, EventArgs e)
        {
            /*if (tb_n1_1_code.Text != "")
            {
                tbbarcodeN1.Text += tb_n1_1_code.Text + ", ";
            }
            else if (tb_n1_2_code.Text != "")
            {
                tbbarcodeN1.Text += tb_n1_2_code.Text + ", ";
            }
            */
        }
        List<int> old_index = new List<int>();
        private void hideStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int index_column = dataGridView1.CurrentCell.OwningColumn.Index;
                dataGridView1.Columns[index_column].Visible = false;
                old_index.Add(index_column);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < old_index.Count; i++)
                {
                    dataGridView1.Columns[old_index[i]].Visible = true;
                }
                old_index.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lot = dataGridView1.CurrentCell.Value.ToString();
            if (lbuser.Text == "admin")
            {
                if (lot == "")
                {
                    MessageBox.Show("Chưa chọn đối tượng cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        DialogResult dialogResult;
                        dialogResult = MessageBox.Show("Bạn có muốn xóa LOT : '"+lot+"'?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (dialogResult == DialogResult.OK)
                        {
                            SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                            sqlcon.Open();
                            string query_delete = "delete from nhatkysanxuat where LOT ='" + lot + "'";
                            SqlCommand cmd = new SqlCommand(query_delete, sqlcon);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Xóa Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            insert_blogtruycap("Đã xóa LOT : " + lot);
                            sqlcon.Close();
                            if (status_search == 0)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_date);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 1)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_date_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 2)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_ma_BTP);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 3)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_ma_BTP_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 4)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_loai);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 5)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_loai_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 6)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_phan_bon_nvl);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 7)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_phan_bon_nvl_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 8)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_dotsx);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 9)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 10)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_ALL);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 11)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_ALL_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 12)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_dotsx_loai);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 13)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_dotsx_loai_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 14)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_dotsx_BTP);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 15)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_dotsx_BTP_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 16)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_dotsx_NVL);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 17)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_dotsx_NVL_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 18)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_loai_ma_BTP);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 19)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_loai_ma_BTP_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 21)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_loai_NVL);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 21)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_loai_NVL_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 22)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_BTP_NVL);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 23)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_BTP_NVL_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 24)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_BTP);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 25)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_BTP_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 26)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_NVL);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 27)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_dotsx_loai_NVL_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 28)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_LOAI_BTP_NVL);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 29)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_LOAI_BTP_NVL_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 30)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_DOTSX_BTP_NVL);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                            else if (status_search == 31)
                            {
                                button_search.Enabled = false;
                                pnloading.Visible = true;
                                ThreadStart threadStart = new ThreadStart(load_data_with_DOTSX_BTP_NVL_S1_02);
                                Thread thread = new Thread(threadStart);
                                thread.Start();
                                thread.IsBackground = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Không được xóa, liên hệ Chương để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try
                {
                    insert_blogtruycap("Đang cố xóa LOT : " + lot);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
