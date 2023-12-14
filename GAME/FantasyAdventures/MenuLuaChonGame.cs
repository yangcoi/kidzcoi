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
    public partial class MenuLuaChonGame : LostForm
    {
        public MenuLuaChonGame()
        {
            InitializeComponent();
        }
        #region Các sự kiện Click
        private void buttonTiepTuc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void buttonTrangChu_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfirmOutGame())
                {
                    this.Close();
                    ControlCharacter.FormStartGame.IsConfirmExit = true;
                    ControlCharacter.FormStartGame.Close();
                    ControlCharacter.FormInitialGame.Close();
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void buttonChoiLai_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfirmOutGame())
                {
                    this.Close();
                    ControlCharacter.FormStartGame.IsConfirmExit = true;
                    ControlCharacter.FormStartGame.Close();
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void buttonThoatGame_Click(object sender, EventArgs e)
        {
            if (ConfirmOutGame())
            {
                Environment.Exit(0);
            }
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

        #endregion


    }
}
