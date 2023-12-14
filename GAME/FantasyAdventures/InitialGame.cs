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
    public partial class InitialGame : LostForm
    {
        //Danh sách nhân vật mà người chơi có thể chọn
        List<Character> _listCharacter;
        //Danh sách map mà người chơi có thể chọn
        List<Map> _listMap;
        public InitialGame()
        {
            InitializeComponent();
        }
        #region Các hàm khởi tạo
        private void InitialGame_Load(object sender, EventArgs e)
        {
            try
            {
                ControlCharacter.FormInitialGame = this;
                KhoiTaoListCharacterSelect();
                KhoiTaoListMapSelect();
                Player.SelectedMap = _listMap.ElementAt(0);
                HienThiNhanVatLuaChon();
                HienThiMapLuaChon();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        // Khởi tạo danh sách nhân vật mà người chơi có thể chọn (đã sở hữu)
        void KhoiTaoListCharacterSelect()
        {
            _listCharacter = new List<Character>(Player.ListCharacter.Count);
            foreach (var item in Player.ListCharacter)
            {
                _listCharacter.Add(item);
            }
        }

        public void KhoiTaoListMapSelect()
        {
            _listMap = null;
            _listMap = new List<Map>(Player.ListMap.Count);
            foreach (var item in Player.ListMap)
            {
                _listMap.Add(item);
            }
        }
        #endregion
        #region Các sự kiện Click Button
        // Sự kiên Button chọn nhân vật tiếp theo
        private void picNextButton_Click(object sender, EventArgs e)
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

        // Sự kiên Button chọn nhân vật trước đó
        private void picPreviousButton_Click(object sender, EventArgs e)
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

        private void label3_Click(object sender, EventArgs e) { }

        // Sự kiên Button chọn map tiếp theo
        private void picNextButtonMap_Click(object sender, EventArgs e)
        {
            int indexMapLuaChon = TimIndexMapLuaChon();
            if (indexMapLuaChon == _listMap.Count - 1)
            {
                SetMapLuaChon(0);
            }
            else
            {
                SetMapLuaChon(indexMapLuaChon + 1);
            }
        }

        // Sự kiên Button chọn map trước đó
        private void picPreviousButtonMap_Click(object sender, EventArgs e)
        {
            int indexMapLuaChon = TimIndexMapLuaChon();
            if (indexMapLuaChon == 0)
            {
                SetMapLuaChon(_listMap.Count - 1);
            }
            else
            {
                SetMapLuaChon(indexMapLuaChon - 1);
            }
        }

        // Sự kiện click Button Bắt đầu
        private void btnBatDau_Click(object sender, EventArgs e)
        {
            ControlCharacter.FormMainGame.HienThiNhanVatLuaChon();
            ControlCharacter.LuotChonLai = Player.SelectedCharacter.LuotTraLoiThem;
            ControlCharacter.SpeedWalker = Player.SelectedCharacter.TocDo;
            ControlCharacter.MayMan = Player.SelectedCharacter.MayMan;
            StartGame fStartGame = new StartGame();
            fStartGame.ShowDialog();
        }
        #endregion
        #region Các hàm phục vụ 
        int TimIndexNhanVatLuaChon()
        {
            int index = -1;
            int i = 0;
            foreach (var item in _listCharacter)
            {
                if (item.Id == Player.SelectedCharacter.Id)
                {
                    index = i;
                    break;
                }
                i++;
            }
            return index;
        }
        int TimIndexMapLuaChon()
        {
            int index = -1;
            int i = 0;
            foreach (var item in _listMap)
            {
                if (item.Id == Player.SelectedMap.Id)
                {
                    index = i;
                    break;
                }
                i++;
            }
            return index;
        }

        // Hàm hiển thị thông tin nhân vật đang chọn
        void HienThiNhanVatLuaChon()
        {
            txtGioiThieuNhanVat.Text = Player.SelectedCharacter.MoTa + "\n";
            txtGioiThieuNhanVat.Text =
                txtGioiThieuNhanVat.Text
                + $"Tốc độ chạy: {Player.SelectedCharacter.TocDo}\nLượt trả lời thêm: {Player.SelectedCharacter.LuotTraLoiThem}\nMay mắn: {Player.SelectedCharacter.MayMan}%";
            lblTenCharacter.Text = Player.SelectedCharacter.Ten;
            int getIDCharacterSelected = Player.SelectedCharacter.Id;
            picSelectedCharacter.Image = Character.GetImageCharacter(getIDCharacterSelected);
        }

        // Hàm hiển thị thông tin map đang chọn
        void HienThiMapLuaChon()
        {
            txtGioiThieuBanDo.Text = Player.SelectedMap.MoTa + "\n";
            txtGioiThieuBanDo.Text =
                txtGioiThieuBanDo.Text
                + $"Cấp độ: {Player.SelectedMap.CapDo} \nCoin target: {Player.SelectedMap.CoinTarget}";
            lblTenMap.Text = Player.SelectedMap.Ten;
            int getIDMapSelected = Player.SelectedMap.Id;
            picSelectedMap.Image = Map.GetImageMap(getIDMapSelected);
        }

        void SetNhanVatLuaChon(int index)
        {
            Character nhanVatLuaChon = _listCharacter.ElementAt(index);
            Player.SetSelectedCharacter(nhanVatLuaChon);
            ControlCharacter.LuotChonLai = Player.SelectedCharacter.LuotTraLoiThem;
            ControlCharacter.SpeedWalker = Player.SelectedCharacter.TocDo;
            ControlCharacter.MayMan = Player.SelectedCharacter.MayMan;
            HienThiNhanVatLuaChon();
        }

        void SetMapLuaChon(int index)
        {
            Map mapLuaChon = _listMap.ElementAt(index);
            Player.SetSelectedMap(mapLuaChon);
            HienThiMapLuaChon();
        }
        #endregion
        private void InitialGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            ControlCharacter.FormInitialGame = null;
        }
    }
}
