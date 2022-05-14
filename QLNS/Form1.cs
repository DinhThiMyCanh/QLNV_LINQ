using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;
namespace QLNS
{
    public partial class Form1 : Form
    {
        QLNVDataContext db = new QLNVDataContext();
        Table<NhanVien> nhanViens;
        Table<PhongBan> phongBans;
        public Form1()
        {
            InitializeComponent();
        }
        //Load du lieu len Combobox phong ban
        public void loadPB()
        {
            phongBans = db.GetTable<PhongBan>();
            var query = from pb in phongBans
                        select new { mapb = pb.MaPB, tenpb = pb.TenPB };
            cboPhongBan.DataSource = query;
            cboPhongBan.DisplayMember = "tenpb";
            cboPhongBan.ValueMember = "mapb";

        }
        //Load du lieu len dataGridview
        public void loadData()
        {
            nhanViens = db.GetTable<NhanVien>();
            var query = from nv in nhanViens
                        select nv;
            dataGridView1.DataSource = query;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadPB();
            loadData();

        }
        //Them 1 nhan vien moi
        private void btnThem_Click(object sender, EventArgs e)
        {
            NhanVien nv = new NhanVien();
            nv.MaNV = txtMaNV.Text;
            nv.TenNV = txtHoTen.Text;
            nv.NgaySinh = Convert.ToDateTime(dtpNgaySinh.Text);
            nv.GioiTinh = rdNam.Checked == true ? true : false;
            nv.SoDT = txtSDT.Text;
            nv.MaPB = cboPhongBan.SelectedValue.ToString();

            nhanViens.InsertOnSubmit(nv);
            db.SubmitChanges();
            loadData();
        }
    }  
}
