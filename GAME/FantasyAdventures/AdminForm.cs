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
using BC = BCrypt.Net.BCrypt;

namespace FantasyAdventures
{
    public partial class AdminForm : LostForm
    {
        string _currentTaiKhoan = null;
        int _currentQuestionId = -1;
        int _currentCharacterId = -1;
        int _currentMapId = -1;

        public AdminForm()
        {
            InitializeComponent();
        }
        private void AdminForm_Load(object sender, EventArgs e)
        {
            try
            {
                FetchDanhSachNguoiChoi();
                FetchLichSu();
                FetchDanhSachTuVung();
                FetchDanhSachNhanVat();
                FetchDanhSachMap();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #region Các hàm phục vụ
        bool ConfirmBox()
        {
            DialogResult r;
            r = MessageBox.Show(
                $"Bạn có muốn thực hiện hành động này hay không?",
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

        void ResetFormNguoiChoi()
        {
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            _currentTaiKhoan = null;
            picBtnDelete.Visible = false;
            picLblDelete.Visible = false;
        }

        void ResetFormTuVung()
        {
            txtDapAn.Text = "";
            txtCauHoi.Text = "";
            txtCauTraLoi.Text = "";
            txtCauHoiThem.Text = "";
            txtCauTraLoiThem.Text = "";
            txtCapDoThem.Text = "";
            txtCoinThem.Text = "";
            txtLoaiThem.Text = "";
            txtIDCauHoiThem.Text = "";
            _currentQuestionId = -1;
            txtCauHoiChinhSua.Text = "";
            cboDanhSachCauTraLoi.Items.Clear();
            txtCoinChinhSua.Text = "";
            txtCapDoChinhSua.Text = "";
            txtLoaiChinhSua.Text = "";
        }

        void ResetFormNhanVat()
        {
            txtTenNVEdit.Text = "";
            txtMoTaNVEdit.Text = "";
            txtMayManNVEdit.Text = "";
            txtCoinNVEdit.Text = "";
            txtLuotTraLoiNVEdit.Text = "";
            txtTocDoNVEdit.Text = "";
            picSelectedCharacter.Image = null;
            _currentCharacterId = -1;
        }

        void ResetFormMap()
        {
            txtMoTaMapEdit.Text = "";
            txtTenMapEdit.Text = "";
            txtMoTaMapEdit.Text = "";
            txtCoinRandom1.Text = "";
            txtCoinRandom2.Text = "";
            txtCoinTarget.Text = "";
            _currentMapId = -1;
            picSelectedMap.Image = null;
        }

        void FetchDanhSachNguoiChoi()
        {
            ResetFormNguoiChoi();
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                "select count(*) as SOLUONGNHANVAT, A.TAIKHOAN, CAPDO, COIN, NGAYTAO from PLAYER A, PLAYER_CHARACTER B where A.TAIKHOAN = B.TAIKHOAN group by A.TAIKHOAN, CAPDO, COIN, NHANVAT, MATKHAU, NGAYTAO";
            dt = Database.SelectQuery(sqlCommand);
            dgvNguoiChoi.DataSource = dt;
        }

        void FetchLichSu()
        {
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                "select NOIDUNG as NOIDUNG_LS, THOIGIAN as THOIGIAN_LS, TAIKHOAN as TAIKHOAN_LS from HISTORY order by THOIGIAN_LS desc";
            dt = Database.SelectQuery(sqlCommand);
            dgvLichSu.DataSource = dt;
        }

        void FetchDanhSachNhanVat()
        {
            ResetFormNhanVat();
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                "select ID as ID_NV, TEN as TEN_NV, MOTA as MOTA_NV, COIN as COIN_NV, TOCDO as TOCDO_NV, LUOTTRALOITHEM as LUOTTRALOITHEM_NV, MAYMAN as MAYMAN_NV  from CHARACTER";
            dt = Database.SelectQuery(sqlCommand);
            dgvDanhSachNhanVat.DataSource = dt;
        }

        void FetchDanhSachMap()
        {
            ResetFormMap();
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                "select ID as ID_MAP, TEN as TEN_MAP, MOTA as MOTA_MAP, COIN_RANDOM1 as COIN_RANDOM_1_MAP, COIN_RANDOM2 as COIN_RANDOM_2_MAP, COIN_TARGET as COIN_TARGET_MAP, CAPDO as CAPDO_MAP  from MAP";
            dt = Database.SelectQuery(sqlCommand);
            dgvDanhSachMap.DataSource = dt;
        }

        void FetchDanhSachTuVung()
        {
            ResetFormTuVung();
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                "select count(*)  as SOLUONGDAPAN_TV , CAUHOI as CAUHOI_TV, QUESTION_ID as QUESTION_ID_TV , LOAI as LOAI_TV, CAPDO as CAPDO_TV, COIN as COIN_TV from QUESTION A, ANSWER B where A.ID = B.QUESTION_ID group by CAUHOI, QUESTION_ID, LOAI, CAPDO, COIN";
            dt = Database.SelectQuery(sqlCommand);
            dgvTuVung.DataSource = dt;
        }
        #endregion
        #region Các hàm Click
        private void picButtonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlCommand = "";
                SqlCommand command;
                string taiKhoan = txtTaiKhoan.Text.Trim();
                string matKhau = txtMatKhau.Text.Trim();
                if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }
                if (ConfirmBox())
                {
                    sqlCommand =
                        $"select COUNT(*) as SOLUONG from PLAYER where TAIKHOAN = '{taiKhoan}'";
                    command = Database.CreateCommand(sqlCommand);
                    int check = (int)command.ExecuteScalar();
                    if (check == 0)
                    {
                        string passwordHash = BC.HashPassword(matKhau);
                        sqlCommand =
                            $"insert into PLAYER (TAIKHOAN, MATKHAU, NHANVAT) values ('{taiKhoan}', '{passwordHash}', '1')";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();

                        sqlCommand =
                            $"insert into PLAYER_CHARACTER (TAIKHOAN, CHARACTER_ID) values ('{taiKhoan}', '1')";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();

                        MessageBox.Show("Tạo người dùng thành công");
                        FetchDanhSachNguoiChoi();
                    }
                }
                else
                {
                    throw new Exception("Tài khoản đã có người đăng ký!");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void picBtnReload_Click(object sender, EventArgs e)
        {
            FetchDanhSachNguoiChoi();
        }

        private void dgvNguoiChoi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlCommand = "";
                SqlCommand command;
                picBtnDelete.Visible = true;
                picLblDelete.Visible = true;
                DataGridViewRow row = new DataGridViewRow();
                row = dgvNguoiChoi.Rows[e.RowIndex];
                _currentTaiKhoan = row.Cells["TAIKHOAN"].Value.ToString();
                sqlCommand =
                    $"select COUNT(*) as SOLUONG from PLAYER where TAIKHOAN = '{_currentTaiKhoan}'";
                command = Database.CreateCommand(sqlCommand);
                int check = (int)command.ExecuteScalar();
                if (check == 0)
                {
                    picBtnDelete.Visible = false;
                    picLblDelete.Visible = false;
                    _currentTaiKhoan = null;
                    throw new Exception("Tài khoản không tồn tại");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void picBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentTaiKhoan != null)
                {
                    if (ConfirmBox())
                    {
                        Database.CreateConnection();
                        string sqlCommand = "";
                        SqlCommand command;

                        sqlCommand =
                            $"delete from PLAYER_CHARACTER where TAIKHOAN = '{_currentTaiKhoan}'";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();

                        sqlCommand = $"delete from PLAYER where TAIKHOAN = '{_currentTaiKhoan}'";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        picBtnDelete.Visible = false;
                        picLblDelete.Visible = false;
                        _currentTaiKhoan = null;
                        MessageBox.Show("Xóa thành công");
                        FetchDanhSachNguoiChoi();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void dgvTuVung_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlString;
                DataTable dt;

                txtDapAn.Text = "";

                DataGridViewRow row = new DataGridViewRow();
                row = dgvTuVung.Rows[e.RowIndex];
                _currentQuestionId = (int)row.Cells["QUESTION_ID_TV"].Value;
                txtCauHoi.Text = (string)row.Cells["CAUHOI_TV"].Value;
                txtCauHoiChinhSua.Text = (string)row.Cells["CAUHOI_TV"].Value;
                txtCapDoChinhSua.Text = row.Cells["CAPDO_TV"].Value.ToString();
                txtCoinChinhSua.Text = row.Cells["COIN_TV"].Value.ToString();
                txtLoaiChinhSua.Text = row.Cells["LOAI_TV"].Value.ToString();

                cboDanhSachCauTraLoi.Items.Clear();
                cboDanhSachCauTraLoi.SelectedIndex = -1;
                sqlString = $"select * from ANSWER where QUESTION_ID = '{_currentQuestionId}'";
                dt = Database.SelectQuery(sqlString);

                foreach (DataRow dr in dt.Rows)
                {
                    cboDanhSachCauTraLoi.Items.Add(dr["DAPAN"]);
                    txtDapAn.Text = txtDapAn.Text + $"{dr["DAPAN"]} \n";
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void picBtnCreateTraLoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentQuestionId != -1)
                {
                    Database.CreateConnection();
                    string sqlCommand = "";
                    SqlCommand command;
                    string cauTraLoi = txtCauTraLoi.Text.Trim();
                    if (string.IsNullOrEmpty(cauTraLoi))
                    {
                        throw new Exception("Vui lòng nhập câu trả lời");
                    }
                    if (ConfirmBox())
                    {
                        sqlCommand =
                            $"insert into ANSWER (DAPAN, QUESTION_ID) values (N'{cauTraLoi}', '{_currentQuestionId}')";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        _currentQuestionId = -1;
                        txtCauHoi.Text = null;
                        txtCauTraLoi.Text = null;
                        txtDapAn.Text = null;

                        MessageBox.Show("Thêm thành công");
                        FetchDanhSachTuVung();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void picBtnTaoCauHoi_Click(object sender, EventArgs e)
        {
            try
            {
                string cauHoi = txtCauHoiThem.Text.Trim();
                string cauTraLoi = txtCauTraLoiThem.Text.Trim();
                int capDo = int.Parse(txtCapDoThem.Text.Trim());
                int loai = int.Parse(txtLoaiThem.Text.Trim());
                int ID = int.Parse(txtIDCauHoiThem.Text.Trim());
                int coin = int.Parse(txtCoinThem.Text.Trim());
                if (string.IsNullOrEmpty(cauHoi) || string.IsNullOrEmpty(cauTraLoi))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin");
                }
                if (ConfirmBox())
                {
                    Database.CreateConnection();
                    string sqlCommand = "";
                    SqlCommand command;
                    sqlCommand =
                        $"insert into QUESTION (ID, CAUHOI, COIN, CAPDO, LOAI) values ('{ID}', N'{cauHoi}', '{coin}', '{capDo}', '{loai}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();

                    sqlCommand =
                        $"insert into ANSWER (DAPAN, QUESTION_ID) values (N'{cauTraLoi}', '{ID}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Thêm thành công");
                    txtIDCauHoiThem.Text = (int.Parse(txtIDCauHoiThem.Text) + 1).ToString();
                    FetchDanhSachTuVung();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void picBtnDeleteCauTraLoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboDanhSachCauTraLoi.SelectedIndex == -1)
                {
                    throw new Exception("Vui lòng chọn câu trả lời muốn xóa");
                }
                if (ConfirmBox())
                {
                    Database.CreateConnection();
                    string sqlCommand = "";
                    SqlCommand command;
                    SqlDataReader rd;

                    sqlCommand =
                        $"select count(*) as SOLUONG from ANSWER where QUESTION_ID = '{_currentQuestionId}' ";
                    command = Database.CreateCommand(sqlCommand);
                    rd = command.ExecuteReader();
                    int dem = 0;
                    while (rd.Read())
                    {
                        dem = (int)rd["SOLUONG"];
                    }

                    if (dem == 1)
                    {
                        throw new Exception("Câu hỏi này phải có tối thiểu 1 câu trả lời");
                    }
                    Database.CreateConnection();

                    sqlCommand =
                        $"delete from ANSWER where DAPAN = N'{cboDanhSachCauTraLoi.SelectedItem}'";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công");
                    FetchDanhSachTuVung();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void picBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentQuestionId != -1)
                {
                    if (ConfirmBox())
                    {
                        int loai = int.Parse(txtLoaiChinhSua.Text);
                        int coin = int.Parse(txtCoinChinhSua.Text);
                        int capDo = int.Parse(txtCapDoChinhSua.Text);
                        Database.CreateConnection();
                        string sqlCommand = "";
                        SqlCommand command;
                        sqlCommand =
                            $"update QUESTION set LOAI = '{loai}', COIN = '{coin}', CAPDO = '{capDo}' where ID = '{_currentQuestionId}' ";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Sửa thành công");
                        FetchDanhSachTuVung();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void picBtnDeleteCauHoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentQuestionId != -1)
                {
                    if (ConfirmBox())
                    {
                        Database.CreateConnection();
                        string sqlCommand = "";
                        SqlCommand command;

                        sqlCommand =
                            $"delete from ANSWER where QUESTION_ID = '{_currentQuestionId}'";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();

                        sqlCommand = $"delete from QUESTION where ID = '{_currentQuestionId}'";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        _currentQuestionId = -1;
                        MessageBox.Show("Xóa thành công");
                        FetchDanhSachTuVung();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void dgvDanhSachNhanVat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlString;
                DataTable dt;
                DataGridViewRow row = new DataGridViewRow();
                row = dgvDanhSachNhanVat.Rows[e.RowIndex];
                _currentCharacterId = (int)row.Cells["ID_NV"].Value;
                txtTenNVEdit.Text = (string)row.Cells["TEN_NV"].Value;
                txtMoTaNVEdit.Text = (string)row.Cells["MOTA_NV"].Value;
                txtMayManNVEdit.Text = row.Cells["MAYMAN_NV"].Value.ToString();
                txtCoinNVEdit.Text = row.Cells["COIN_NV"].Value.ToString();
                txtLuotTraLoiNVEdit.Text = row.Cells["LUOTTRALOITHEM_NV"].Value.ToString();
                txtTocDoNVEdit.Text = row.Cells["TOCDO_NV"].Value.ToString();
                picSelectedCharacter.Image = Character.GetImageCharacter(_currentCharacterId);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void picButtonEditNV_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentCharacterId != -1)
                {
                    if (ConfirmBox())
                    {
                        string ten = txtTenNVEdit.Text;
                        string moTa = txtMoTaNVEdit.Text;
                        int luotTraLoi = int.Parse(txtLuotTraLoiNVEdit.Text);
                        int mayMan = int.Parse(txtMayManNVEdit.Text);
                        int coin = int.Parse(txtCoinNVEdit.Text);
                        int tocDo = int.Parse(txtTocDoNVEdit.Text);
                        Database.CreateConnection();
                        string sqlCommand = "";
                        SqlCommand command;
                        sqlCommand =
                            $"update CHARACTER set TEN = N'{ten}', MOTA = N'{moTa}', LUOTTRALOITHEM = '{luotTraLoi}', MAYMAN = '{mayMan}', COIN = '{coin}', TOCDO = '{tocDo}' where ID = '{_currentCharacterId}' ";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        _currentCharacterId = -1;
                        MessageBox.Show("Sửa thành công");
                        FetchDanhSachNhanVat();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void dgvDanhSachMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Database.CreateConnection();
            string sqlString;
            DataTable dt;
            DataGridViewRow row = new DataGridViewRow();
            row = dgvDanhSachMap.Rows[e.RowIndex];
            _currentMapId = (int)row.Cells["ID_MAP"].Value;
            txtTenMapEdit.Text = (string)row.Cells["TEN_MAP"].Value;
            txtMoTaMapEdit.Text = (string)row.Cells["MOTA_MAP"].Value;
            txtCapDoMapEdit.Text = row.Cells["CAPDO_MAP"].Value.ToString();
            txtCoinRandom1.Text = row.Cells["COIN_RANDOM_1_MAP"].Value.ToString();
            txtCoinRandom2.Text = row.Cells["COIN_RANDOM_2_MAP"].Value.ToString();
            txtCoinTarget.Text = row.Cells["COIN_TARGET_MAP"].Value.ToString();
            picSelectedMap.Image = Map.GetImageMap(_currentMapId);
        }

        private void picBtnEditMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentMapId != -1)
                {
                    if (ConfirmBox())
                    {
                        string ten = txtTenMapEdit.Text;
                        string moTa = txtMoTaMapEdit.Text;
                        int capDo = int.Parse(txtCapDoMapEdit.Text);
                        int coinRandom1 = int.Parse(txtCoinRandom1.Text);
                        int coinRandom2 = int.Parse(txtCoinRandom2.Text);
                        int coinTarget = int.Parse(txtCoinTarget.Text);
                        Database.CreateConnection();
                        string sqlCommand = "";
                        SqlCommand command;
                        sqlCommand =
                            $"update MAP set TEN = N'{ten}', MOTA = N'{moTa}', COIN_RANDOM1 = '{coinRandom1}', COIN_RANDOM2 = '{coinRandom2}', COIN_TARGET = '{coinTarget}' where ID = '{_currentMapId}' ";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        _currentMapId = -1;
                        MessageBox.Show("Sửa thành công");
                        FetchDanhSachMap();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        // Reload lại danh sách khi Tab page thay đổi
        private void tpAdminPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tpAdminPanel.SelectedTab == tpNguoiChoi)
                {
                    FetchDanhSachNguoiChoi();
                }
                else if (tpAdminPanel.SelectedTab == tpLichSu)
                {
                    FetchLichSu();
                }
                else if (tpAdminPanel.SelectedTab == tpMap)
                {
                    FetchDanhSachMap();
                }
                else if (tpAdminPanel.SelectedTab == tpNhanVat)
                {
                    FetchDanhSachNhanVat();
                }
                else if (tpAdminPanel.SelectedTab == tpTuVung)
                {
                    FetchDanhSachTuVung();
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
