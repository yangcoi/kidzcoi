using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Util;
using System.Data.SqlClient;
using BC = BCrypt.Net.BCrypt;

namespace FantasyAdventures
{
    public partial class Login : LostForm
    {
        Timer _timerMoveObject;
        int _cloudSpeed = 15;

        public Login()
        {
            InitializeComponent();
            _timerMoveObject = new System.Windows.Forms.Timer();
            _timerMoveObject.Tick += timerMoveObject_Tick;
            _timerMoveObject.Interval = 100;
            _timerMoveObject.Enabled = false;
        }
        private void Login_Load(object sender, EventArgs e)
        {
            _timerMoveObject.Start();
        }
        #region Các hàm tạo nền
        // Hàm vẽ nền cho Form
        private void Login_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.country_platform, 0, 0, this.Width, this.Height);
        }
        private void timerMoveObject_Tick(object sender, EventArgs e)
        {
            MoveCloud();
        }
        // Hàm di chuyển mây trong nền
        void MoveCloud()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "cloud")
                {
                    x.Left -= _cloudSpeed;

                    if (x.Left < -100)
                    {
                        x.Left = this.Width;
                    }
                }
            }
        }
        #endregion
        #region Các sự kiện Click Button
        private void buttonDangKy_Click(object sender, EventArgs e)
        {
            try
            {
                string taiKhoan = txtTaiKhoan.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }

                Database.CreateConnection();
                string sqlCommand =
                    $"select COUNT(*) as SOLUONG from PLAYER where TAIKHOAN = '{taiKhoan}'";
                SqlCommand command = Database.CreateCommand(sqlCommand);
                int check = (int)command.ExecuteScalar();
                // Kiểm tra xem có tồn tại tài khoản chưa
                if (check == 0)
                {
                    // Hash password và insert người chơi mới vào database
                    string passwordHash = BC.HashPassword(matKhau);
                    sqlCommand =
                        $"insert into PLAYER (TAIKHOAN, MATKHAU, NHANVAT) values ('{taiKhoan}', '{passwordHash}', '1')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    // Insert danh sách nhân vật của người chơi
                    sqlCommand =
                        $"insert into PLAYER_CHARACTER (TAIKHOAN, CHARACTER_ID) values ('{taiKhoan}', '1')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    // Hộp thoại thông báo cho người dùng
                    GameDialog fGameDialog = new GameDialog();
                    fGameDialog.SetState(0, "Đăng ký thành công, vui lòng đăng nhập");
                    fGameDialog.ShowDialog();
                }
                else
                {
                    throw new Exception("Tài khoản đã có người đăng ký!");
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            try
            {
                string taiKhoan = txtTaiKhoan.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }

                Database.CreateConnection();
                string sqlCommand = $"select * from PLAYER where TAIKHOAN = '{taiKhoan}'";
                SqlCommand command = Database.CreateCommand(sqlCommand);
                bool check = false;
                SqlDataReader reader = command.ExecuteReader();
                string passwordDatabase = "";
                int capDo = 0;
                int coin = 0;
                int selectedCharacter = 0;
                int quyenHan = 0;
                // Lấy thông tin người chơi
                while (reader.Read())
                {
                    check = true;
                    passwordDatabase = reader["MATKHAU"].ToString();
                    capDo = (int)reader["CAPDO"];
                    coin = (int)reader["COIN"];
                    selectedCharacter = (int)reader["NHANVAT"];
                    quyenHan = (int)reader["QUYENHAN"];
                }
                // Kiểm tra xem có tồn tại người chơi hay không
                if (check == true)
                {
                    bool passwordCheck = BC.Verify(matKhau, passwordDatabase);
                    // Kiểm tra password nhập với password đã được mã hóa
                    if (passwordCheck)
                    {
                        // Tìm nhân vật mà người chơi đã chọn
                        Character getCharacter = List_Character.GetCharacter(selectedCharacter);
                        if (getCharacter == null)
                        {
                            // Nếu không tồn tại nhân vật đó thì sẽ lấy nhân vật mặc định (với ID = 1)
                            getCharacter = List_Character.GetCharacter(1);
                        }
                        // Set các thuộc tính cho lớp Player
                        Player.SetPlayer(taiKhoan, capDo, coin, getCharacter);
                        Player.QuyenHan = quyenHan;
                        GameDialog fGameDialog = new GameDialog();
                        fGameDialog.SetState(0, "Đăng nhập thành công");
                        fGameDialog.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        throw new Exception(
                            "Tài khoản không tồn tại hoặc mật khẩu không chính xác!"
                        );
                    }
                }
                else
                {
                    throw new Exception("Tài khoản không tồn tại hoặc mật khẩu không chính xác!");
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }
        #endregion
        #region Các ràng buộc input
        // Ngăn không cho người chơi đặt tên tài khoản có chứa dấu cách
        private void txtTaiKhoan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Space))
            {
                e.Handled = true;
            }
        }
        // Ngăn không cho người chơi đặt mật khẩu có chứa dấu cách
        private void txtMatKhau_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Space))
            {
                e.Handled = true;
            }
        }
        #endregion
    }
}
