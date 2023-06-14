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
        }
        public void insert_data()
        {
            if (tblot.Text == "")
            {
                MessageBox.Show("Chưa Nhập LOT", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                SqlConnection sqlcon = new SqlConnection(@"Data Source = 192.168.21.244,1433; Initial Catalog= RSFLOGSANXUAT ;User ID = sa; Password =mylan@2016");
                SqlCommand command = new SqlCommand();
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
                        "ngay_0,ngay_7,ngay_14,ngay_21,ngay_28,ngay_42,ngay_49,ngay_56,ngay_70,ngay_84,ngay_98,ngay_112,ngay_126,ngay_140)" +
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
                        "'" + ngay_112 + "','" + ngay_126 + "','" + ngay_140 + "')";
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
                        "N3_2_lot='" + tb_n3_2_lot.Text + "',N3_3='" + tb_n3_3_kl.Text + "',N3_3_barcode='" + tb_n3_3_code.Text + "',N3_3_lot='" + tb_n3_3_lot.Text + "' where LOT ='" + tblot.Text + "'";
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
                }
                catch
                {
                    SqlConnection sqlcon = new SqlConnection(@"Data Source=192.168.23.219,1433;Initial Catalog=QL_SX;User ID=sa;Password=rynan2020");
                    sqlcon.Open();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    cmd = sqlcon.CreateCommand();
                    cmd.CommandText = "Select DOT_SX,ME_THU,MA_TB,TG_BD,LOAI_SP,KL_NL from DataSX_RSF WHERE SO_LOT = '" + tblot.Text + "'";
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
                    cbmaBTP.Text = row[0]["LOAI_SP"].ToString();
                    tbkhoiluongphanbonnvl.Text = row[0]["KL_NL"].ToString();
                    update_or_add = false;
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
            {
                tbbarcodeN1.Text += tb_n1_1_code.Text + ", ";
            }
        }
        private void tb_n1_2_code_Leave(object sender, EventArgs e)
        {
            if (tb_n1_2_code.Text != "")
            {
                tbbarcodeN1.Text += tb_n1_2_code.Text + ", ";
            }
        }
        private void tb_n1_3_code_Leave(object sender, EventArgs e)
        {
            if (tb_n1_3_code.Text != "")
            {
                tbbarcodeN1.Text += tb_n1_3_code.Text + ", ";
            }
        }
        private void tb_n1_4_code_Leave(object sender, EventArgs e)
        {
            if (tb_n1_4_code.Text != "")
            {
                tbbarcodeN1.Text += tb_n1_4_code.Text;
            }
        }
        private void tb_n2_1_code_Leave(object sender, EventArgs e)
        {
            if (tb_n2_1_code.Text != "")
            {
                tbbarcodeN2.Text += tb_n2_1_code.Text + ", ";
            }
        }
        private void tb_n2_2_code_Leave(object sender, EventArgs e)
        {
            if (tb_n2_2_code.Text != "")
            {
                tbbarcodeN2.Text += tb_n2_2_code.Text + ", ";
            }
        }
        private void tb_n2_3_code_Leave(object sender, EventArgs e)
        {
            if (tb_n2_3_code.Text != "")
            {
                tbbarcodeN2.Text += tb_n2_3_code.Text;
            }
        }
        private void tb_n3_1_code_Leave(object sender, EventArgs e)
        {
            if (tb_n3_1_code.Text != "")
            {
                tbbarcodeN3.Text += tb_n3_1_code.Text + ", ";
            }
        }
        private void tb_n3_2_code_Leave(object sender, EventArgs e)
        {
            if (tb_n3_2_code.Text != "")
            {
                tbbarcodeN3.Text += tb_n3_2_code.Text + ", ";
            }
        }
        private void tb_n3_3_code_Leave(object sender, EventArgs e)
        {
            if (tb_n3_3_code.Text != "")
            {
                tbbarcodeN3.Text += tb_n3_3_code.Text + ", ";
            }
        }
    }
}
