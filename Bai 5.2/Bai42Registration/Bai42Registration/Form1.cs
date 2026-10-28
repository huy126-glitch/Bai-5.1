
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Bai42Registration
{
    public partial class Form1 : Form
    {
        private TextBox txtHoTen;
        private TextBox txtSoDienThoai;
        private DateTimePicker dtpNgaySinh;
        private RadioButton radNam;
        private RadioButton radNu;
        private ComboBox cboKhoaHoc;
        private RadioButton radTrucTuyen;
        private RadioButton radTrucTiep;
        private CheckBox chkNhanThongBao;
        private Label lblHocPhi;
        private Button btnDangKy;
        private Button btnLamMoi;
        private Button btnThoat;
        private ErrorProvider errorProvider;

        public Form1()
        {
            InitializeComponent();

            // Tao giao dien bang code
            Controls.Clear();

            Text = "Bài 4.2 - Đăng ký khóa học";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(680, 590);
            MinimumSize = new Size(680, 590);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 10);

            errorProvider = new ErrorProvider();
            errorProvider.ContainerControl = this;

            TaoGiaoDien();
            Load += Form1_Load;
        }

        private void TaoGiaoDien()
        {
            Label lblTieuDe = new Label
            {
                Text = "PHIẾU ĐĂNG KÝ KHÓA HỌC",
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(145, 20)
            };

            Controls.Add(lblTieuDe);

            // Ho ten
            ThemNhan("Họ và tên:", 75);

            txtHoTen = new TextBox
            {
                Location = new Point(205, 70),
                Size = new Size(370, 28),
                Name = "txtHoTen"
            };
            Controls.Add(txtHoTen);

            // So dien thoai
            ThemNhan("Số điện thoại:", 120);

            txtSoDienThoai = new TextBox
            {
                Location = new Point(205, 115),
                Size = new Size(370, 28),
                Name = "txtSoDienThoai",
                MaxLength = 20
            };
            Controls.Add(txtSoDienThoai);

            // Ngay sinh
            ThemNhan("Ngày sinh:", 165);

            dtpNgaySinh = new DateTimePicker
            {
                Location = new Point(205, 160),
                Size = new Size(230, 28),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MaxDate = DateTime.Today,
                Value = new DateTime(2000, 1, 1)
            };
            Controls.Add(dtpNgaySinh);

            // Gioi tinh
            ThemNhan("Giới tính:", 210);

            radNam = new RadioButton
            {
                Text = "Nam",
                Location = new Point(205, 205),
                AutoSize = true,
                Checked = true
            };

            radNu = new RadioButton
            {
                Text = "Nữ",
                Location = new Point(290, 205),
                AutoSize = true
            };

            Controls.Add(radNam);
            Controls.Add(radNu);

            // Khoa hoc
            ThemNhan("Khóa học:", 255);

            cboKhoaHoc = new ComboBox
            {
                Location = new Point(205, 250),
                Size = new Size(370, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cboKhoaHoc"
            };

            cboKhoaHoc.SelectedIndexChanged +=
                CboKhoaHoc_SelectedIndexChanged;

            Controls.Add(cboKhoaHoc);

            // Hoc phi
            ThemNhan("Học phí:", 300);

            lblHocPhi = new Label
            {
                Text = "Chưa chọn khóa học",
                Location = new Point(205, 295),
                Size = new Size(370, 30),
                ForeColor = Color.DarkBlue,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Controls.Add(lblHocPhi);

            // Hinh thuc hoc
            ThemNhan("Hình thức học:", 345);

            radTrucTiep = new RadioButton
            {
                Text = "Trực tiếp",
                Location = new Point(205, 340),
                AutoSize = true,
                Checked = true
            };

            radTrucTuyen = new RadioButton
            {
                Text = "Trực tuyến",
                Location = new Point(315, 340),
                AutoSize = true
            };

            Controls.Add(radTrucTiep);
            Controls.Add(radTrucTuyen);

            // Nhan thong bao
            chkNhanThongBao = new CheckBox
            {
                Text = "Đăng ký nhận thông báo khóa học",
                Location = new Point(205, 380),
                AutoSize = true
            };
            Controls.Add(chkNhanThongBao);

            // Nut dang ky
            btnDangKy = new Button
            {
                Text = "Đăng ký",
                Location = new Point(105, 445),
                Size = new Size(120, 40)
            };
            btnDangKy.Click += BtnDangKy_Click;
            Controls.Add(btnDangKy);

            // Nut lam moi
            btnLamMoi = new Button
            {
                Text = "Làm mới",
                Location = new Point(260, 445),
                Size = new Size(120, 40)
            };
            btnLamMoi.Click += BtnLamMoi_Click;
            Controls.Add(btnLamMoi);

            // Nut thoat
            btnThoat = new Button
            {
                Text = "Thoát",
                Location = new Point(415, 445),
                Size = new Size(120, 40)
            };
            btnThoat.Click += BtnThoat_Click;
            Controls.Add(btnThoat);

            AcceptButton = btnDangKy;
            CancelButton = btnThoat;
        }

        private void ThemNhan(string noiDung, int y)
        {
            Label lbl = new Label
            {
                Text = noiDung,
                Location = new Point(55, y),
                Size = new Size(140, 28),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Controls.Add(lbl);
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            TaiDanhSachKhoaHoc();
        }

        // Doc danh sach khoa hoc tu SQL Server
        private void TaiDanhSachKhoaHoc()
        {
            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT Id, TenKhoaHoc, HocPhi
                        FROM dbo.KhoaHoc
                        ORDER BY Id";

                    using (SqlDataAdapter adapter =
                           new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cboKhoaHoc.DataSource = null;
                        cboKhoaHoc.DisplayMember = "TenKhoaHoc";
                        cboKhoaHoc.ValueMember = "Id";
                        cboKhoaHoc.DataSource = dt;
                        cboKhoaHoc.SelectedIndex = -1;

                        lblHocPhi.Text = "Chưa chọn khóa học";
                    }
                }

                if (cboKhoaHoc.Items.Count == 0)
                {
                    MessageBox.Show(
                        "Chưa có khóa học trong cơ sở dữ liệu.",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không tải được danh sách khóa học.\n\n" + ex.Message,
                    "Lỗi cơ sở dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CboKhoaHoc_SelectedIndexChanged(
            object? sender, EventArgs e)
        {
            if (cboKhoaHoc.SelectedItem is DataRowView row)
            {
                decimal hocPhi = Convert.ToDecimal(row["HocPhi"]);

                lblHocPhi.Text =
                    hocPhi.ToString("N0") + " VNĐ";
            }
            else
            {
                lblHocPhi.Text = "Chưa chọn khóa học";
            }
        }

        // Kiem tra va luu dang ky
        private void BtnDangKy_Click(object? sender, EventArgs e)
        {
            errorProvider.Clear();

            bool hopLe = true;

            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                errorProvider.SetError(
                    txtHoTen, "Vui lòng nhập họ và tên.");
                hopLe = false;
            }

            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                errorProvider.SetError(
                    txtSoDienThoai, "Vui lòng nhập số điện thoại.");
                hopLe = false;
            }
            else
            {
                string soChiChu = soDienThoai.Replace(" ", "")
                                             .Replace("-", "")
                                             .Replace("+", "");

                if (!System.Text.RegularExpressions.Regex
                    .IsMatch(soChiChu, @"^\d{9,15}$"))
                {
                    errorProvider.SetError(
                        txtSoDienThoai,
                        "Số điện thoại phải có từ 9 đến 15 chữ số.");
                    hopLe = false;
                }
            }

            if (dtpNgaySinh.Value.Date >= DateTime.Today)
            {
                errorProvider.SetError(
                    dtpNgaySinh,
                    "Ngày sinh phải trước ngày hôm nay.");
                hopLe = false;
            }

            if (cboKhoaHoc.SelectedIndex < 0)
            {
                errorProvider.SetError(
                    cboKhoaHoc, "Vui lòng chọn khóa học.");
                hopLe = false;
            }

            if (!hopLe)
                return;

            string gioiTinh = radNam.Checked ? "Nam" : "Nữ";
            string hinhThuc = radTrucTuyen.Checked
                ? "Trực tuyến"
                : "Trực tiếp";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO dbo.DangKy
                        (
                            HoTen,
                            SoDienThoai,
                            NgaySinh,
                            GioiTinh,
                            KhoaHocId,
                            HinhThucHoc,
                            NhanThongBao
                        )
                        VALUES
                        (
                            @HoTen,
                            @SoDienThoai,
                            @NgaySinh,
                            @GioiTinh,
                            @KhoaHocId,
                            @HinhThucHoc,
                            @NhanThongBao
                        )";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(
                            "@HoTen", SqlDbType.NVarChar, 100
                        ).Value = txtHoTen.Text.Trim();

                        cmd.Parameters.Add(
                            "@SoDienThoai", SqlDbType.VarChar, 20
                        ).Value = soDienThoai;

                        cmd.Parameters.Add(
                            "@NgaySinh", SqlDbType.Date
                        ).Value = dtpNgaySinh.Value.Date;

                        cmd.Parameters.Add(
                            "@GioiTinh", SqlDbType.NVarChar, 10
                        ).Value = gioiTinh;

                        cmd.Parameters.Add(
                            "@KhoaHocId", SqlDbType.Int
                        ).Value = Convert.ToInt32(cboKhoaHoc.SelectedValue);

                        cmd.Parameters.Add(
                            "@HinhThucHoc", SqlDbType.NVarChar, 30
                        ).Value = hinhThuc;

                        cmd.Parameters.Add(
                            "@NhanThongBao", SqlDbType.Bit
                        ).Value = chkNhanThongBao.Checked;

                        int ketQua = cmd.ExecuteNonQuery();

                        if (ketQua > 0)
                        {
                            string tenKhoaHoc =
                                cboKhoaHoc.Text;

                            MessageBox.Show(
                                "Đăng ký khóa học thành công!\n\n" +
                                "Họ tên: " + txtHoTen.Text.Trim() + "\n" +
                                "Số điện thoại: " + soDienThoai + "\n" +
                                "Khóa học: " + tenKhoaHoc + "\n" +
                                "Hình thức: " + hinhThuc,
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            LamMoiForm();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể lưu thông tin đăng ký.\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            LamMoiForm();
        }

        private void LamMoiForm()
        {
            errorProvider.Clear();

            txtHoTen.Clear();
            txtSoDienThoai.Clear();

            dtpNgaySinh.Value = new DateTime(2000, 1, 1);

            radNam.Checked = true;
            radTrucTiep.Checked = true;
            chkNhanThongBao.Checked = false;

            cboKhoaHoc.SelectedIndex = -1;
            lblHocPhi.Text = "Chưa chọn khóa học";

            txtHoTen.Focus();
        }

        private void BtnThoat_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
