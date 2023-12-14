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
using System.Data.SqlClient;

namespace FantasyAdventures
{
    public partial class GameComplete : LostForm
    {
        public GameComplete()
        {
            InitializeComponent();
        }

        // Hàm vẽ nền
        private void GameComplete_Paint(object sender, PaintEventArgs e)
        {
            if (Player.SelectedMap.CapDo == 1)
            {
                e.Graphics.DrawImage(Properties.Resources.map1, 0, 0);
            }
            else if (Player.SelectedMap.CapDo == 2)
            {
                e.Graphics.DrawImage(Properties.Resources.map2, 0, 0);
            }
            else if (Player.SelectedMap.CapDo == 3)
            {
                e.Graphics.DrawImage(Properties.Resources.map3, 0, 0);
            }
        }

        private void GameComplete_Load(object sender, EventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlCommand;
                SqlCommand command;
                string contentHistory;
                // Cập nhật lịch sử
                contentHistory = $"username vừa hoàn thành game tại map: {Player.SelectedMap.Ten}";
                sqlCommand =
                    $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                command = Database.CreateCommand(sqlCommand);
                command.ExecuteNonQuery();
                // Cập nhật người chơi
                sqlCommand =
                    $"update PLAYER set CAPDO = CAPDO + '1' where TAIKHOAN = '{Player.UserName}'";
                command = Database.CreateCommand(sqlCommand);
                command.ExecuteNonQuery();

                Player.CapDo++;
                ControlCharacter.FormMainGame.UpdateListMapForUser();
                ControlCharacter.FormInitialGame.KhoiTaoListMapSelect();

                int getIDCharacterSelected = Player.SelectedCharacter.Id;

                picAvatar.BackgroundImage = Character.GetImageCharacterAvatar(
                    getIDCharacterSelected
                );

                lblCoin.Text = $"{ControlCharacter.Coin}/{Player.SelectedMap.CoinTarget}";
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        #region Sự kiện Click
        private void picButtonTrangChu_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                if (ControlCharacter.FormStartGame != null)
                {
                    ControlCharacter.FormStartGame.IsConfirmExit = true;
                    ControlCharacter.FormStartGame.Close();
                    ControlCharacter.FormInitialGame.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #endregion
    }
}
