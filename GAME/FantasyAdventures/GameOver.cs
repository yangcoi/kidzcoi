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
    public partial class GameOver : LostForm
    {
        public GameOver()
        {
            InitializeComponent();
        }

        private void GameOver_Load(object sender, EventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlCommand;
                SqlCommand command;
                string contentHistory;
                // Cập nhật lịch sử
                contentHistory = $"username vừa gameover tại map: {Player.SelectedMap.Ten}";
                sqlCommand =
                    $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                command = Database.CreateCommand(sqlCommand);
                command.ExecuteNonQuery();
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

        // Hàm vẽ nền
        private void GameOver_Paint(object sender, PaintEventArgs e)
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

        #region Sự kiện click
        private void picButtonChoiLai_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                ControlCharacter.FormStartGame.IsConfirmExit = true;
                ControlCharacter.FormStartGame.Close();
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }
        #endregion
    }
}
