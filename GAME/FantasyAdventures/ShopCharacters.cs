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
    public partial class ShopCharacters : LostForm
    {
        List<Character> _listCharacter = List_Character.ListCharacter;
        Character _characterSelected = null;

        public ShopCharacters()
        {
            InitializeComponent();
            if (_listCharacter.Count > 0)
            {
                _characterSelected = _listCharacter.ElementAt(0);
            }
        }

        private void ShopCharacters_Load(object sender, EventArgs e)
        {
            HienThiNhanVatLuaChon();
        }

        #region Các hàm phục vụ

        bool ConfirmBox()
        {
            DialogResult r;
            r = MessageBox.Show(
                $"Bạn có muốn mua nhân vật này hay không?",
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

        int TimIndexNhanVatLuaChon()
        {
            int index = -1;
            int i = 0;
            foreach (var item in _listCharacter)
            {
                if (item.Id == _characterSelected.Id)
                {
                    index = i;
                    break;
                }
                i++;
            }
            return index;
        }

        bool CheckIsBoughtCharacter(int ID)
        {
            bool check = false;
            Database.CreateConnection();
            string sqlCommand;
            SqlCommand command;
            sqlCommand =
                $"select * from PLAYER_CHARACTER where TAIKHOAN = '{Player.UserName}' and CHARACTER_ID = '{ID}'";
            command = Database.CreateCommand(sqlCommand);
            SqlDataReader rd = command.ExecuteReader();
            while (rd.Read())
            {
                check = true;
            }
            return check;
        }

        void HienThiNhanVatLuaChon()
        {
            if (CheckIsBoughtCharacter(_characterSelected.Id))
            {
                picBought.Visible = true;
                btnMua.Visible = false;
            }
            else
            {
                picBought.Visible = false;
                btnMua.Visible = true;
            }
            txtGioiThieuNhanVat.Text = _characterSelected.MoTa + "\n";
            lblTenCharacter.Text = _characterSelected.Ten;
            lblCoin.Text = $"{_characterSelected.Coin}/{Player.Coin}";
            lblTocDo.Text = $"Tốc độ: {_characterSelected.TocDo}";
            lblMayMan.Text = $"May mắn: {_characterSelected.MayMan}";
            lblLuotTraLoiThem.Text = $"Lượt trả lời thêm: {_characterSelected.LuotTraLoiThem}";
            picSelectedCharacter.Image = Character.GetImageCharacter(_characterSelected.Id);
        }

        void SetNhanVatLuaChon(int index)
        {
            _characterSelected = _listCharacter.ElementAt(index);

            HienThiNhanVatLuaChon();
        }
        #endregion
        #region Các sự kiện Click
        private void picNextButton_Click(object sender, EventArgs e)
        {
            try
            {
                int indexNhanVatLuaChon = TimIndexNhanVatLuaChon();
                if (indexNhanVatLuaChon == _listCharacter.Count - 1)
                {
                    SetNhanVatLuaChon(0);
                }
                else
                {
                    SetNhanVatLuaChon(indexNhanVatLuaChon + 1);
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void picPreviousButton_Click(object sender, EventArgs e)
        {
            try
            {
                int indexNhanVatLuaChon = TimIndexNhanVatLuaChon();
                if (indexNhanVatLuaChon == 0)
                {
                    SetNhanVatLuaChon(_listCharacter.Count - 1);
                }
                else
                {
                    SetNhanVatLuaChon(indexNhanVatLuaChon - 1);
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void btnMua_Click(object sender, EventArgs e)
        {
            try
            {
                if (ConfirmBox())
                {
                    if (CheckIsBoughtCharacter(_characterSelected.Id))
                    {
                        throw new Exception("Bạn đã mua nhân vật này rồi");
                    }
                    if (Player.Coin < _characterSelected.Coin)
                    {
                        throw new Exception("Bạn không đủ Coin để mua nhân vật này");
                    }
                    Player.Coin -= _characterSelected.Coin;
                    Database.CreateConnection();
                    string sqlCommand;
                    SqlCommand command;
                    string contentHistory;
                    // Cập nhật lịch sử
                    contentHistory =
                        $"username vừa mua thành công nhân vật: {_characterSelected.Ten}";
                    sqlCommand =
                        $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    // Cập nhật người chơi
                    sqlCommand =
                        $"update PLAYER set COIN = COIN - '{_characterSelected.Coin}' where TAIKHOAN = '{Player.UserName}'";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();

                    sqlCommand =
                        $"insert into PLAYER_CHARACTER (TAIKHOAN, CHARACTER_ID) values ('{Player.UserName}', '{_characterSelected.Id}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    // Thêm nhân vật đã mua vào danh sách Nhân Vật của Player
                    Player.AddCharacter(_characterSelected);
                    // Cập nhật lại thông tin nhân vật
                    HienThiNhanVatLuaChon();
                    ControlCharacter.FormMainGame.HienThiNhanVatLuaChon();
                    GameDialog fGameDialog = new GameDialog();
                    fGameDialog.SetState(0, "Mua thành công");
                    fGameDialog.ShowDialog();
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


    }
}
