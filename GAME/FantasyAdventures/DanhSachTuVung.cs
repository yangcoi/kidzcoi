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
    public partial class DanhSachTuVung : LostForm
    {
        public DanhSachTuVung()
        {
            InitializeComponent();
        }

        private void DanhSachTuVung_Load(object sender, EventArgs e)
        {
            UpdateDanhSachTuVung();
        }

        #region Hàm phục vụ
        void UpdateTuVungTiengAnh()
        {
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                $"select A.THOIGIAN, A.QUESTION_ID, B.CAUHOI, B.COIN, B.CAPDO from VOCABULARY A, QUESTION B where A.TAIKHOAN = '{Player.UserName}' and A.QUESTION_ID = B.ID and B.LOAI = 0 order by THOIGIAN desc";
            dt = Database.SelectQuery(sqlCommand);
            dgvTiengAnh.DataSource = dt;
        }

        void UpdateTuVungTiengViet()
        {
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                $"select A.THOIGIAN as THOIGIAN_VIET, A.QUESTION_ID as QUESTION_ID_VIET, B.CAUHOI as CAUHOI_VIET, B.COIN as COIN_VIET, B.CAPDO as CAPDO_VIET from VOCABULARY A, QUESTION B where A.TAIKHOAN = '{Player.UserName}' and A.QUESTION_ID = B.ID and B.LOAI = 1 order by THOIGIAN desc";
            dt = Database.SelectQuery(sqlCommand);
            dgvTiengViet.DataSource = dt;
        }

        void UpdateDanhSachTuVung()
        {
            try
            {
                UpdateTuVungTiengAnh();
                UpdateTuVungTiengViet();
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
        private void buttonLamMoi_Click(object sender, EventArgs e)
        {
            UpdateDanhSachTuVung();
        }

        private void dgvTiengAnh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlString;
                DataTable dt;

                txtDapAn.Text = "";
                DataGridViewRow row = new DataGridViewRow();
                row = dgvTiengAnh.Rows[e.RowIndex];
                sqlString =
                    $"select * from ANSWER where QUESTION_ID = '{row.Cells["QUESTION_ID"].Value}'";
                dt = Database.SelectQuery(sqlString);
                // Hiển thị đáp án lên textbox
                foreach (DataRow dr in dt.Rows)
                {
                    txtDapAn.Text = txtDapAn.Text + $"{dr["DAPAN"]} \n";
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void dgvTiengViet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlString;
                DataTable dt;

                txtDapAn.Text = "";
                DataGridViewRow row = new DataGridViewRow();
                row = dgvTiengViet.Rows[e.RowIndex];
                sqlString =
                    $"select * from ANSWER where QUESTION_ID = '{row.Cells["QUESTION_ID_VIET"].Value}'";
                dt = Database.SelectQuery(sqlString);
                // Hiển thị đáp án lên textbox
                foreach (DataRow dr in dt.Rows)
                {
                    txtDapAn.Text = txtDapAn.Text + $"{dr["DAPAN"]} \n";
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void tpLichSuHocTap_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tpLichSuHocTap.SelectedTab == tpTiengAnh)
                {
                    UpdateTuVungTiengAnh();
                }
                else if (tpLichSuHocTap.SelectedTab == tpTiengViet)
                {
                    UpdateTuVungTiengViet();
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
