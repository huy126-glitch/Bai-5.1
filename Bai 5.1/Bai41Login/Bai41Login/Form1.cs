
using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Bai41Login
{
    public partial class Form1 : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnLogin;
        private Button btnExit;
        private ErrorProvider errorProvider;

        public Form1()
        {
            InitializeComponent();

            // Xoa cac control mac dinh do Designer tao ra
            Controls.Clear();

            // Cau hinh cua so
            Text = "Bài 4.1 - Đăng nhập";
            Size = new Size(460, 330);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            // ErrorProvider
            errorProvider = new ErrorProvider();
            errorProvider.ContainerControl = this;

            // Tieu de
            Label lblTitle = new Label
            {
                Text = "ĐĂNG NHẬP HỆ THỐNG",
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(85, 25)
            };

            // Ten dang nhap
            Label lblUsername = new Label
            {
                Text = "Tên đăng nhập:",
                AutoSize = true,
                Location = new Point(35, 90)
            };

            txtUsername = new TextBox
            {
                Name = "txtUsername",
                Location = new Point(165, 86),
                Width = 230
            };

            // Mat khau
            Label lblPassword = new Label
            {
                Text = "Mật khẩu:",
                AutoSize = true,
                Location = new Point(35, 135)
            };

            txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(165, 131),
                Width = 230,
                UseSystemPasswordChar = true
            };

            // Hien mat khau
            chkShowPassword = new CheckBox
            {
                Text = "Hiện mật khẩu",
                AutoSize = true,
                Location = new Point(165, 165)
            };

            chkShowPassword.CheckedChanged +=
                ChkShowPassword_CheckedChanged;

            // Nut dang nhap
            btnLogin = new Button
            {
                Name = "btnLogin",
                Text = "Đăng nhập",
                Location = new Point(100, 220),
                Size = new Size(110, 38)
            };

            btnLogin.Click += BtnLogin_Click;

            // Nut thoat
            btnExit = new Button
            {
                Name = "btnExit",
                Text = "Thoát",
                Location = new Point(235, 220),
                Size = new Size(110, 38)
            };

            btnExit.Click += BtnExit_Click;

            AcceptButton = btnLogin;
            CancelButton = btnExit;

            // Them control vao form
            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(chkShowPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnExit);

            Load += Form1_Load;
        }

        // Su kien khi form duoc mo
        private void Form1_Load(object? sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        // An hoac hien mat khau
        private void ChkShowPassword_CheckedChanged(
            object? sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !chkShowPassword.Checked;
        }

        // Bam nut thoat
        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        // Bam nut dang nhap
        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            errorProvider.Clear();

            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider.SetError(
                    txtUsername,
                    "Vui lòng nhập tên đăng nhập.");

                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider.SetError(
                    txtPassword,
                    "Vui lòng nhập mật khẩu.");

                valid = false;
            }

            if (!valid)
                return;

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                        SELECT COUNT(*)
                        FROM dbo.TaiKhoan
                        WHERE Username = @Username
                          AND PasswordHash = @PasswordHash";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.Add(
                            "@Username",
                            SqlDbType.NVarChar,
                            50
                        ).Value = txtUsername.Text.Trim();

                        cmd.Parameters.Add(
                            "@PasswordHash",
                            SqlDbType.VarChar,
                            64
                        ).Value = HashPassword(txtPassword.Text);

                        int count = Convert.ToInt32(
                            cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show(
                                "Đăng nhập thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(
                                "Tên đăng nhập hoặc mật khẩu không đúng!",
                                "Đăng nhập thất bại",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            txtPassword.Clear();
                            txtPassword.Focus();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Lỗi kết nối cơ sở dữ liệu!\n\n" + ex.Message,
                    "Lỗi SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Đã xảy ra lỗi!\n\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Bam mat khau SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(
                    Encoding.Unicode.GetBytes(password));

                StringBuilder sb = new StringBuilder();

                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
