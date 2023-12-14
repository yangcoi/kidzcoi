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
    public partial class GameDialog : LostForm
    {
        int _type = 0; // 0: information, 1: error
        string _content = "";
        public GameDialog()
        {
            InitializeComponent();
        }
        public void SetState(int type, string content)
        {
            _type = type;
            _content = content;
        }

        private void GameDialog_Load(object sender, EventArgs e)
        {
            if (_type == 0)
            {
                picTypeDialog.Image = Properties.Resources.information;
                this.BackColor = Color.MediumTurquoise;
            }
            else if (_type == 1)
            {
                picTypeDialog.Image = Properties.Resources.ERROR;
                this.BackColor = Color.LightCoral;
            }
            txtContentDialog.Text = _content;

        }
        // Hàm vẽ nền
        private void GameDialog_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.back_main_game, 0, 0, this.Width, this.Height);

        }
        #region Sự kiện Click
        private void buttonQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
