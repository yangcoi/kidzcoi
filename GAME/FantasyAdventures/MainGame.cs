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
using System.Threading;
using System.Data.SqlClient;

namespace FantasyAdventures
{
    public partial class MainGame : LostForm
    {
        System.Windows.Forms.Timer _timerMoveObject;
        int _cloudSpeed = 15;

        public MainGame()
        {
            InitializeComponent();
            _timerMoveObject = new System.Windows.Forms.Timer();
            _timerMoveObject.Tick += timerMoveObject_Tick;
            _timerMoveObject.Interval = 100;
            _timerMoveObject.Enabled = false;
        }

        #region Các hàm khởi tạo
        private void MainGame_Load(object sender, EventArgs e)
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
                Database.CreateConnection();
                List_Map.ListMap.Clear();
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
                ListQuestion.InitialListAllQuestion();
                ListQuestion.InitialListQuestionLuyenTap();
                ControlCharacter.FormMainGame = this;
                // Update List Character For User
                UpdateListCharacterForUser();
                UpdateListMapForUser();
                HienThiNhanVatLuaChon();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        // Hàm cập nhật List Character cho Player
        void UpdateListCharacterForUser()
        {
            Player.ListCharacter.Clear();
            Database.CreateConnection();
            string sqlCommand =
                $"select A.TAIKHOAN, A.CHARACTER_ID, A.THOIGIAN, B.TEN, B.MOTA from PLAYER_CHARACTER A, CHARACTER B where A.TAIKHOAN = '{Player.UserName}' and A.CHARACTER_ID = B.ID";
            SqlCommand command = Database.CreateCommand(sqlCommand);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                Character getCharacter = List_Character.GetCharacter((int)rd["CHARACTER_ID"]);
                if (getCharacter != null)
                {
                    Player.AddCharacter(getCharacter);
                }
            }
        }

        // Hàm cập nhật Map cho Player
        public void UpdateListMapForUser()
        {
            Player.ListMap.Clear();
            int playerLevel = Player.CapDo;
            for (int i = playerLevel; i >= 1; i--)
            {
                Map getMap = List_Map.GetMap(i);
                if (getMap != null)
                {
                    Player.AddMap(getMap);
                }
            }
        }

        // Hàm hiển thị thông tin của Player
        public void HienThiNhanVatLuaChon()
        {
            lblLevel.Text = $"Level: {Player.CapDo}";
            lblCoin.Text = $"x{Player.Coin}";
            int getIDCharacterSelected = Player.SelectedCharacter.Id;
            picSelectedCharacter.Image = Character.GetImageCharacter(getIDCharacterSelected);
        }
        #endregion
        #region Các hàm tạo nền
        // Hàm vẽ nền
        private void MainGame_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(
                Properties.Resources.background_maingame,
                0,
                0,
                this.Width,
                this.Height
            );
        }

        private void timerMoveObject_Tick(object sender, EventArgs e)
        {
            MoveCloud();
        }

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
        #region Các sự kiện Click
        private void btnBangXepHang_Click(object sender, EventArgs e)
        {
            BangXepHang fBangXepHang = new BangXepHang();
            fBangXepHang.ShowDialog();
        }

        private void btnLichSuHocTap_Click(object sender, EventArgs e)
        {
            DanhSachTuVung fDanhSachTuVung = new DanhSachTuVung();
            fDanhSachTuVung.ShowDialog();
        }

        private void btnShop_Click(object sender, EventArgs e)
        {
            ShopCharacters fShopCharacters = new ShopCharacters();
            fShopCharacters.ShowDialog();
        }

        private void btnLuyenTap_Click(object sender, EventArgs e)
        {
            LuyenTapForm fLuyenTapForm = new LuyenTapForm();
            fLuyenTapForm.ShowDialog();
        }

        private void picBtnThoatGame_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnVaoChoi_Click(object sender, EventArgs e)
        {
            InitialGame fInitial = new InitialGame();
            fInitial.ShowDialog();
        }

        private void picBtnHuongDan_Click(object sender, EventArgs e)
        {
            GuideForm fGuideForm = new GuideForm();
            fGuideForm.ShowDialog();
        }
        #endregion
        private void MainGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            ControlCharacter.FormMainGame = null;
        }

    }
}
