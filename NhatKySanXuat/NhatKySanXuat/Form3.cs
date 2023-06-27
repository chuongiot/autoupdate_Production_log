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
    public partial class Form3 : Form
    {
        public Form3(string LOT_DATA)
        {
            InitializeComponent();
            tblot.Text = LOT_DATA;
        }
        bool update_or_add;
        private void Form3_Load(object sender, EventArgs e)
        {
            load_data(tblot.Text);
            loadcbb_LOT();
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Tahoma", 8, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
        }
        public void insert_data()
        {
            if (tblot.Text == "")
            {
                MessageBox.Show("Chưa Nhập LOT", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string Nguoi_nhap = cbbnguoinhap.Text;
                string LOT = tblot.Text;
                string Dotsx = tbdotsx.Text;
                string Ngaysx = dateTimePickerngaysx.Text;
                string Thietbi = cbbthietbi.Text;
                string Mabtp = cbmaBTP.Text;
                string Tenbtp = tbtenbtp.Text;
                string Me = tbme.Text;
                string Tocdo_release = tbtocdorelease.Text;
                string Ngayrelease = tbngay_release.Text;
                string Loai = tbloai.Text;
                string Tongklsp_thuduoc = tbtongklspthuduoc.Text;
                string Kldongkhoi = tbkhoiluongdongkhoi.Text;
                string Khongdongkhoi = tbspkhongbidongkhoi.Text;
                string Kl_lythuyet = tbkhoiluonglythuyet.Text;
                string Hieusuatthu = tbhieusuatthu.Text;
                string Hieusuatrelease = tbhieusuatrelease.Text;
                string Thoigiancb = tbthoigiancb.Text;
                string Thoigiansx = tbthoigiansx.Text;
                string Phanbon_nvl = cbbphanbonnvl.Text;
                string KL_phan_nvl = tbkhoiluongphanbonnvl.Text;
                string Barcode_nvl = tbbarcodephanbonvl.Text;
                string LOT_nvl = tbLOTphanbonnvl.Text;
                string N1_khoiluong = tbn1157.Text;
                string N1_barcode = tbbarcodeN1.Text;
                string N1_LOT = tbLOTN1.Text;
                string N2_khoiluong = tbn221.Text;
                string N2_barcode = tbbarcodeN2.Text;
                string N2_LOT = tbLOTN2.Text;
                string n3_khoiluong = tbn3190.Text;
                string N3_barcode = tbbarcodeN3.Text;
                string N3_LOT = tbLOTN3.Text;

                string phan_nvl1 = comboBox_nvl_1.Text;
                string kl_nvl_1 = textBox_kl_nvl_1.Text;
                string barcode_nvl1 = textBox_barcode_nvl_1.Text;
                string lot_nvl1 = textBox_lot_nvl_1.Text;
                string phan_nvl2 = comboBox_nvl_2.Text;
                string kl_nvl_2 = textBox_kl_nvl_2.Text;
                string barcode_nvl2 = textBox_barcode_nvl_2.Text;
                string lot_nvl2 = textBox_lot_nvl_2.Text;



                string N1_1_kl = tb_n1_1_kl.Text;
                string N1_2_kl = tb_n1_2_kl.Text;
                string N1_3_kl = tb_n1_3_kl.Text;
                string N1_4_kl = tb_n1_4_kl.Text;
                string N2_1_kl = tb_n2_1_kl.Text;
                string N2_2_kl = tb_n2_2_kl.Text;
                string N2_3_kl = tb_n2_3_kl.Text;
                string N3_1_kl = tb_n3_1_kl.Text;
                string N3_2_kl = tb_n3_2_kl.Text;
                string N3_3_kl = tb_n3_3_kl.Text;

                string N1_1_code = tb_n1_1_code.Text;
                string N1_2_code = tb_n1_2_code.Text;
                string N1_3_code = tb_n1_3_code.Text;
                string N1_4_code = tb_n1_4_code.Text;
                string N2_1_code = tb_n2_1_code.Text;
                string N2_2_code = tb_n2_2_code.Text;
                string N2_3_code = tb_n2_3_code.Text;
                string N3_1_code = tb_n3_1_code.Text;
                string N3_2_code = tb_n3_2_code.Text;
                string N3_3_code = tb_n3_3_code.Text;

                string N1_1_lot = tb_n1_1_lot.Text;
                string N1_2_lot = tb_n1_2_lot.Text;
                string N1_3_lot = tb_n1_3_lot.Text;
                string N1_4_lot = tb_n1_4_lot.Text;
                string N2_1_lot = tb_n2_1_lot.Text;
                string N2_2_lot = tb_n2_2_lot.Text;
                string N2_3_lot = tb_n2_3_lot.Text;
                string N3_1_lot = tb_n3_1_lot.Text;
                string N3_2_lot = tb_n3_2_lot.Text;
                string N3_3_lot = tb_n3_3_lot.Text;


                string N1_BD = "0";
                string N1_KT = "0";
                string N2_BD = "0";
                string N2_KT = "0";
                string N3_BD = "0";
                string N3_KT = "0";
                if (dataGridView1.Rows.Count > 0)
                {
                    N1_BD = dataGridView1.Rows[0].Cells[1].Value.ToString();
                    N1_KT = dataGridView1.Rows[0].Cells[2].Value.ToString();
                    N2_BD = dataGridView1.Rows[1].Cells[1].Value.ToString();
                    N2_KT = dataGridView1.Rows[1].Cells[2].Value.ToString();
                    N3_BD = dataGridView1.Rows[2].Cells[1].Value.ToString();
                    N3_KT = dataGridView1.Rows[2].Cells[2].Value.ToString();
                }

                string GA3 = tbga3.Text;
                string GA3_barcode = tbbarcodeGA3.Text;
                string Borax = tbborax.Text;
                string Borax_barcode = tbbarcodeBorax.Text;
                string NAA = tbnaa.Text;
                string NAA_barcode = tbbarcodeNAA.Text;
                string Sodium = tbsodium.Text;
                string Sodium_barcode = tbbarcodeSodium.Text;
                string Citric = tbcitric.Text;
                string Barcode_Citric = tbbarcode_citric.Text;
                string Naoh = tbnaoh.Text;
                string Barcode_Naoh = tbbarcode_naoh.Text;
                string Solubo = tbsolubo.Text;
                string Barcode_Solubo = tbbarcode_solubo.Text;
                string Edtazn = tbEDTA.Text;
                string Barcode_Edta = tbbarcode_edta.Text;
                string Red = tbred.Text;
                string Barcode_red = tbbarcode_red.Text;
                string Violet = tbviolet.Text;
                string Barcode_violet = tbbarcode_violet.Text;
                string Blue = tbblue.Text;
                string Barcode_blue = tbbarcode_blue.Text;
                string Yellow = tbyellow.Text;
                string Barcode_yellow = tbbarcode_yellow.Text;
                string Black = tbblack.Text;
                string Barcode_black = tbbarcode_black.Text;
                string Prev = tbPREV.Text;
                string Barcode_Prev = tbbarcode_prev.Text;
                string Than_cam = tbsoluongthancam.Text;
                string Dien = tbkwdien.Text;
                string Nuoc_RO = tbm3nuocRO.Text;
                string Nuoc_thuycuc = tbm3nuocthuycuc.Text;
                string BHLD = tbbaoholaodong.Text;
                string Ghi_chu = tbghi_chu.Text;
                string Vitri_spthuduoc = tbvitri_tongklsp_thuduoc.Text;
                string Vitri_spdongkhoi = tbvitri_spdongkhoi.Text;
                string Vitri_spkhongdongkhoi = tbvitri_spkhongdongkhoi.Text;


                string do_am = tb_do_am.Text;
                string coating = tb_coating.Text;
                string thoi_gian = tb_thoigianondinh.Text;
                string ngay_0 = tb_ngay0.Text;
                string ngay_7 = tb_ngay7.Text;
                string ngay_14 = tb_ngay14.Text;
                string ngay_21 = tb_ngay21.Text;
                string ngay_28 = tb_ngay28.Text;
                string ngay_42 = tb_ngay42.Text;
                string ngay_49 = tb_ngay49.Text;
                string ngay_56 = tb_ngay56.Text;
                string ngay_70 = tb_ngay70.Text;
                string ngay_84 = tb_ngay84.Text;
                string ngay_98 = tb_ngay98.Text;
                string ngay_112 = tb_ngay112.Text;
                string ngay_126 = tb_ngay126.Text;
                string ngay_140 = tb_ngay140.Text;
                try
                {
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    SqlCommand command = new SqlCommand();
                    sqlcon.Open();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "insert into nhatkysanxuat (name,dot_sx,ngay_sx,thiet_bi,ma_BTP,ten_BTP,me,LOT ,tocdo_release," +
                        "ngay_release,loai,tong_klspsx,kl_dongkhoi,kl_khongdongkhoi,kl_lythuyet,hieuxuat_thu,hieuxuat_release," +
                        "thoigian_cb,thoigian_sx,phanbon_nvl,kl_nvl,barcode_nvl,lot_nvl,N1,barcode_n1,lot_n1," +
                        "N2,barcode_n2,lot_n2,N3,barcode_n3,lot_n3,Ga3,barcode_ga3,Borax,bacode_borax,Naa,barcode_naa,solubo,barocde_solubo," +
                        "Edta,barcode_edta,Red,barcode_red,violet,barcode_violet,blue,barocde_blue,yellow,barcode_yellow,black,barcode_back,prev," +
                        "barcode_prev,thancam,dien,nuocRO,nuocthuycuc,BHLD,Sodium,barcode_sodium,Citric,barcode_citric,Naoh,barocde_naoh,ghi_chu," +
                        "vitri_spthuduoc,vitri_spdongkhoi,vitri_spkhongdongkhoi,N1_1,N1_2,N1_3,N1_1_barcode,N1_2_barcode,N1_3_barcode,N1_1_lot," +
                        "N1_2_lot,N1_3_lot,N2_1,N2_2,N2_1_barcode,N2_2_barcode,N2_1_lot,N2_2_lot,N3_1,N3_1_barcode,N3_1_lot,N1_4,N1_4_barcode," +
                        "N1_4_lot,N2_3,N2_3_barcode,N2_3_lot,N3_2,N3_2_barcode,N3_2_lot,N3_3,N3_3_barcode,N3_3_lot,thoigian_ondinh,do_am,coating_layer," +
                        "ngay_0,ngay_7,ngay_14,ngay_21,ngay_28,ngay_42,ngay_49,ngay_56,ngay_70,ngay_84,ngay_98,ngay_112,ngay_126,ngay_140,NVL_1," +
                        "barcode_NVL_1,lot_NVL_1,NVL_2,barcode_NVL_2,lot_NVL_2,KL_NVL_1,KL_NVL_2,N1_BD,N1_KT,N2_BD,N2_KT,N3_BD,N3_KT)" +
                        "values (N'" + Nguoi_nhap + "','" + Dotsx + "','" + Ngaysx + "','" + Thietbi + "','" + Mabtp + "','" + Tenbtp + "','" + Me + "','" + LOT + "','" + Tocdo_release + "'," +
                        "'" + Ngayrelease + "','" + Loai + "','" + Tongklsp_thuduoc + "','" + Kldongkhoi + "','" + Khongdongkhoi + "','" + Kl_lythuyet + "','" + Hieusuatthu + "'," +
                        "'" + Hieusuatrelease + "','" + Thoigiancb + "','" + Thoigiansx + "','" + Phanbon_nvl + "','" + KL_phan_nvl + "','" + Barcode_nvl + "','" + LOT_nvl + "'," +
                        "'" + N1_khoiluong + "','" + N1_barcode + "','" + N1_LOT + "','" + N2_khoiluong + "','" + N2_barcode + "','" + N2_LOT + "','" + n3_khoiluong + "','" + N3_barcode + "','" + N3_LOT + "'," +
                        "'" + GA3 + "','" + GA3_barcode + "','" + Borax + "','" + Borax_barcode + "'," +
                        "'" + NAA + "','" + NAA_barcode + "','" + Solubo + "','" + Barcode_Solubo + "','" + Edtazn + "','" + Barcode_Edta + "','" + Red + "','" + Barcode_red + "'," +
                        "'" + Violet + "','" + Barcode_violet + "','" + Blue + "','" + Barcode_blue + "','" + Yellow + "','" + Barcode_yellow + "','" + Black + "','" + Barcode_black + "'," +
                        "'" + Prev + "','" + Barcode_Prev + "','" + Than_cam + "','" + Dien + "','" + Nuoc_RO + "','" + Nuoc_thuycuc + "','" + BHLD + "','" + Sodium + "','" + Sodium_barcode + "','" + Citric + "'," +
                        "'" + Barcode_Citric + "','" + Naoh + "','" + Barcode_Naoh + "',N'" + Ghi_chu + "','" + Vitri_spthuduoc + "','" + Vitri_spdongkhoi + "','" + Vitri_spkhongdongkhoi + "'," +
                        "'" + N1_1_kl + "','" + N1_2_kl + "','" + N1_3_kl + "','" + N1_1_code + "','" + N1_2_code + "','" + N1_3_code + "','" + N1_1_lot + "','" + N1_2_lot + "','" + N1_3_lot + "'," +
                        "'" + N2_1_kl + "','" + N2_2_kl + "','" + N2_1_code + "','" + N2_2_code + "','" + N2_1_lot + "','" + N2_2_lot + "','" + N3_1_kl + "','" + N3_1_code + "','" + N3_1_lot + "'," +
                        "'" + N1_4_kl + "','" + N1_4_code + "','" + N1_4_lot + "','" + N2_3_kl + "','" + N2_3_code + "','" + N2_3_lot + "','" + N3_2_kl + "','" + N3_2_code + "','" + N3_2_lot + "'," +
                        "'" + N3_3_kl + "','" + N3_3_code + "','" + N3_3_lot + "','" + thoi_gian + "','" + do_am + "','" + coating + "','" + ngay_0 + "','" + ngay_7 + "','" + ngay_14 + "'," +
                        "'" + ngay_21 + "','" + ngay_28 + "','" + ngay_42 + "','" + ngay_49 + "','" + ngay_56 + "','" + ngay_70 + "','" + ngay_84 + "','" + ngay_98 + "'," +
                        "'" + ngay_112 + "','" + ngay_126 + "','" + ngay_140 + "','" + phan_nvl1 + "','" + barcode_nvl1 + "','" + lot_nvl1 + "','" + phan_nvl2 + "','" + barcode_nvl2 + "','" + lot_nvl2 + "'," +
                        "'" + kl_nvl_1 + "','" + kl_nvl_2 + "','" + N1_BD + "','" + N1_KT + "','" + N2_BD + "','" + N2_KT + "','" + N3_BD + "','" + N3_KT + "')";
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm Thành Công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    insert_blogtruycap("Đã thêm LOT : " + tblot.Text);
                    sqlcon.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void update()
        {
            if (tblot.Text == "")
            {
                MessageBox.Show("Chưa Nhập Thông Tin", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string N1_BD = "0";
                string N1_KT = "0";
                string N2_BD = "0";
                string N2_KT = "0";
                string N3_BD = "0";
                string N3_KT = "0";
                if (dataGridView1.Rows.Count > 0)
                {
                    N1_BD = dataGridView1.Rows[0].Cells[1].Value.ToString();
                    N1_KT = dataGridView1.Rows[0].Cells[2].Value.ToString();
                    N2_BD = dataGridView1.Rows[1].Cells[1].Value.ToString();
                    N2_KT = dataGridView1.Rows[1].Cells[2].Value.ToString();
                    N3_BD = dataGridView1.Rows[2].Cells[1].Value.ToString();
                    N3_KT = dataGridView1.Rows[2].Cells[2].Value.ToString();
                }
                try
                {
                    string sqlupdate = "update nhatkysanxuat set thoigian_ondinh = '" + tb_thoigianondinh.Text + "',do_am = '" + tb_do_am.Text + "',coating_layer = '" + tb_coating.Text + "'," +
                        "ngay_0 = '" + tb_ngay0.Text + "',ngay_7 = '" + tb_ngay7.Text + "',ngay_14 = '" + tb_ngay14.Text + "',ngay_21 = '" + tb_ngay21.Text + "'," +
                        "ngay_28 = '" + tb_ngay28.Text + "',ngay_42 = '" + tb_ngay42.Text + "',ngay_49 = '" + tb_ngay49.Text + "',ngay_56 = '" + tb_ngay56.Text + "'," +
                        "ngay_70 = '" + tb_ngay70.Text + "',ngay_84 = '" + tb_ngay84.Text + "',ngay_98 = '" + tb_ngay98.Text + "',ngay_112 = '" + tb_ngay112.Text + "'," +
                        "ngay_126 = '" + tb_ngay126.Text + "',ngay_140 = '" + tb_ngay140.Text + "',dot_sx = '" + tbdotsx.Text + "',ngay_sx = '" + dateTimePickerngaysx.Text + "'," +
                        "thiet_bi = '" + cbbthietbi.Text + "',ma_BTP='" + cbmaBTP.Text + "',ten_BTP='" + tbtenbtp.Text + "',me='" + tbme.Text + "'," +
                        "tocdo_release='" + tbtocdorelease.Text + "',ngay_release='" + tbngay_release.Text + "',loai='" + tbloai.Text + "'," +
                        "name=N'" + cbbnguoinhap.Text + "',tong_klspsx='" + tbtongklspthuduoc.Text + "'," +
                        "kl_dongkhoi='" + tbkhoiluongdongkhoi.Text + "',kl_khongdongkhoi='" + tbspkhongbidongkhoi.Text + "',kl_lythuyet='" + tbkhoiluonglythuyet.Text + "'," +
                        "hieuxuat_thu='" + tbhieusuatthu.Text + "',hieuxuat_release='" + tbhieusuatrelease.Text + "',thoigian_cb='" + tbthoigiancb.Text + "'," +
                        "thoigian_sx='" + tbthoigiansx.Text + "',phanbon_nvl='" + cbbphanbonnvl.Text + "'," +
                        "kl_nvl='" + tbkhoiluongphanbonnvl.Text + "',barcode_nvl='" + tbbarcodephanbonvl.Text + "',lot_nvl='" + tbLOTphanbonnvl.Text + "'," +
                        "N1='" + tbn1157.Text + "',barcode_n1='" + tbbarcodeN1.Text + "',lot_n1='" + tbLOTN1.Text + "',N2='" + tbn221.Text + "'," +
                        "barcode_n2='" + tbbarcodeN2.Text + "',lot_n2='" + tbLOTN2.Text + "',N3='" + tbn3190.Text + "',barcode_n3='" + tbbarcodeN3.Text + "'," +
                        "lot_n3='" + tbLOTN3.Text + "',Ga3='" + tbga3.Text + "',barcode_ga3='" + tbbarcodeGA3.Text + "',Borax='" + tbborax.Text + "'," +
                        "bacode_borax='" + tbbarcodeBorax.Text + "',Naa='" + tbnaa.Text + "',barcode_naa='" + tbbarcodeNAA.Text + "',solubo='" + tbsolubo.Text + "'," +
                        "barocde_solubo='" + tbbarcode_solubo.Text + "',Edta='" + tbEDTA.Text + "',barcode_edta='" + tbbarcode_edta.Text + "',Red='" + tbred.Text + "'," +
                        "barcode_red='" + tbbarcode_red.Text + "',violet='" + tbviolet.Text + "',barcode_violet='" + tbbarcode_violet.Text + "',blue='" + tbblue.Text + "'," +
                        "barocde_blue='" + tbbarcode_blue.Text + "',yellow='" + tbyellow.Text + "',barcode_yellow='" + tbbarcode_yellow.Text + "',black='" + tbblack.Text + "'," +
                        "barcode_back='" + tbbarcode_black.Text + "',prev='" + tbPREV.Text + "',barcode_prev='" + tbbarcode_prev.Text + "',thancam='" + tbsoluongthancam.Text + "'," +
                        "dien='" + tbkwdien.Text + "',nuocRo='" + tbm3nuocRO.Text + "',nuocthuycuc='" + tbm3nuocthuycuc.Text + "',BHLD='" + tbbaoholaodong.Text + "'," +
                        "Sodium='" + tbsodium.Text + "',barcode_sodium='" + tbbarcodeSodium.Text + "',Citric='" + tbcitric.Text + "',barcode_citric='" + tbbarcode_citric.Text + "'," +
                        "Naoh='" + tbnaoh.Text + "',barocde_naoh='" + tbbarcode_naoh.Text + "',ghi_chu=N'" + tbghi_chu.Text + "',vitri_spthuduoc='" + tbvitri_tongklsp_thuduoc.Text + "'," +
                        "vitri_spdongkhoi = '" + tbvitri_spdongkhoi.Text + "',vitri_spkhongdongkhoi='" + tbvitri_spkhongdongkhoi.Text + "'," +
                        "N1_1='" + tb_n1_1_kl.Text + "',N1_2='" + tb_n1_2_kl.Text + "',N1_3='" + tb_n1_3_kl.Text + "',N1_1_barcode='" + tb_n1_1_code.Text + "'," +
                        "N1_2_barcode='" + tb_n1_2_code.Text + "',N1_3_barcode='" + tb_n1_3_code.Text + "',N1_1_lot='" + tb_n1_1_lot.Text + "',N1_2_lot='" + tb_n1_2_lot.Text + "',N1_3_lot='" + tb_n1_3_lot.Text + "'," +
                        "N2_1='" + tb_n2_1_kl.Text + "',N2_2='" + tb_n2_2_kl.Text + "',N2_1_barcode='" + tb_n2_1_code.Text + "',N2_2_barcode='" + tb_n2_2_code.Text + "'," +
                        "N2_1_lot='" + tb_n2_1_lot.Text + "',N2_2_lot='" + tb_n2_2_lot.Text + "',N3_1='" + tb_n3_1_kl.Text + "',N3_1_barcode='" + tb_n3_1_code.Text + "',N3_1_lot='" + tb_n3_1_lot.Text + "'," +
                        "N1_4='" + tb_n1_4_kl.Text + "',N1_4_barcode='" + tb_n1_4_code.Text + "',N1_4_lot='" + tb_n1_4_lot.Text + "',N2_3='" + tb_n2_3_kl.Text + "'," +
                        "N2_3_barcode='" + tb_n2_3_code.Text + "',N2_3_lot='" + tb_n2_3_lot.Text + "',N3_2='" + tb_n3_2_kl.Text + "',N3_2_barcode='" + tb_n3_2_code.Text + "'," +
                        "N3_2_lot='" + tb_n3_2_lot.Text + "',N3_3='" + tb_n3_3_kl.Text + "',N3_3_barcode='" + tb_n3_3_code.Text + "',N3_3_lot='" + tb_n3_3_lot.Text + "'," +
                        "TG_BD='" + dateTimePicker_TG_BD.Text + "',TG_KT='" + dateTimePicker_TG_KT.Text + "',NVL_1='" + comboBox_nvl_1.Text + "',barcode_NVL_1='" + textBox_barcode_nvl_1.Text + "'," +
                        "lot_NVL_1='" + textBox_lot_nvl_1.Text + "',NVL_2='" + comboBox_nvl_2.Text + "',barcode_NVL_2='" + textBox_barcode_nvl_2.Text + "',lot_NVL_2='" + textBox_lot_nvl_2.Text + "'," +
                        "KL_NVL_1='" + textBox_kl_nvl_1.Text + "',KL_NVL_2='" + textBox_kl_nvl_2.Text + "',N1_BD='" + N1_BD + "',N1_KT='" + N1_KT + "'," +
                        "N2_BD='" + N2_BD + "',N2_KT='" + N2_KT + "',N3_BD='" + N3_BD + "',N3_KT='" + N3_KT + "' where LOT ='" + tblot.Text + "'";
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand cmd = new SqlCommand(sqlupdate, sqlcon);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập Nhật Thành Công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    insert_blogtruycap("Đã cập nhật LOT : " + tblot.Text);
                    sqlcon.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void load_data(string LOT)
        {
            cleardata();
            if (LOT != "")
            {
                try
                {
                    update_or_add = true;
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand command = new SqlCommand();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable tb_buff = new DataTable();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "select * from nhatkysanxuat where LOT = '" + tblot.Text + "'";
                    adapter.SelectCommand = command;
                    tb_buff.Clear();
                    adapter.Fill(tb_buff);
                    sqlcon.Close();
                    DataRow[] row = tb_buff.Select();
                    cbbnguoinhap.Text = row[0]["name"].ToString();
                    tbdotsx.Text = row[0]["dot_sx"].ToString();
                    dateTimePickerngaysx.Text = row[0]["ngay_sx"].ToString();
                    dateTimePicker_TG_BD.Text = row[0]["TG_BD"].ToString();
                    dateTimePicker_TG_KT.Text = row[0]["TG_KT"].ToString();
                    cbbthietbi.Text = row[0]["thiet_bi"].ToString();
                    cbmaBTP.Text = row[0]["ma_BTP"].ToString();
                    tbtenbtp.Text = row[0]["ten_BTP"].ToString();
                    tbme.Text = row[0]["me"].ToString();
                    tbkhoiluongphanbonnvl.Text = row[0]["klnl_sudung"].ToString();
                    tbtocdorelease.Text = row[0]["tocdo_release"].ToString();
                    tbngay_release.Text = row[0]["ngay_release"].ToString();
                    tbloai.Text = row[0]["loai"].ToString();
                    tbtongklspthuduoc.Text = row[0]["tong_klspsx"].ToString();
                    tbkhoiluongdongkhoi.Text = row[0]["kl_dongkhoi"].ToString();
                    tbspkhongbidongkhoi.Text = row[0]["kl_khongdongkhoi"].ToString();
                    tbkhoiluonglythuyet.Text = row[0]["kl_lythuyet"].ToString();
                    tbhieusuatthu.Text = row[0]["hieuxuat_thu"].ToString();
                    tbhieusuatrelease.Text = row[0]["hieuxuat_release"].ToString();
                    tbthoigiancb.Text = row[0]["thoigian_cb"].ToString();
                    tbthoigiansx.Text = row[0]["thoigian_sx"].ToString();
                    cbbphanbonnvl.Text = row[0]["phanbon_nvl"].ToString();
                    tbkhoiluongphanbonnvl.Text = row[0]["kl_nvl"].ToString();
                    tbbarcodephanbonvl.Text = row[0]["barcode_nvl"].ToString();
                    tbLOTphanbonnvl.Text = row[0]["lot_nvl"].ToString();
                    tbn1157.Text = row[0]["N1"].ToString();
                    tbbarcodeN1.Text = row[0]["barcode_n1"].ToString();
                    tbLOTN1.Text = row[0]["lot_n1"].ToString();
                    tbn221.Text = row[0]["N2"].ToString();
                    tbbarcodeN2.Text = row[0]["barcode_n2"].ToString();
                    tbLOTN2.Text = row[0]["lot_n2"].ToString();
                    tbn3190.Text = row[0]["N3"].ToString();
                    tbbarcodeN3.Text = row[0]["barcode_n3"].ToString();
                    tbLOTN3.Text = row[0]["lot_n3"].ToString();
                    tbga3.Text = row[0]["Ga3"].ToString();
                    tbbarcodeGA3.Text = row[0]["barcode_ga3"].ToString();
                    tbborax.Text = row[0]["Borax"].ToString();
                    tbbarcodeBorax.Text = row[0]["bacode_borax"].ToString();
                    tbnaa.Text = row[0]["Naa"].ToString();
                    tbbarcodeNAA.Text = row[0]["barcode_naa"].ToString();
                    tbsodium.Text = row[0]["Sodium"].ToString();
                    tbbarcodeSodium.Text = row[0]["barcode_sodium"].ToString();
                    tbcitric.Text = row[0]["Citric"].ToString();
                    tbbarcode_citric.Text = row[0]["barcode_citric"].ToString();
                    tbnaoh.Text = row[0]["Naoh"].ToString();
                    tbbarcode_naoh.Text = row[0]["barocde_naoh"].ToString();
                    tbsolubo.Text = row[0]["solubo"].ToString();
                    tbbarcode_solubo.Text = row[0]["barocde_solubo"].ToString();
                    tbEDTA.Text = row[0]["Edta"].ToString();
                    tbbarcode_edta.Text = row[0]["barcode_edta"].ToString();
                    tbred.Text = row[0]["Red"].ToString();
                    tbbarcode_red.Text = row[0]["barcode_red"].ToString();
                    tbviolet.Text = row[0]["violet"].ToString();
                    tbbarcode_violet.Text = row[0]["barcode_violet"].ToString();
                    tbblue.Text = row[0]["blue"].ToString();
                    tbbarcode_blue.Text = row[0]["barocde_blue"].ToString();
                    tbyellow.Text = row[0]["yellow"].ToString();
                    tbbarcode_yellow.Text = row[0]["barcode_yellow"].ToString();
                    tbblack.Text = row[0]["black"].ToString();
                    tbbarcode_black.Text = row[0]["barcode_back"].ToString();
                    tbPREV.Text = row[0]["prev"].ToString();
                    tbbarcode_prev.Text = row[0]["barcode_prev"].ToString();
                    tbsoluongthancam.Text = row[0]["thancam"].ToString();
                    tbkwdien.Text = row[0]["dien"].ToString();
                    tbm3nuocRO.Text = row[0]["nuocRo"].ToString();
                    tbm3nuocthuycuc.Text = row[0]["nuocthuycuc"].ToString();
                    tbbaoholaodong.Text = row[0]["BHLD"].ToString();
                    tbghi_chu.Text = row[0]["ghi_chu"].ToString();
                    tbvitri_tongklsp_thuduoc.Text = row[0]["vitri_spthuduoc"].ToString();
                    tbvitri_spdongkhoi.Text = row[0]["vitri_spdongkhoi"].ToString();
                    tbvitri_spkhongdongkhoi.Text = row[0]["vitri_spkhongdongkhoi"].ToString();
                    tb_do_am.Text = row[0]["do_am"].ToString();
                    tb_coating.Text = row[0]["coating_layer"].ToString();
                    tb_thoigianondinh.Text = row[0]["thoigian_ondinh"].ToString();
                    tb_ngay0.Text = row[0]["ngay_0"].ToString();
                    tb_ngay7.Text = row[0]["ngay_7"].ToString();
                    tb_ngay14.Text = row[0]["ngay_14"].ToString();
                    tb_ngay21.Text = row[0]["ngay_21"].ToString();
                    tb_ngay28.Text = row[0]["ngay_28"].ToString();
                    tb_ngay42.Text = row[0]["ngay_42"].ToString();
                    tb_ngay49.Text = row[0]["ngay_49"].ToString();
                    tb_ngay56.Text = row[0]["ngay_56"].ToString();
                    tb_ngay70.Text = row[0]["ngay_70"].ToString();
                    tb_ngay84.Text = row[0]["ngay_84"].ToString();
                    tb_ngay98.Text = row[0]["ngay_98"].ToString();
                    tb_ngay112.Text = row[0]["ngay_112"].ToString();
                    tb_ngay126.Text = row[0]["ngay_126"].ToString();
                    tb_ngay140.Text = row[0]["ngay_140"].ToString();

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

                    dateTimePicker_TG_BD.Text = row[0]["TG_BD"].ToString();
                    dateTimePicker_TG_KT.Text = row[0]["TG_KT"].ToString();
                    comboBox_nvl_1.Text = row[0]["NVL_1"].ToString();
                    textBox_kl_nvl_1.Text = row[0]["KL_NVL_1"].ToString();
                    textBox_barcode_nvl_1.Text = row[0]["barcode_NVL_1"].ToString();
                    textBox_lot_nvl_1.Text = row[0]["lot_NVL_1"].ToString();
                    comboBox_nvl_2.Text = row[0]["NVL_2"].ToString();
                    textBox_kl_nvl_2.Text = row[0]["KL_NVL_2"].ToString();
                    textBox_barcode_nvl_2.Text = row[0]["barcode_NVL_2"].ToString();
                    textBox_lot_nvl_2.Text = row[0]["lot_NVL_2"].ToString();
                    string N1_BD = "0";
                    string N1_KT = "0";
                    string N2_BD = "0";
                    string N2_KT = "0";
                    string N3_BD = "0";
                    string N3_KT = "0";
                    if (row[0]["N1_BD"].ToString() != "")
                    {
                        N1_BD = row[0]["N1_BD"].ToString();
                    }
                    if (row[0]["N1_KT"].ToString() != "")
                    {
                        N1_KT = row[0]["N1_KT"].ToString();
                    }
                    if (row[0]["N2_BD"].ToString() != "")
                    {
                        N2_BD = row[0]["N2_BD"].ToString();
                    }
                    if (row[0]["N2_KT"].ToString() != "")
                    {
                        N2_KT = row[0]["N2_KT"].ToString();
                    }
                    if (row[0]["N3_BD"].ToString() != "")
                    {
                        N3_BD = row[0]["N3_BD"].ToString();
                    }
                    if (row[0]["N3_KT"].ToString() != "")
                    {
                        N3_KT = row[0]["N3_KT"].ToString();
                    }
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add("N1-157", N1_BD, N1_KT, Math.Round((Convert.ToDouble(N1_BD) - Convert.ToDouble(N1_KT)), 1));
                    dataGridView1.Rows.Add("N2-21", N2_BD, N2_KT, Math.Round((Convert.ToDouble(N2_BD) - Convert.ToDouble(N2_KT)), 1));
                    dataGridView1.Rows.Add("N3-190", N3_BD, N3_KT, Math.Round((Convert.ToDouble(N3_BD) - Convert.ToDouble(N3_KT)), 1));
                    total_tring_barcode_n1_n2_n3();
                    total_tring_lot_n1_n2_n3();
                }
                catch
                {
                    try
                    {
                        cleardata();
                        SqlConnection sqlcon = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=QL_SX;User ID=sa;Password=rynan2020");
                        sqlcon.Open();
                        SqlCommand cmd = new SqlCommand();
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        cmd = sqlcon.CreateCommand();
                        cmd.CommandText = "Select DOT_SX,ME_THU,MA_TB,TG_BD,TG_KT,LOAI_SP,KL_NL from DataSX_RSF WHERE SO_LOT = '" + tblot.Text + "'";
                        sqlDataAdapter.SelectCommand = cmd;
                        DataTable dt_a = new DataTable();
                        dt_a.Clear();
                        sqlDataAdapter.Fill(dt_a);
                        sqlcon.Close();
                        DataRow[] row = dt_a.Select();
                        tbdotsx.Text = row[0]["DOT_SX"].ToString();
                        tbme.Text = row[0]["ME_THU"].ToString();
                        cbbthietbi.Text = row[0]["MA_TB"].ToString();
                        dateTimePickerngaysx.Text = row[0]["TG_BD"].ToString();
                        dateTimePicker_TG_BD.Text = row[0]["TG_BD"].ToString();
                        dateTimePicker_TG_KT.Text = row[0]["TG_KT"].ToString();
                        cbmaBTP.Text = row[0]["LOAI_SP"].ToString();
                        tbkhoiluongphanbonnvl.Text = row[0]["KL_NL"].ToString();
                        update_or_add = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                update_or_add = false;
            }
        }
        private void buttonsave_Click(object sender, EventArgs e)
        {
            if (update_or_add == true)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn muốn cập nhật LOT : '" + tblot.Text + "'", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    update();
                }
            }
            else if (update_or_add == false)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn muốn thêm LOT : " + tblot.Text + "", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                    insert_data();
                }
            }
        }
        public void insert_blogtruycap(string hoat_dong)
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
            sqlcon.Open();
            string Thoi_gian = DateTime.Now.ToString();
            string user = "non";
            SqlCommand cmd = sqlcon.CreateCommand();
            cmd.CommandText = "insert into logtruycap (ten_user,thoi_gian,hoat_dong) values ('" + user + "','" + Thoi_gian + "',N'" + hoat_dong + "')";
            cmd.ExecuteNonQuery();
            sqlcon.Close();
        }
        public void Tinh_kllythuyet()
        {
            try
            {
                tbkhoiluonglythuyet.Text = (Convert.ToDouble(tbkhoiluongphanbonnvl.Text) + (Convert.ToDouble(tbn1157.Text) + Convert.ToDouble(tbn221.Text) + Convert.ToDouble(tbn3190.Text)) / 4).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Tinh_hsthu()
        {
            try
            {
                tbhieusuatthu.Text = Math.Round(((Convert.ToDouble(tbtongklspthuduoc.Text) / Convert.ToDouble(tbkhoiluonglythuyet.Text)) * 100), 4).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Tinh_hsrelease()
        {
            try
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
        }
        private void tb_n1_1_kl_Leave(object sender, EventArgs e)
        {
            double N1_1 = 0;
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
        }
        private void tb_n2_1_kl_Leave(object sender, EventArgs e)
        {
            double N2_1 = 0;
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
        }
        private void tb_n3_1_kl_Leave(object sender, EventArgs e)
        {
            double N3_1 = 0;
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
        }
        private void tbspkhongbidongkhoi_Leave(object sender, EventArgs e)
        {
            double kl_dongkhoi = 0;
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
        }
        private void tbkhoiluonglythuyet_Click(object sender, EventArgs e)
        {
            Tinh_kllythuyet();
        }
        private void tbhieusuatthu_Click(object sender, EventArgs e)
        {
            Tinh_hsthu();
        }
        private void tbhieusuatrelease_Click(object sender, EventArgs e)
        {
            Tinh_hsrelease();
        }
        private void tb_n1_1_code_Leave(object sender, EventArgs e)
        {
            if (tb_n1_1_code.Text != "")
                tbbarcodeN1.Text = tb_n1_1_code.Text;
        }
        private void tb_n1_2_code_Leave(object sender, EventArgs e)
        {
            if (tb_n1_2_code.Text != "")
                tbbarcodeN1.Text += ", " + tb_n1_2_code.Text;
        }
        private void tb_n1_3_code_Leave(object sender, EventArgs e)
        {
            if (tb_n1_3_code.Text != "")
                tbbarcodeN1.Text += ", " + tb_n1_3_code.Text;
        }
        private void tb_n1_4_code_Leave(object sender, EventArgs e)
        {
            if (tb_n1_4_code.Text != "")
                tbbarcodeN1.Text += tb_n1_4_code.Text;
        }
        private void tb_n2_1_code_Leave(object sender, EventArgs e)
        {
            if (tb_n2_1_code.Text != "")
                tbbarcodeN2.Text = tb_n2_1_code.Text;
        }
        private void tb_n2_2_code_Leave(object sender, EventArgs e)
        {
            if (tb_n2_2_code.Text != "")
                tbbarcodeN2.Text += ", " + tb_n2_2_code.Text;
        }
        private void tb_n2_3_code_Leave(object sender, EventArgs e)
        {
            if (tb_n2_3_code.Text != "")
                tbbarcodeN2.Text += tb_n2_3_code.Text;
        }
        private void tb_n3_1_code_Leave(object sender, EventArgs e)
        {
            if (tb_n3_1_code.Text != "")
                tbbarcodeN3.Text = tb_n3_1_code.Text;
        }
        private void tb_n3_2_code_Leave(object sender, EventArgs e)
        {
            if (tb_n3_2_code.Text != "")
                tbbarcodeN3.Text += ", " + tb_n3_2_code.Text;
        }
        private void tb_n3_3_code_Leave(object sender, EventArgs e)
        {
            if (tb_n3_3_code.Text != "")
                tbbarcodeN3.Text += tb_n3_3_code.Text;
        }
        public void total_tring_barcode_n1_n2_n3()
        {
            if (tbbarcodeN1.Text == "" && tbbarcodeN2.Text == "" && tbbarcodeN3.Text == "")
            {
                if (tb_n1_1_code.Text != "")
                    tbbarcodeN1.Text += tb_n1_1_code.Text;
                if (tb_n1_2_code.Text != "")
                    tbbarcodeN1.Text += ", " + tb_n1_2_code.Text;
                if (tb_n1_3_code.Text != "")
                    tbbarcodeN1.Text += ", " + tb_n1_3_code.Text;
                if (tb_n1_4_code.Text != "")
                    tbbarcodeN1.Text += ", " + tb_n1_4_code.Text;

                if (tb_n2_1_code.Text != "")
                    tbbarcodeN2.Text += tb_n2_1_code.Text;
                if (tb_n2_2_code.Text != "")
                    tbbarcodeN2.Text += ", " + tb_n2_2_code.Text;
                if (tb_n2_3_code.Text != "")
                    tbbarcodeN2.Text += ", " + tb_n2_3_code.Text;

                if (tb_n3_1_code.Text != "")
                    tbbarcodeN3.Text += tb_n3_1_code.Text;
                if (tb_n3_2_code.Text != "")
                    tbbarcodeN3.Text += ", " + tb_n3_2_code.Text;
                if (tb_n3_3_code.Text != "")
                    tbbarcodeN3.Text += ", " + tb_n3_3_code.Text;
            }
        }
        public void total_tring_lot_n1_n2_n3()
        {
            if (tbLOTN1.Text == "" && tbLOTN2.Text == "" && tbLOTN3.Text == "")
            {
                if (tb_n1_1_lot.Text != "")
                    tbLOTN1.Text += tb_n1_1_lot.Text;
                if (tb_n1_2_lot.Text != "")
                    tbLOTN1.Text += ", " + tb_n1_2_lot.Text;
                if (tb_n1_3_lot.Text != "")
                    tbLOTN1.Text += ", " + tb_n1_3_lot.Text;
                if (tb_n1_4_lot.Text != "")
                    tbLOTN1.Text += ", " + tb_n1_4_lot.Text;

                if (tb_n2_1_lot.Text != "")
                    tbLOTN2.Text += tb_n2_1_lot.Text;
                if (tb_n2_2_lot.Text != "")
                    tbLOTN2.Text += ", " + tb_n2_2_lot.Text;
                if (tb_n2_3_lot.Text != "")
                    tbLOTN2.Text += ", " + tb_n2_3_lot.Text;

                if (tb_n3_1_lot.Text != "")
                    tbLOTN3.Text += tb_n3_1_lot.Text;
                if (tb_n3_2_lot.Text != "")
                    tbLOTN3.Text += ", " + tb_n3_2_lot.Text;
                if (tb_n3_3_lot.Text != "")
                    tbLOTN3.Text += ", " + tb_n3_3_lot.Text;
            }
        }
        private void tb_n1_1_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n1_1_lot.Text != "")
                tbLOTN1.Text = tb_n1_1_lot.Text;
        }
        private void tb_n1_2_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n1_2_lot.Text != "")
                tbLOTN1.Text += ", " + tb_n1_2_lot.Text;
        }
        private void tb_n1_3_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n1_3_lot.Text != "")
                tbLOTN1.Text += ", " + tb_n1_3_lot.Text;
        }
        private void tb_n1_4_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n1_4_lot.Text != "")
                tbLOTN1.Text += tb_n1_4_lot.Text;
        }
        private void tb_n2_1_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n2_1_lot.Text != "")
                tbLOTN2.Text = tb_n2_1_lot.Text;
        }
        private void tb_n2_2_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n2_2_lot.Text != "")
                tbLOTN2.Text += ", " + tb_n2_2_lot.Text;
        }
        private void tb_n2_3_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n2_3_lot.Text != "")
                tbLOTN2.Text += ", " + tb_n2_3_lot.Text;
        }
        private void tb_n3_1_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n3_1_lot.Text != "")
                tbLOTN3.Text = tb_n3_1_lot.Text;
        }
        private void tb_n3_2_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n3_2_lot.Text != "")
                tbLOTN3.Text += ", " + tb_n3_2_lot.Text;
        }
        private void tb_n3_3_lot_Leave(object sender, EventArgs e)
        {
            if (tb_n3_3_lot.Text != "")
                tbLOTN3.Text += ", " + tb_n3_3_lot.Text;
        }
        public void loadcbb_LOT()
        {
            try
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=QL_SX;User ID=sa;Password=rynan2020");
                sqlcon.Open();
                SqlCommand command = new SqlCommand();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable dt = new DataTable();
                command = sqlcon.CreateCommand();
                command.CommandText = "SELECT SO_LOT,TG_BD from DataSX_RSF ORDER BY TG_BD DESC";
                adapter.SelectCommand = command;
                dt.Clear();
                adapter.Fill(dt);
                sqlcon.Close();
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["SO_LOT"].ToString() != "")
                    {
                        tblot.Items.Add(dataRow["SO_LOT"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void cleardata()
        {
            cbbnguoinhap.Text = "";
            tbdotsx.Text = "";
            dateTimePickerngaysx.Text = "";
            cbbthietbi.Text = "";
            cbmaBTP.Text = "";
            tbtenbtp.Text = "";
            tbme.Text = "";
            //tblot.Text = "";
            tbtocdorelease.Text = "";
            tbngay_release.Text = "";
            tbloai.Text = "";
            tbtongklspthuduoc.Text = "";
            tbvitri_tongklsp_thuduoc.Text = "";
            tbkhoiluongdongkhoi.Text = "";
            tbvitri_spdongkhoi.Text = "";
            tbspkhongbidongkhoi.Text = "";
            tbkhoiluonglythuyet.Text = "";
            tbvitri_spkhongdongkhoi.Text = "";
            tbhieusuatthu.Text = "";
            tbhieusuatrelease.Text = "";
            tbthoigiancb.Text = "";
            tbthoigiansx.Text = "";
            cbbphanbonnvl.Text = "";
            tbkhoiluongphanbonnvl.Text = "";
            tbbarcodephanbonvl.Text = "";
            tbLOTphanbonnvl.Text = "";
            tbn1157.Text = "";
            tbbarcodeN1.Text = "";
            tbLOTN1.Text = "";
            tbn221.Text = "";
            tbbarcodeN2.Text = "";
            tbLOTN2.Text = "";
            tbn3190.Text = "";
            tbbarcodeN3.Text = "";
            tbLOTN3.Text = "";
            tbga3.Text = "";
            tbbarcodeGA3.Text = "";
            tbborax.Text = "";
            tbbarcodeBorax.Text = "";
            tbnaa.Text = "";
            tbbarcodeNAA.Text = "";
            tbsodium.Text = "";
            tbbarcodeSodium.Text = "";
            tbcitric.Text = "";
            tbbarcode_citric.Text = "";
            tbnaoh.Text = "";
            tbbarcode_naoh.Text = "";
            tbsolubo.Text = "";
            tbbarcode_solubo.Text = "";
            tbEDTA.Text = "";
            tbbarcode_edta.Text = "";
            tbred.Text = "";
            tbbarcode_red.Text = "";
            tbviolet.Text = "";
            tbbarcode_violet.Text = "";
            tbblue.Text = "";
            tbbarcode_blue.Text = "";
            tbyellow.Text = "";
            tbbarcode_yellow.Text = "";
            tbblack.Text = "";
            tbbarcode_black.Text = "";
            tbPREV.Text = "";
            tbbarcode_prev.Text = "";
            tbsoluongthancam.Text = "";
            tbkwdien.Text = "";
            tbm3nuocRO.Text = "";
            tbm3nuocthuycuc.Text = "";
            tbbaoholaodong.Text = "";
            tbghi_chu.Text = "";
            tb_n1_1_code.Text = "";
            tb_n1_2_code.Text = "";
            tb_n1_3_code.Text = "";
            tb_n1_4_code.Text = "";
            tb_n1_1_lot.Text = "";
            tb_n1_2_lot.Text = "";
            tb_n1_3_lot.Text = "";
            tb_n1_4_lot.Text = "";
            tb_n2_1_code.Text = "";
            tb_n2_2_code.Text = "";
            tb_n2_3_code.Text = "";
            tb_n2_1_lot.Text = "";
            tb_n2_2_lot.Text = "";
            tb_n2_3_lot.Text = "";
            tb_n3_1_code.Text = "";
            tb_n3_2_code.Text = "";
            tb_n3_3_code.Text = "";
            tb_n3_1_lot.Text = "";
            tb_n3_2_lot.Text = "";
            tb_n3_3_lot.Text = "";
            tb_do_am.Text = "";
            tb_coating.Text = "";
            tb_thoigianondinh.Text = "";
            tb_ngay0.Text = "";
            tb_ngay7.Text = "";
            tb_ngay14.Text = "";
            tb_ngay21.Text = "";
            tb_ngay28.Text = "";
            tb_ngay42.Text = "";
            tb_ngay49.Text = "";
            tb_ngay56.Text = "";
            tb_ngay70.Text = "";
            tb_ngay84.Text = "";
            tb_ngay98.Text = "";
            tb_ngay112.Text = "";
            tb_ngay126.Text = "";
            tb_ngay140.Text = "";
            tbN1_pro.Text = "";
            tbN2_pro.Text = "";
            tbN3_pro.Text = "";
            comboBox_nvl_1.Text = "";
            textBox_kl_nvl_1.Text = "";
            textBox_barcode_nvl_1.Text = "";
            textBox_lot_nvl_1.Text = "";
            comboBox_nvl_2.Text = "";
            textBox_kl_nvl_2.Text = "";
            textBox_barcode_nvl_2.Text = "";
            textBox_lot_nvl_2.Text = "";
            tb_n1_1_kl.Text = "";
            tb_n2_1_kl.Text = "";
            tb_n3_1_kl.Text = "";
        }
        private void tblot_SelectedValueChanged(object sender, EventArgs e)
        {
            load_data(tblot.Text);
        }
        public void Load_data_polymer_pro()
        {
            if (tblot.Text != "")
            {
                try
                {
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand command = new SqlCommand();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "SELECT TG_BD,TG_KT from nhatkysanxuat where LOT = '" + tblot.Text + "'";
                    adapter.SelectCommand = command;
                    dt.Clear();
                    adapter.Fill(dt);
                    sqlcon.Close();
                    DateTime TG_KT = Convert.ToDateTime(dt.Rows[0]["TG_KT"].ToString());
                    DateTime TG_KT_30 = TG_KT.AddMinutes(-35);
                    DateTime TG_BD = Convert.ToDateTime(dt.Rows[0]["TG_BD"].ToString());
                    if (tblot.Text.Substring(0, 2) == "02")
                    {
                        string sql = "select top 1 id ,Siemens_System_COAT2_DB101_COATING_RATE_KL_Kg_Pro_01_VALUE,"
                                    + "Siemens_System_COAT2_DB101_COATING_RATE_KL_Kg_Pro_02_VALUE,"
                                    + "Siemens_System_COAT2_DB101_COATING_RATE_KL_Kg_Pro_03_VALUE "
                                    + "FROM Coater02_Resport "
                                    + "with (index(PK__Coater02__3213E83FF4576378)) "
                                    + "WHERE Siemens_System_COAT2_DB101_COATING_RATE_COATING_RATE_01_TIMESTAMP >= '" + TG_BD + "' AND Siemens_System_COAT2_DB101_COATING_RATE_COATING_RATE_01_TIMESTAMP <= '" + TG_KT_30 + "'"
                                    + " ORDER by Siemens_System_COAT2_DB101_COATING_RATE_COATING_RATE_01_TIMESTAMP DESC ";
                        SqlConnection connect = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=COATER02_ResportDi_2023;Persist Security Info=True;User ID=sa;Password=rynan2020");
                        SqlCommand cmd = new SqlCommand(sql, connect);
                        cmd.CommandTimeout = 120;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt_1 = new DataTable();
                        da.Fill(dt_1);
                        connect.Close();
                        tbN1_pro.Text = Math.Round(float.Parse(dt_1.Rows[0]["Siemens_System_COAT2_DB101_COATING_RATE_KL_Kg_Pro_01_VALUE"].ToString()), 1).ToString();
                        tbN2_pro.Text = Math.Round(float.Parse(dt_1.Rows[0]["Siemens_System_COAT2_DB101_COATING_RATE_KL_Kg_Pro_02_VALUE"].ToString()), 1).ToString();
                        tbN3_pro.Text = Math.Round(float.Parse(dt_1.Rows[0]["Siemens_System_COAT2_DB101_COATING_RATE_KL_Kg_Pro_03_VALUE"].ToString()), 1).ToString();
                    }
                    else if (tblot.Text.Substring(0, 2) == "S1")
                    {
                        string sql = " Select top 1 id ,Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_01_VALUE,"
                                        + "Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_02_VALUE,"
                                        + "Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_03_VALUE "
                                        + "FROM Coater03Resport "
                                        + "with (index(PK__Coater03__3213E83F5524E5D0)) "
                                        + "WHERE Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP >= '" + TG_BD + "' AND Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP <= '" + TG_KT_30 + "'"
                                        + "ORDER by Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP ASC ";
                        SqlConnection connect = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=COATERS1_ResportDi_2023;User ID=sa;Password=rynan2020");
                        SqlCommand cmd = new SqlCommand(sql, connect);
                        cmd.CommandTimeout = 120;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt_2 = new DataTable();
                        da.Fill(dt_2);
                        connect.Close();
                        if (dt_2.Rows[0]["Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_01_VALUE"].ToString() == "")
                        {
                            tbN1_pro.Text = "0";
                        }
                        else
                        {
                            tbN1_pro.Text = Math.Round(float.Parse(dt_2.Rows[0]["Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_01_VALUE"].ToString()), 1).ToString();
                        }
                        if (dt_2.Rows[0]["Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_02_VALUE"].ToString() == "")
                        {
                            tbN2_pro.Text = "0";
                        }
                        else
                        {
                            tbN2_pro.Text = Math.Round(float.Parse(dt_2.Rows[0]["Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_02_VALUE"].ToString()), 1).ToString();
                        }
                        if (dt_2.Rows[0]["Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_03_VALUE"].ToString() == "")
                        {
                            tbN3_pro.Text = "0";
                        }
                        else
                        {
                            tbN3_pro.Text = Math.Round(float.Parse(dt_2.Rows[0]["Siemens_System_COAT_100_V1_DB222_COATING_RATE_AUTO_VAVLE_KL_Kg_Pro_03_VALUE"].ToString()), 1).ToString();
                        }
                    }
                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }
        public void load_data_polymer_sd()
        {
            if (tblot.Text != "")
            {
                try
                {
                    SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                    sqlcon.Open();
                    SqlCommand command = new SqlCommand();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    DataTable dt = new DataTable();
                    command = sqlcon.CreateCommand();
                    command.CommandText = "SELECT * from DATA_polymer where SOLOT = '" + tblot.Text + "'";
                    adapter.SelectCommand = command;
                    dt.Clear();
                    adapter.Fill(dt);
                    sqlcon.Close();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add("N1", dt.Rows[0]["N1_BD"], dt.Rows[0]["N1_KT"], Convert.ToDouble(dt.Rows[0]["N1_BD"]) - Convert.ToDouble(dt.Rows[0]["N1_KT"]));
                    dataGridView1.Rows.Add("N2", dt.Rows[0]["N2_BD"], dt.Rows[0]["N2_KT"], Convert.ToDouble(dt.Rows[0]["N2_BD"]) - Convert.ToDouble(dt.Rows[0]["N2_KT"]));
                    dataGridView1.Rows.Add("N3", dt.Rows[0]["N3_BD"], dt.Rows[0]["N3_KT"], Convert.ToDouble(dt.Rows[0]["N3_BD"]) - Convert.ToDouble(dt.Rows[0]["N3_KT"]));
                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }
        private void textBox_kl_nvl_1_Leave(object sender, EventArgs e)
        {
            if (textBox_kl_nvl_1.Text != "")
            {
                tbkhoiluongphanbonnvl.Text = Convert.ToDouble(textBox_kl_nvl_1.Text).ToString();
            }
        }
        private void textBox_barcode_nvl_1_Leave(object sender, EventArgs e)
        {
            if (textBox_barcode_nvl_1.Text != "")
            {
                tbbarcodephanbonvl.Text = textBox_barcode_nvl_1.Text;
            }
        }
        private void textBox_kl_nvl_2_Leave(object sender, EventArgs e)
        {
            if (textBox_kl_nvl_2.Text != "")
            {
                tbkhoiluongphanbonnvl.Text = (Convert.ToDouble(tbkhoiluongphanbonnvl.Text) + Convert.ToDouble(textBox_kl_nvl_2.Text)).ToString();
            }
        }
        private void textBox_barcode_nvl_2_Leave(object sender, EventArgs e)
        {
            if (textBox_barcode_nvl_2.Text != "")
            {
                tbbarcodephanbonvl.Text += "," + textBox_barcode_nvl_2.Text;
            }
        }
        private void textBox_lot_nvl_1_Leave(object sender, EventArgs e)
        {
            if (textBox_lot_nvl_1.Text != "")
            {
                tbLOTphanbonnvl.Text = textBox_lot_nvl_1.Text;
            }
        }
        private void textBox_lot_nvl_2_Leave(object sender, EventArgs e)
        {
            if (textBox_lot_nvl_2.Text != "")
            {
                tbLOTphanbonnvl.Text += "," + textBox_lot_nvl_2.Text;
            }
        }
        private void comboBox_nvl_1_Leave(object sender, EventArgs e)
        {
            if (comboBox_nvl_1.Text != "")
            {
                cbbphanbonnvl.Text = comboBox_nvl_1.Text;
            }
        }
        private void comboBox_nvl_2_Leave(object sender, EventArgs e)
        {
            if (comboBox_nvl_2.Text != "")
            {
                cbbphanbonnvl.Text += "," + comboBox_nvl_2.Text;
            }
        }
        Double TANK1_BD, TANK1_KT, TANK2_BD, TANK2_KT, TANK3_BD, TANK3_KT;
        Boolean bit_tank1, bit_tank2, bit_tank3, bit_tank4;
        private void load_polymer_Click(object sender, EventArgs e)
        {
            button_load_polymer.Enabled = false;
            ThreadStart threadStart = new ThreadStart(load_data_polymer_use);
            Thread thread = new Thread(threadStart);
            thread.Start();
            thread.IsBackground = true;
            pnloading.Visible = true;
        }
        Double TANK1_BD_02, TANK1_KT_02, TANK2_BD_02, TANK2_KT_02, TANK3_BD_02, TANK3_KT_02;
        Boolean bit_tank1_02, bit_tank2_02, bit_tank3_02, bit_tank4_02;
        string sqlcon, sql;
        public void load_data_polymer_use()
        {
            dataGridView1.Rows.Clear();
            if (tblot.Text != "")
            {
                DateTime StartTime = Convert.ToDateTime(dateTimePicker_TG_BD.Text);
                DateTime EndTime = Convert.ToDateTime(dateTimePicker_TG_KT.Text);
                if (tblot.Text.Substring(0, 2) == "S1")
                {
                    try
                    {
                        if (Convert.ToInt32(tbdotsx.Text) > 53)
                        {
                            sql = " Select Siemens_System_COAT_100_V1_FB_VALVE_DD_1_VALUE,"
                            + "Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_DD_2_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_DD_3_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_DD_NUOC_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_SUNG_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_BOM_POLYMER_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_FAN_VALUE,"
                            + "Siemens_System_COAT_100_V1_NET_TANK_1_VALUE,"
                            + "Siemens_System_COAT_100_V1_NET_TANK_2_VALUE,"
                            + "Siemens_System_COAT_100_V1_NET_TANK_3_VALUE "
                            + "FROM Coater03Resport "
                            + "with (index(PK__Coater03__3213E83F5524E5D0)) "
                            + "WHERE Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP >= '" + StartTime + "' AND Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP <= '" + EndTime + "'"
                            + "ORDER by Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP ASC ";
                            sqlcon = @"Data Source=192.168.23.219,1433;Initial Catalog=COATERS1_ResportDi_2023;User ID=sa;Password=rynan2020";
                        }
                        else
                        {
                            sql = " Select Siemens_System_COAT_100_V1_FB_VALVE_DD_1_VALUE,"
                            + "Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_DD_2_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_DD_3_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_DD_NUOC_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_VALVE_SUNG_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_BOM_POLYMER_VALUE,"
                            + "Siemens_System_COAT_100_V1_FB_FAN_VALUE,"
                            + "Siemens_System_COAT_100_V1_NET_TANK_1_VALUE,"
                            + "Siemens_System_COAT_100_V1_NET_TANK_2_VALUE,"
                            + "Siemens_System_COAT_100_V1_NET_TANK_3_VALUE "
                            + "FROM Coater03Resport "
                            + "with (index(PK__Coater03__3213E83F5B1FD6E6)) "
                            + "WHERE Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP >= '" + StartTime + "' AND Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP <= '" + EndTime + "'"
                            + "ORDER by Siemens_System_COAT_100_V1_ACTIVE_PID_SAY_1_TIMESTAMP ASC ";
                            sqlcon = @"Data Source=192.168.23.219,1433;Initial Catalog=COATERS1_ResportDi;User ID=sa;Password=rynan2020";
                        }
                        SqlConnection connect = new SqlConnection(sqlcon);
                        SqlCommand cmd = new SqlCommand(sql, connect);
                        cmd.CommandTimeout = 500;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        int totalRows_1 = dt.Rows.Count;
                        connect.Close();
                        Boolean DD1, DD2, DD3, DD4, FAN, BOM, SUNG;
                        for (int f = 0; f <= totalRows_1 - 1; f = f + 2)
                        {
                            try
                            {
                                if (dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_1_VALUE"].ToString() == "")
                                    DD1 = true;
                                else
                                    DD1 = Convert.ToBoolean(dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_1_VALUE"]);
                                if (dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_2_VALUE"].ToString() == "")
                                    DD2 = true;
                                else
                                    DD2 = Convert.ToBoolean(dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_2_VALUE"]);
                                if (dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_3_VALUE"].ToString() == "")
                                    DD3 = true;
                                else
                                    DD3 = Convert.ToBoolean(dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_3_VALUE"]);
                                if (dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_NUOC_VALUE"].ToString() == "")
                                    DD4 = true;
                                else
                                    DD4 = Convert.ToBoolean(dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_DD_NUOC_VALUE"]);
                                if (dt.Rows[f]["Siemens_System_COAT_100_V1_FB_FAN_VALUE"].ToString() == "")
                                    FAN = false;
                                else
                                    FAN = Convert.ToBoolean(dt.Rows[f]["Siemens_System_COAT_100_V1_FB_FAN_VALUE"]);
                                if (dt.Rows[f]["Siemens_System_COAT_100_V1_FB_BOM_POLYMER_VALUE"].ToString() == "")
                                    BOM = false;
                                else
                                    BOM = Convert.ToBoolean(dt.Rows[f]["Siemens_System_COAT_100_V1_FB_BOM_POLYMER_VALUE"]);
                                if (dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_SUNG_VALUE"].ToString() == "")
                                    SUNG = false;
                                else
                                    SUNG = Convert.ToBoolean(dt.Rows[f]["Siemens_System_COAT_100_V1_FB_VALVE_SUNG_VALUE"]);
                                if (FAN == true && BOM == true & SUNG == true && DD1 == false & DD2 == true && DD3 == true && DD4 == true && bit_tank1 == false)
                                {
                                    TANK1_BD = Math.Round(Convert.ToDouble(dt.Rows[f]["Siemens_System_COAT_100_V1_NET_TANK_1_VALUE"]), 1);
                                    bit_tank1 = true;
                                }
                                if (FAN == true && BOM == true & SUNG == true && DD1 == true & DD2 == false && DD3 == true && DD4 == true && bit_tank2 == false)
                                {
                                    TANK1_KT = Math.Round(Convert.ToDouble(dt.Rows[f]["Siemens_System_COAT_100_V1_NET_TANK_1_VALUE"]), 1);
                                    TANK2_BD = Math.Round(Convert.ToDouble(dt.Rows[f]["Siemens_System_COAT_100_V1_NET_TANK_2_VALUE"]), 1);
                                    bit_tank2 = true;
                                }
                                if (FAN == true && BOM == true & SUNG == true && DD1 == true & DD2 == true && DD3 == false && DD4 == true && bit_tank3 == false)
                                {
                                    TANK2_KT = Math.Round(Convert.ToDouble(dt.Rows[f]["Siemens_System_COAT_100_V1_NET_TANK_2_VALUE"]), 1);
                                    TANK3_BD = Math.Round(Convert.ToDouble(dt.Rows[f]["Siemens_System_COAT_100_V1_NET_TANK_3_VALUE"]), 1);
                                    bit_tank3 = true;
                                }
                                if (FAN == true && BOM == true & SUNG == true && DD1 == true & DD2 == true && DD3 == true && DD4 == false && bit_tank4 == false)
                                {
                                    TANK3_KT = Math.Round(Convert.ToDouble(dt.Rows[f]["Siemens_System_COAT_100_V1_NET_TANK_3_VALUE"]), 1);
                                    bit_tank4 = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        dataGridView1.Rows.Add("N1-157", TANK1_BD, TANK1_KT, Math.Round(TANK1_BD - TANK1_KT, 1));
                        dataGridView1.Rows.Add("N2-21", TANK2_BD, TANK2_KT, Math.Round(TANK2_BD - TANK2_KT, 1));
                        dataGridView1.Rows.Add("N3-190", TANK3_BD, TANK3_KT, Math.Round(TANK3_BD - TANK3_KT, 1));
                        bit_tank1 = false;
                        bit_tank2 = false;
                        bit_tank3 = false;
                        bit_tank4 = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (tblot.Text.Substring(0, 2) == "02")
                {
                    try
                    {
                        string sql = " Select Siemens_System_COAT2_DB101_COATING_RATE_COATING_RATE_01_TIMESTAMP,"
                            + "Siemens_System_COAT2_I1_2_COAT2_RUN_QUAT_CAP_GIO_FB_VALUE,"
                            + "Siemens_System_COAT2_I14_0_PNEUM2_VALVE_DD_1_FB_VALUE,"
                            + "Siemens_System_COAT2_I14_1_PNEUM2_VALVE_DD_2_FB_VALUE,"
                            + "Siemens_System_COAT2_I14_2_PNEUM2_VALVE_DD_3_FB_VALUE,"
                            + "Siemens_System_COAT2_I14_3_PNEUM2_VALVE_DD_4_FB_VALUE,"
                            + "Siemens_System_COAT2_I6_3_SUPPLY2_RUN_CAP_DICH_01_FB_VALUE,"
                            + "Siemens_System_COAT2_I6_5_SUPPLY2_RUN_CAP_DICH_02_FB_VALUE,"
                            + "Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_1_VALUE,"
                            + "Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_2_VALUE,"
                            + "Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_3_VALUE "
                            + "FROM Coater02_Resport "
                            + "with (index(PK__Coater02__3213E83FF4576378)) "
                            + "WHERE Siemens_System_COAT2_DB101_COATING_RATE_COATING_RATE_01_TIMESTAMP >= '" + StartTime + "' AND Siemens_System_COAT2_DB101_COATING_RATE_COATING_RATE_01_TIMESTAMP <= '" + EndTime + "'"
                            + "ORDER by Siemens_System_COAT2_DB101_COATING_RATE_COATING_RATE_01_TIMESTAMP ASC ";
                        SqlConnection connect = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=COATER02_ResportDi_2023;Persist Security Info=True;User ID=sa;Password=rynan2020");
                        SqlCommand cmd = new SqlCommand(sql, connect);
                        cmd.CommandTimeout = 120;
                        cmd.CommandType = CommandType.Text;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        int totalRows_1 = dt.Rows.Count;
                        connect.Close();
                        Boolean DD1, DD2, DD3, DD4, FAN, BOM1, BOM2;
                        for (int i = 0; i <= totalRows_1 - 1; i++)
                        {
                            try
                            {
                                if (dt.Rows[i]["Siemens_System_COAT2_I14_0_PNEUM2_VALVE_DD_1_FB_VALUE"].ToString() == "")
                                    DD1 = true;
                                else
                                    DD1 = Convert.ToBoolean(dt.Rows[i]["Siemens_System_COAT2_I14_0_PNEUM2_VALVE_DD_1_FB_VALUE"]);
                                if (dt.Rows[i]["Siemens_System_COAT2_I14_1_PNEUM2_VALVE_DD_2_FB_VALUE"].ToString() == "")
                                    DD2 = true;
                                else
                                    DD2 = Convert.ToBoolean(dt.Rows[i]["Siemens_System_COAT2_I14_1_PNEUM2_VALVE_DD_2_FB_VALUE"]);
                                if (dt.Rows[i]["Siemens_System_COAT2_I14_2_PNEUM2_VALVE_DD_3_FB_VALUE"].ToString() == "")
                                    DD3 = true;
                                else
                                    DD3 = Convert.ToBoolean(dt.Rows[i]["Siemens_System_COAT2_I14_2_PNEUM2_VALVE_DD_3_FB_VALUE"]);
                                if (dt.Rows[i]["Siemens_System_COAT2_I14_3_PNEUM2_VALVE_DD_4_FB_VALUE"].ToString() == "")
                                    DD4 = true;
                                else
                                    DD4 = Convert.ToBoolean(dt.Rows[i]["Siemens_System_COAT2_I14_3_PNEUM2_VALVE_DD_4_FB_VALUE"]);
                                if (dt.Rows[i]["Siemens_System_COAT2_I1_2_COAT2_RUN_QUAT_CAP_GIO_FB_VALUE"].ToString() == "")
                                    FAN = false;
                                else
                                    FAN = Convert.ToBoolean(dt.Rows[i]["Siemens_System_COAT2_I1_2_COAT2_RUN_QUAT_CAP_GIO_FB_VALUE"]);
                                if (dt.Rows[i]["Siemens_System_COAT2_I6_3_SUPPLY2_RUN_CAP_DICH_01_FB_VALUE"].ToString() == "")
                                    BOM1 = false;
                                else
                                    BOM1 = Convert.ToBoolean(dt.Rows[i]["Siemens_System_COAT2_I6_3_SUPPLY2_RUN_CAP_DICH_01_FB_VALUE"]);
                                if (dt.Rows[i]["Siemens_System_COAT2_I6_5_SUPPLY2_RUN_CAP_DICH_02_FB_VALUE"].ToString() == "")
                                    BOM2 = false;
                                else
                                    BOM2 = Convert.ToBoolean(dt.Rows[i]["Siemens_System_COAT2_I6_5_SUPPLY2_RUN_CAP_DICH_02_FB_VALUE"]);
                                if (FAN == true && BOM1 == true && BOM2 == true && DD1 == false && DD2 == true && DD3 == true && DD4 == true && bit_tank1_02 == false)
                                {
                                    TANK1_BD_02 = Math.Round(Convert.ToDouble(dt.Rows[i]["Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_1_VALUE"]), 1);
                                    bit_tank1_02 = true;
                                }
                                if (FAN == true && BOM1 == true && BOM2 == true && DD1 == true && DD2 == false && DD3 == true && DD4 == true && bit_tank2_02 == false)
                                {
                                    TANK1_KT_02 = Math.Round(Convert.ToDouble(dt.Rows[i]["Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_1_VALUE"]), 1);
                                    TANK2_BD_02 = Math.Round(Convert.ToDouble(dt.Rows[i]["Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_2_VALUE"]), 1);
                                    bit_tank2_02 = true;
                                }
                                if (FAN == true && BOM1 == true && BOM2 == true && DD1 == true && DD2 == true && DD3 == false && DD4 == true && bit_tank3_02 == false)
                                {
                                    TANK2_KT_02 = Math.Round(Convert.ToDouble(dt.Rows[i]["Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_2_VALUE"]), 1);
                                    TANK3_BD_02 = Math.Round(Convert.ToDouble(dt.Rows[i]["Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_3_VALUE"]), 1);
                                    bit_tank3_02 = true;
                                }
                                if (FAN == true && BOM1 == true && BOM2 == true && DD1 == true && DD2 == true && DD3 == true && DD4 == false && bit_tank4_02 == false)
                                {
                                    TANK3_KT_02 = Math.Round(Convert.ToDouble(dt.Rows[i]["Siemens_System_COAT2_DB214_W100_NET_WEIGHT_CONVERT_NET_WEIGHT_POLYMER_TANK_POLYMEER_TANK_3_VALUE"]), 1);
                                    bit_tank3_02 = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        dataGridView1.Rows.Add("N1-157", TANK1_BD_02, TANK1_KT_02, Math.Round(TANK1_BD_02 - TANK1_KT_02, 1));
                        dataGridView1.Rows.Add("N2-21", TANK2_BD_02, TANK2_KT_02, Math.Round(TANK2_BD_02 - TANK2_KT_02, 1));
                        dataGridView1.Rows.Add("N3-190", TANK3_BD_02, TANK3_KT_02, Math.Round(TANK3_BD_02 - TANK3_KT_02, 1));
                        bit_tank1_02 = false;
                        bit_tank2_02 = false;
                        bit_tank3_02 = false;
                        bit_tank4_02 = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            pnloading.Visible = false;
            button_load_polymer.Enabled = true;
        }
        public void load_time()
        {
            SqlConnection sqlcon = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=QL_SX;User ID=sa;Password=rynan2020");
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            cmd = sqlcon.CreateCommand();
            cmd.CommandText = "Select TG_BD,TG_KT from DataSX_RSF WHERE SO_LOT = '" + tblot.Text + "'";
            sqlDataAdapter.SelectCommand = cmd;
            DataTable dt_a = new DataTable();
            dt_a.Clear();
            sqlDataAdapter.Fill(dt_a);
            sqlcon.Close();
            DataRow[] row = dt_a.Select();
            dateTimePicker_TG_BD.Text = row[0]["TG_BD"].ToString();
            dateTimePicker_TG_KT.Text = row[0]["TG_KT"].ToString();
        }
    }
}
