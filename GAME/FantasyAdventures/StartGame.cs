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

namespace FantasyAdventures
{
    public partial class StartGame : LostForm
    {
        Timer _timerGame;
        Timer _timeCloud;

        // b1: vị trí tọa độ x của tấm nền 1
        // b2: vị trí tọa độ x của tấm nền 2
        int b1 = 0,
            b2 = 1176;

        // Tốc độ di chuyển mây trong nền
        int _cloudSpeed = 10;
        Random _random = new Random();

        // Hộp quà xuất hiện trên màn hình
        Item _item = null;
        bool _isConfirmExit = false;
        public bool IsConfirmExit
        {
            set { _isConfirmExit = value; }
        }

        public StartGame()
        {
            InitializeComponent();
            _timerGame = new Timer();
            _timerGame.Tick += timerGame_Tick;
            _timerGame.Interval = 100;
            _timerGame.Enabled = false;

            _timeCloud = new Timer();
            _timeCloud.Tick += timerCloud_Tick;
            _timeCloud.Interval = 100;
            _timeCloud.Enabled = false;
        }

        #region Các hàm khởi tạo

        private void StartGame_Load(object sender, EventArgs e)
        {
            ControlCharacter.FormStartGame = this;
            ListQuestion.InitialListQuestionMap();
            _timerGame.Start();
            _timeCloud.Start();
            // Vị trị khởi tạo của nhân vật
            picCharacter.Left = ControlCharacter.PositionLeft;
            picCharacter.Top = ControlCharacter.PositionTop;
            // Tạo mới một item rồi thêm vào màn hình
            Item inititalItem = new Item();
            _item = inititalItem;
            this.Controls.Add(inititalItem.CreateItem());
            // Hiển thị thông tin nhân vật lên màn hình
            HienThiNhanVatLuaChon();
        }

        // Hàm vẽ nền: vẽ 2 nền liên tiếp nhau
        private void StartGame_Paint(object sender, PaintEventArgs e)
        {
            if (Player.SelectedMap.CapDo == 1)
            {
                e.Graphics.DrawImage(Properties.Resources.nentroi2, b1, 0);
                e.Graphics.DrawImage(Properties.Resources.nentroi2, b2, 0);
            }
            else if (Player.SelectedMap.CapDo == 2)
            {
                e.Graphics.DrawImage(Properties.Resources.map2, b1, 0);
                e.Graphics.DrawImage(Properties.Resources.map2, b2, 0);
            }
            else if (Player.SelectedMap.CapDo == 3)
            {
                e.Graphics.DrawImage(Properties.Resources.map3, b1, 0);
                e.Graphics.DrawImage(Properties.Resources.map3, b2, 0);
            }
        }
        #endregion
        #region Các hàm timer

        private void timerGame_Tick(object sender, EventArgs e)
        {
            BackgroundMove();
            ItemMove();
            ChoiLai();
            VeTrangChu();
        }

        private void timerCloud_Tick(object sender, EventArgs e)
        {
            CloudMove();
        }
        #endregion
        #region Hàm di chuyển tới khi nhấn phím mũi tên phải

        private void StartGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                ControlCharacter.IsRight = true;
            }
        }

        private void StartGame_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                ControlCharacter.IsRight = false;
            }
        }
        #endregion
        #region Các sự kiện Click

        private void picButtonLuaChon_Click(object sender, EventArgs e)
        {
            MenuLuaChonGame fMenuLuaChonGame = new MenuLuaChonGame();
            fMenuLuaChonGame.ShowDialog();
        }
        #endregion
        #region Các hàm phục vụ
        bool ConfirmOutGame()
        {
            DialogResult r;
            r = MessageBox.Show(
                $"Bạn có muốn thoát hay không?",
                "Cảnh báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (r == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }

        void CloudMove()
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "cloud")
                {
                    x.Left -= _cloudSpeed;

                    if (x.Left < -100)
                    {
                        x.Left = _random.Next(300, 1100);
                    }
                }
            }
        }

        void VeTrangChu()
        {
            if (ControlCharacter.IsGoHome == true)
            {
                _isConfirmExit = true;
                ControlCharacter.IsGoHome = false;
                this.Close();
                ControlCharacter.FormInitialGame.Close();
            }
        }

        void ChoiLai()
        {
            if (ControlCharacter.IsPlayAgain == true)
            {
                _isConfirmExit = true;
                ControlCharacter.IsPlayAgain = false;
                this.Close();
            }
        }

        void HienThiNhanVatLuaChon()
        {
            int getIDCharacterSelected = Player.SelectedCharacter.Id;
            picCharacter.Image = Character.GetImageCharacterWalk(getIDCharacterSelected);
            picAvatar.BackgroundImage = Character.GetImageCharacterAvatar(getIDCharacterSelected);
        }

        // Hàm tạo mới Item
        void CreateNewItem()
        {
            if (_item == null)
            {
                int leftPositionItem = _random.Next(200, 400);
                Item newItem = new Item(leftPositionItem);
                _item = newItem;
                this.Controls.Add(newItem.CreateItem());
            }
        }

        // Hàm update game khi trả lời xong câu hỏi
        public void UpdateGame()
        {
            if (ControlCharacter.CurrentQuestion != null && ControlCharacter.IsCompletedQA)
            {
                if (!ControlCharacter.IsSelectedRightAnswer)
                {
                    ControlCharacter.Mang = ControlCharacter.Mang - 1;
                }

                ControlCharacter.IsCompletedQA = false;
                ControlCharacter.CurrentQuestion = null;
                ControlCharacter.IsSelectedRightAnswer = false;
            }
        }

        // Hàm hiển thị mạng
        void HienThiMang()
        {
            if (ControlCharacter.Mang == 2)
            {
                this.Controls.Remove(picTim3);
            }
            else if (ControlCharacter.Mang == 1)
            {
                this.Controls.Remove(picTim3);
                this.Controls.Remove(picTim2);
            }
            // Nếu mạng = 0 thì Game Over
            else if (ControlCharacter.Mang == 0)
            {
                this.Controls.Remove(picTim3);
                this.Controls.Remove(picTim2);
                this.Controls.Remove(picTim1);
                _timeCloud.Stop();
                _timerGame.Stop();
                _isConfirmExit = true;
                GameOver fGameOver = new GameOver();
                fGameOver.ShowDialog();
                this.Close();
            }
        }

        // Hàm hiển thị Coin
        void HienThiCoin()
        {
            lblCoin.Text = $"{ControlCharacter.Coin}/{Player.SelectedMap.CoinTarget}";
            // Nếu vượt qua Coin Target của Map thì sẽ hoàn thành game
            if (ControlCharacter.Coin >= Player.SelectedMap.CoinTarget)
            {
                _timeCloud.Stop();
                _timerGame.Stop();
                _isConfirmExit = true;
                GameComplete fGameComplete = new GameComplete();
                fGameComplete.ShowDialog();
                this.Close();
            }
        }

        // Hàm di chuyển Item
        void ItemMove()
        {
            if (ControlCharacter.IsRight)
            {
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && (string)x.Tag == "item")
                    {
                        x.Left -= _cloudSpeed;

                        if (x.Left < -100)
                        {
                            this.Controls.Remove(x);
                            _item = null;
                            CreateNewItem();
                        }
                        // Nếu nhân vật chạm với item thì bắt đầu trả lời câu hỏi
                        if (x.Bounds.IntersectsWith(picCharacter.Bounds) && _item != null)
                        {
                            this.Controls.Remove(x);
                            _item = null;
                            CreateNewItem();
                            ControlCharacter.IsRight = false;
                            ControlCharacter.IsSelectedRightAnswer = false;
                            QuestionForm fQuestion = new QuestionForm();
                            ControlCharacter.CurrentQuestion = null;
                            ControlCharacter.IsCompletedQA = false;
                            ControlCharacter.IsSelectedRightAnswer = false;

                            fQuestion.ShowDialog();
                        }
                    }
                }
            }
            HienThiMang();
            HienThiCoin();
        }

        // Hàm di chuyển Background
        void BackgroundMove()
        {
            if (b1 < -1175)
            {
                b1 = 1160;
            }
            if (b2 < -1175)
            {
                b2 = 1150;
            }
            // Di chuyển nhân vật
            if (ControlCharacter.IsRight)
            {
                b1 -= (ControlCharacter.SpeedWalker * 2);
                b2 -= (ControlCharacter.SpeedWalker * 2);
            }
            // Vẽ lại nền
            Invalidate();
        }

        #endregion

        private void StartGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isConfirmExit)
            {
                if (ConfirmOutGame())
                {
                    _timerGame.Stop();
                    _timeCloud.Stop();
                    ControlCharacter.FormStartGame = null;
                    ControlCharacter.ResetGame();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                _timerGame.Stop();
                _timeCloud.Stop();
                ControlCharacter.FormStartGame = null;
                ControlCharacter.ResetGame();
            }
        }
    }
}
