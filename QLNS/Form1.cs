using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;
using System.Collections;

namespace QLNS
{
    public partial class Form1 : Form
    {
        QLNVDataContext db = new QLNVDataContext();
        Table<NHANVIEN> nhanviens;
        Table<PHONGBAN> phongbans;
        public Form1()
        {
            InitializeComponent();
        }
        //Lấy dữ liệu phong ban
        public void loadPB()
        {
            //Nguồn dữ liệu
            phongbans = db.GetTable<PHONGBAN>();
            //Câu lệnh truy vấn Linq
            var query = from pb in phongbans
                        select new { mapb = pb.MaPB, tenpb = pb.TenPB };

            //Thực thi câu lệnh truy vấn
            cboPhongBan.DataSource = query;
            cboPhongBan.DisplayMember = "tenpb";
            cboPhongBan.ValueMember = "mapb";

        }
        //Lấy dữ liệu nhân viên
        public void loadNV()
        {
            //Nguồn dữ liệu
            nhanviens = db.GetTable<NHANVIEN>();
            //Câu lệnh truy vấn Linq
            var query = from nv in nhanviens
                        select nv;

            //Thực thi câu lệnh truy vấn
            data.DataSource = query;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadPB();
            loadNV();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            NHANVIEN nv = new NHANVIEN();
            nv.MaNV = txtMaNV.Text;
            nv.TenNV = txtHoTen.Text;
            nv.NgaySinh = Convert.ToDateTime(dtpNgaySinh.Text);
            nv.GioiTinh = rdNam.Checked == true ? true : false;
            nv.SoDT = txtSDT.Text;
            nv.MaPB = cboPhongBan.SelectedValue.ToString();

            nhanviens.InsertOnSubmit(nv);//Thêm nv trên DataContext
            db.SubmitChanges();//Cập nhật xuống Database
            loadNV();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                int i = data.CurrentCell.RowIndex;
                string ma = data.Rows[i].Cells[0].Value.ToString();
                var query = from nv in nhanviens
                            where nv.MaNV == ma
                            select nv;
                foreach (var nv in query)
                {
                    nhanviens.DeleteOnSubmit(nv);
                }
                db.SubmitChanges();
                loadNV();
            }    
            
                
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = data.CurrentCell.RowIndex;
            txtMaNV.Text = data.Rows[i].Cells[0].Value.ToString();
            txtHoTen.Text = data.Rows[i].Cells[1].Value.ToString();
            dtpNgaySinh.Text = data.Rows[i].Cells[2].Value.ToString();
            string gt = data.Rows[i].Cells[3].Value.ToString();
            if (gt == "True")
            {
                rdNam.Checked = true;
            }
            else
                rdNu.Checked = true;
            txtSDT.Text = data.Rows[i].Cells[4].Value.ToString();
            cboPhongBan.SelectedValue = data.Rows[i].Cells[5].Value.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var query = from nv in nhanviens
                        where nv.MaNV == txtMaNV.Text
                        select nv;
            foreach (var nv in query)
            {
                nv.MaNV = txtMaNV.Text;
                nv.TenNV = txtHoTen.Text;
                nv.NgaySinh = Convert.ToDateTime(dtpNgaySinh.Text);
                nv.GioiTinh = rdNam.Checked == true ? true : false;
                nv.SoDT = txtSDT.Text;
                nv.MaPB = cboPhongBan.SelectedValue.ToString();
            }
            db.SubmitChanges();
            loadNV();
        }
    }  
}
