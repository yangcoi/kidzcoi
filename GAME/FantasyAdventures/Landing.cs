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

namespace FantasyAdventures
{
    public partial class Landing : LostForm
    {
        Timer _timerCheckIsLogin;
        Timer _timerMoveObject;
        int _cloudSpeed = 15;
        int _characterSpeed = 10;

        public Landing()
        {
            InitializeComponent();
            _timerCheckIsLogin = new Timer();
            _timerCheckIsLogin.Tick += timerCheckIsLogin_Tick;
            _timerCheckIsLogin.Interval = 1000;
            _timerCheckIsLogin.Enabled = false;

            _timerMoveObject = new Timer();
            _timerMoveObject.Tick += timerMoveObject_Tick;
            _timerMoveObject.Interval = 100;
            _timerMoveObject.Enabled = false;
        }

        private void Landing_Load(object sender, EventArgs e)
        {
            try
            {
                _timerMoveObject.Start();
                // Khởi tạo danh sách CHARACTER
                List_Character.ListCharacter.Clear();
                Database.CreateConnection();
                string sqlCommand = $"select * from CHARACTER";
                SqlCommand command = Database.CreateCommand(sqlCommand);
                SqlDataReader rd = command.ExecuteReader();
                while (rd.Read())
                {
                    Character newCharacter = new Character(
                        (int)rd["ID"],
                        (string)rd["TEN"],
                        (string)rd["MOTA"],
                        (int)rd["COIN"],
                        (int)rd["TOCDO"],
                        (int)rd["MAYMAN"],
                        (int)rd["LUOTTRALOITHEM"]
                    );
                    List_Character.Add(newCharacter);
                }

                // Khởi tạo danh sách MAP
                List_Map.ListMap.Clear();
                Database.CreateConnection();
                sqlCommand = $"select * from MAP";
                command = Database.CreateCommand(sqlCommand);
                rd = command.ExecuteReader();
                while (rd.Read())
                {
                    Map newMap = new Map(
                        (int)rd["ID"],
                        (string)rd["TEN"],
                        (string)rd["MOTA"],
                        (int)rd["CAPDO"],
                        (int)rd["COIN_RANDOM1"],
                        (int)rd["COIN_RANDOM2"],
                        (int)rd["COIN_TARGET"]
                    );
                    List_Map.Add(newMap);
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        #region Các hàm tạo nền
        // Hàm vẽ nền background
        private void Landing_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.nentroi2, 0, 0, this.Width, this.Height);
        }

        // Hàm di chuyển Cloud trong nền
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

        // Hàm di chuyển Character trong nền
        void MoveCharacter()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "character")
                {
                    x.Left += _characterSpeed;

                    if (x.Left > this.Width)
                    {
                        x.Left = 0;
                    }
                }
            }
        }

        // Timer di chuyển các đối tượng
        private void timerMoveObject_Tick(object sender, EventArgs e)
        {
            MoveCloud();
            MoveCharacter();
        }

        // Timer check xem đã login hay chưa
        private void timerCheckIsLogin_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Player.IsLogin())
                {
                    _timerCheckIsLogin.Stop();
                    btnVaoGame.Visible = true;
                    btnDangXuat.Visible = true;
                    lblXinChao.Text = $"Xin chào: {Player.UserName}";
                    lblXinChao.Visible = true;
                    btnChoiNgay.Visible = false;
                    if (Player.QuyenHan >= 1)
                    {
                        btnAdminPanel.Visible = true;
                    }
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
        #region Các sự kiện Click

        private void btnChoiNgay_Click(object sender, EventArgs e)
        {
            try
            {
                _timerCheckIsLogin.Start();
                // Mở form Login
                Login fLogin = new Login();
                fLogin.ShowDialog();
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            try
            {
                if (Player.IsLogin())
                {
                    // Reset các thuộc tính static của lớp Player
                    Player.Destroy();
                    _timerCheckIsLogin.Stop();
                    btnVaoGame.Visible = false;
                    btnDangXuat.Visible = false;
                    lblXinChao.Text = $"Xin chào: {Player.UserName}";
                    lblXinChao.Visible = false;
                    btnChoiNgay.Visible = true;
                    btnAdminPanel.Visible = false;
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void btnVaoGame_Click(object sender, EventArgs e)
        {
            try
            {
                MainGame fMainGame = new MainGame();
                fMainGame.Show();
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void btnAdminPanel_Click(object sender, EventArgs e)
        {
            if (Player.IsLogin() && Player.QuyenHan >= 1)
            {
                AdminForm fAdminForm = new AdminForm();
                fAdminForm.ShowDialog();
            }
        }
        #endregion

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutForm fAboutForm = new AboutForm();
            fAboutForm.ShowDialog();
        }
    }
}
