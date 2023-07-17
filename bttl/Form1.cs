using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows.Forms;
using bttl.Model;

namespace bttl
{
    public partial class Form1 : Form
    {
        Model1 db = new Model1();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            // datatimepicker
            datetimepicker.Format = DateTimePickerFormat.Custom;
            datetimepicker.CustomFormat = "dd/MM/yyyy";
            datetimepicker.Value = DateTime.Now;

            cbPhongBan.SelectedIndex = 0;
        }

        private void LoadData()
        {
            List<Phongban> pb = db.Phongbans.ToList();
            List<Nhanvien> nv = db.Nhanviens.ToList();
            addToComboBox(pb);
            BindData(nv);
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ form
            string maNV = txtMaNV.Text.Trim();
            string tenNV = txtTenNV.Text.Trim();

            DateTime ngaySinh;
            try
            {
                ngaySinh = DateTime.Parse(datetimepicker.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ngày sinh không hợp lệ!");
                return;
            }

            // Kiểm tra danh sách phòng ban
            if (cbPhongBan.Items.Count == 0)
            {
                MessageBox.Show("Không có phòng ban nào trong danh sách!");
                return;
            }

            // Lấy mã phòng ban từ ComboBox
            string maPB = cbPhongBan.SelectedValue?.ToString();
            if (maPB == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban!");
                return;
            }

            // Kiểm tra trùng mã NV
            var existingNV = db.Nhanviens.FirstOrDefault(x => x.MaNV == maNV);

            if (existingNV != null)
            {
                MessageBox.Show("Mã nhân viên đã tồn tại!");
                return;
            }

            var nv = new Nhanvien()
            {
                MaNV = maNV,
                TenNV = tenNV,
                Ngaysinh = ngaySinh,
                MaPB = maPB
            };

            try
            {
                db.Nhanviens.Add(nv);
                db.SaveChanges();
                MessageBox.Show("Thêm nhân viên thành công!");

                // Load lại dữ liệu
                LoadData();
            }
            catch (DbUpdateException ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private void addToComboBox(List<Phongban> list)
        {
            // Set the DataSource property to null to clear any existing items
            cbPhongBan.DataSource = null;

            cbPhongBan.DataSource = list;
            cbPhongBan.DisplayMember = "TenPB";
            cbPhongBan.ValueMember = "MaPB";
        }

        private void BindData(List<Nhanvien> nv)
        {
            dataGridView1.Rows.Clear();
            foreach (Nhanvien nhanvien in nv)
            {
                int idx = dataGridView1.Rows.Add();
                dataGridView1.Rows[idx].Cells[0].Value = nhanvien.MaNV;
                dataGridView1.Rows[idx].Cells[1].Value = nhanvien.TenNV;
                dataGridView1.Rows[idx].Cells[2].Value = nhanvien.Ngaysinh;
                dataGridView1.Rows[idx].Cells[3].Value = nhanvien.Phongban.TenPB;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idx = e.RowIndex;
            txtMaNV.Text = dataGridView1.Rows[idx].Cells[0].Value.ToString();
            txtTenNV.Text = dataGridView1.Rows[idx].Cells[1].Value.ToString();
            datetimepicker.Text = dataGridView1.Rows[idx].Cells[2].Value.ToString();
            cbPhongBan.SelectedValue = dataGridView1.Rows[idx].Cells[3].Value.ToString();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var find = db.Nhanviens.FirstOrDefault(s => s.MaNV == txtMaNV.Text);
            if (find != null)
            {
                var ask = MessageBox.Show("Ban co muon xoa ? ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes)
                {
                    db.Nhanviens.Remove(find);
                }
                db.SaveChanges();
                List<Nhanvien> nv = db.Nhanviens.ToList();
                BindData(nv);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var find = db.Nhanviens.FirstOrDefault(s => s.MaNV == txtMaNV.Text);
            if (find != null)
            {
                // Lấy dữ liệu từ form
                string tenNV = txtTenNV.Text.Trim();

                DateTime ngaySinh;
                try
                {
                    ngaySinh = DateTime.Parse(datetimepicker.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Ngày sinh không hợp lệ!");
                    return;
                }

                // Kiểm tra danh sách phòng ban
                if (cbPhongBan.Items.Count == 0)
                {
                    MessageBox.Show("Không có phòng ban nào trong danh sách!");
                    return;
                }

                // Lấy mã phòng ban từ ComboBox
                string maPB = cbPhongBan.SelectedValue?.ToString();
                if (maPB == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng ban!");
                    return;
                }

                find.TenNV = tenNV;
                find.Ngaysinh = ngaySinh;
                find.MaPB = maPB;

                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Cập nhật nhân viên thành công!");

                    // Load lại dữ liệu
                    List<Nhanvien> nv = db.Nhanviens.ToList();
                    BindData(nv);
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên có mã " + txtMaNV.Text);
            }
        }
    }
}