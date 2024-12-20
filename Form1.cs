using De02.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De02
{
   
    public partial class Form1 : Form
    {
        private MyDbContext dbContext = new MyDbContext(); 
        private string selectedProductId;
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSanpham();
            LoadLoaiSP();
        }
        private void LoadSanpham()
        {
            try
            {
                var sanphamList = dbContext.Sanpham.ToList();
                dataGridView1.DataSource = sanphamList;

                // Định dạng cột
                dataGridView1.Columns["NgayNhap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dataGridView1.Columns["MaSP"].HeaderText = "Mã sản phẩm";
                dataGridView1.Columns["TenSP"].HeaderText = "Tên sản phẩm";
                dataGridView1.Columns["NgayNhap"].HeaderText = "Ngày Nhập";
                dataGridView1.Columns["MaLoai"].HeaderText = "Mã Loại";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load danh sách loại sản phẩm vào ComboBox
        private void LoadLoaiSP()
        {
            try
            {
                var loaiSPList = dbContext.LoaiSP.ToList();
                cboloaisp.DataSource = loaiSPList;
                cboloaisp.DisplayMember = "TenLoai";
                cboloaisp.ValueMember = "MaLoai";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải loại sản phẩm: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                var sanpham = new Sanpham
                {
                    MaSP = txtMaSP.Text,
                    TenSP = textBox4.Text,
                    NgayNhap = dtNgaynhap.Value,
                    MaLoai = cboloaisp.SelectedValue.ToString()
                };

                dbContext.Sanpham.Add(sanpham);
                dbContext.SaveChanges();

                LoadSanpham();
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string maSP = txtMaSP.Text;
                var sanpham = dbContext.Sanpham.FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanpham != null)
                {
                    sanpham.TenSP = textBox4.Text;
                    sanpham.NgayNhap = dtNgaynhap.Value;
                    sanpham.MaLoai = cboloaisp.SelectedValue.ToString();

                    dbContext.SaveChanges();
                    LoadSanpham();
                    MessageBox.Show("Chỉnh sửa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chỉnh sửa: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string maSP = txtMaSP.Text;
                var sanpham = dbContext.Sanpham.FirstOrDefault(sp => sp.MaSP == maSP);

                if (sanpham != null)
                {
                    dbContext.Sanpham.Remove(sanpham);
                    dbContext.SaveChanges();
                    LoadSanpham();
                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = textBox4.Text.Trim();
                var result = dbContext.Sanpham
                    .Where(sp => sp.TenSP.Contains(keyword))
                    .ToList();

                dataGridView1.DataSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvSanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtMaSP.Text = row.Cells["MaSP"].Value.ToString();
                textBox4.Text = row.Cells["TenSP"].Value.ToString();
                dtNgaynhap.Value = DateTime.Parse(row.Cells["NgayNhap"].Value.ToString());
                cboloaisp.SelectedValue = row.Cells["MaLoai"].Value.ToString();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();      }
    }
}
