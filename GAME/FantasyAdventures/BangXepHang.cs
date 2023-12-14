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
    public partial class BangXepHang : LostForm
    {
        public BangXepHang()
        {
            InitializeComponent();
        }

        private void BangXepHang_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateBangXepHang();
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        #region Hàm phục vụ
        void UpdateBXHCapDo()
        {
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand = "select top 5 TAIKHOAN, CAPDO, COIN from PLAYER order by CAPDO desc";
            dt = Database.SelectQuery(sqlCommand);
            dgvXepHangCapDo.DataSource = dt;
        }

        void UpdateBXHCoin()
        {
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand = "select top 5 TAIKHOAN, CAPDO, COIN from PLAYER order by COIN desc";
            dt = Database.SelectQuery(sqlCommand);
            dgvXepHangCoin.DataSource = dt;
        }

        void UpdateBXHHocTap()
        {
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand command;
            DataTable dt;
            sqlCommand =
                "select top 5 count(*) as SOLUONG, A.TAIKHOAN, B.CAPDO, B.COIN  from VOCABULARY A, PLAYER B where A.TAIKHOAN = B.TAIKHOAN  group by A.TAIKHOAN, B.CAPDO, B.COIN order by SOLUONG desc";
            dt = Database.SelectQuery(sqlCommand);
            dgvXepHangHocTap.DataSource = dt;
        }

        void UpdateBangXepHang()
        {
            UpdateBXHCapDo();
            UpdateBXHCoin();
            UpdateBXHHocTap();
        }

        #endregion
        #region Sự kiện Click
        private void tpBXH_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tpBXH.SelectedTab == tpCapDo)
                {
                    UpdateBXHCapDo();
                }
                else if (tpBXH.SelectedTab == tpCoin)
                {
                    UpdateBXHCoin();
                }
                else if (tpBXH.SelectedTab == tpHocTap)
                {
                    UpdateBXHHocTap();
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        private void ButtonLamMoi_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateBangXepHang();
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
