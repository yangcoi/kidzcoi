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
    public partial class LuyenTapForm : LostForm
    {
        // Kiểm tra đã thua hay chưa
        bool _isLose = false;
        bool _isClickingButton = false;

        // Số câu đã trả lời
        int _soCau = 0;

        // Câu hỏi hiện tại
        Question _currentQuestion = null;

        // Câu trả lời đúng của câu hỏi hiện tại
        Answer _correctAnswer = null;
        Timer _timerThoiGianTraLoi;

        // Thời gian trả lời: 90s
        int _counter = 90;

        // Mạng: 3
        int _mang = 3;
        bool _isConfirmExit = false;

        public LuyenTapForm()
        {
            InitializeComponent();
            this.btnA.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
            this.btnB.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
            this.btnC.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
            this.btnD.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
        }

        private void LuyenTapForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Khởi tạo List Câu hỏi ôn tập
                ListQuestion.InitialListQuestionLuyenTap();
                _soCau++;
                if (_soCau > ListQuestion.ListItemLuyenTap.Count && _isLose == false)
                {
                    if (_timerThoiGianTraLoi != null)
                    {
                        _timerThoiGianTraLoi.Stop();
                    }
                    _isConfirmExit = true;
                    GameDialog fGameDialog = new GameDialog();
                    fGameDialog.SetState(0, "Bạn đã hoàn thành phần Luyện Tập");
                    fGameDialog.ShowDialog();
                    this.Close();
                    return;
                }
                lblSoCau.Text = $"Câu {_soCau}/{ListQuestion.ListItemLuyenTap.Count}";

                lblThoiGian.Text = _counter.ToString() + "s";
                _timerThoiGianTraLoi = new Timer();
                _timerThoiGianTraLoi.Tick += timerThoiGianTraLoi_Tick;
                _timerThoiGianTraLoi.Interval = 1000;
                _timerThoiGianTraLoi.Enabled = false;
                Question question = ListQuestion.ListItemLuyenTap.ElementAt(_soCau - 1);
                if (question != null)
                {
                    _currentQuestion = question;
                    HienThiThongTinCauHoi();
                    HienThiThongTinDapAn();
                    _timerThoiGianTraLoi.Start();
                }
            }
            catch (Exception err)
            {
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        #region Các hàm Timer
        private void timerThoiGianTraLoi_Tick(object sender, EventArgs e)
        {
            HienThiMang();

            if (_counter > 0)
            {
                _counter--;
            }
            else
            {
                _timerThoiGianTraLoi.Stop();
                _isLose = true;
                _isConfirmExit = true;
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(0, "Bạn đã thua cuộc");
                fGameDialog.ShowDialog();

                this.Close();
            }
            lblThoiGian.Text = $"{_counter}s";
        }
        #endregion
        #region Các hàm phục vụ
        void UpdateNewQuestion()
        {
            // Nếu đã thua thì thoát Form
            if (_isLose == true)
            {
                _isConfirmExit = true;
                this.Close();
                return;
            }

            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "answer")
                {
                    x.Visible = false;
                }
            }
            _soCau++;
            if (_soCau > ListQuestion.ListItemLuyenTap.Count && _isLose == false)
            {
                _timerThoiGianTraLoi.Stop();
                _isConfirmExit = true;
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(0, "Bạn đã hoàn thành phần Luyện Tập");
                fGameDialog.ShowDialog();
                this.Close();
                return;
            }
            lblSoCau.Text = $"Câu {_soCau}/{ListQuestion.ListItemLuyenTap.Count}";

            _currentQuestion = null;
            _correctAnswer = null;
            lblThoiGian.Text = _counter.ToString() + "s";
            Question question = ListQuestion.ListItemLuyenTap.ElementAt(_soCau - 1);
            if (question != null)
            {
                _currentQuestion = question;
                HienThiThongTinCauHoi();
                HienThiThongTinDapAn();
            }
        }

        void HienThiThongTinCauHoi()
        {
            if (_currentQuestion != null)
            {
                if (_currentQuestion.Loai == 0)
                {
                    lblLoaiCauHoi.Text = "Hãy chọn nghĩa tiếng Việt của từ này:";
                }
                else
                {
                    lblLoaiCauHoi.Text = "Hãy chọn nghĩa tiếng Anh của từ này:";
                }
                lblCauHoi.Text = _currentQuestion.CauHoi;
            }
        }

        void HienThiThongTinDapAn()
        {
            if (_currentQuestion != null)
            {
                Answer dapAn = _currentQuestion.GetRandomAnswer();
                _correctAnswer = dapAn;
                // NULL: Lỗi không tìm thấy câu trả lời nào của đáp án
                if (_correctAnswer == null)
                {
                    foreach (Control x in this.Controls)
                    {
                        if ((string)x.Tag == "answer")
                        {
                            this.Controls.Remove(x);
                        }
                    }
                }
                else
                {
                    // random vị trí button cho câu trả lời đúng
                    PhanBoViTriCauTraLoiDung();
                }
            }
        }

        void HienThiMang()
        {
            if (_mang == 2)
            {
                this.Controls.Remove(picTim3);
            }
            else if (_mang == 1)
            {
                this.Controls.Remove(picTim3);
                this.Controls.Remove(picTim2);
            }
            else if (_mang == 0)
            {
                _timerThoiGianTraLoi.Stop();
                this.Controls.Remove(picTim3);
                this.Controls.Remove(picTim2);
                this.Controls.Remove(picTim1);
                _isLose = true;
                _isConfirmExit = true;
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(0, "Bạn đã thua cuộc");
                fGameDialog.ShowDialog();
                this.Close();
            }
        }

        void PhanBoViTriCauTraLoiDung()
        {
            Random random = new Random();
            int viTriButtonDapAn = random.Next(0, 3);
            if (viTriButtonDapAn == 0)
            {
                btnA.Text = _correctAnswer.DapAn;
            }
            else if (viTriButtonDapAn == 1)
            {
                btnB.Text = _correctAnswer.DapAn;
            }
            else if (viTriButtonDapAn == 2)
            {
                btnC.Text = _correctAnswer.DapAn;
            }
            else if (viTriButtonDapAn == 3)
            {
                btnD.Text = _correctAnswer.DapAn;
            }
            PhanBoViTriCauTraLoiConLai(viTriButtonDapAn);
            foreach (Control x in this.Controls)
            {
                if ((string)x.Tag == "answer")
                {
                    x.Visible = true;
                }
            }
        }

        void PhanBoViTriCauTraLoiConLai(int indexCauTraLoiDung)
        {
            // chọn ngẫu nhiên các câu trả lời khác trong database
            Random random = new Random();
            List<Answer> answerList = new List<Answer>();
            List<Answer> answerListStore = ListAnswer.GetAnswersLuyenTapByType(
                _currentQuestion.Loai
            );
            int i = 0;
            while (i < 4)
            {
                int index = random.Next(0, answerListStore.Count);
                Answer answer = answerListStore.ElementAt(index);
                // Nếu random trùng câu trả lời đúng thì random lại
                while (answer.CauHoiID == _currentQuestion.Id)
                {
                    index = random.Next(0, answerListStore.Count);
                    answer = answerListStore.ElementAt(index);
                }
                answerList.Add(answer);
                i++;
            }
            // phân bố câu trả lời sai vào các button
            for (int j = 0; j <= 3; j++)
            {
                if (j != indexCauTraLoiDung)
                {
                    if (j == 0)
                    {
                        btnA.Text = answerList.ElementAt(j).DapAn;
                    }
                    else if (j == 1)
                    {
                        btnB.Text = answerList.ElementAt(j).DapAn;
                    }
                    else if (j == 2)
                    {
                        btnC.Text = answerList.ElementAt(j).DapAn;
                    }
                    else if (j == 3)
                    {
                        btnD.Text = answerList.ElementAt(j).DapAn;
                    }
                }
            }
        }

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
        #region Các hàm Click
        private void buttonSelectAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isClickingButton == false)
                {
                    _isClickingButton = true;
                    Database.CreateConnection();
                    string sqlCommand;
                    SqlCommand command;
                    string contentHistory;
                    Control ctr = (Control)sender;
                    if (ctr.Text != _correctAnswer.DapAn)
                    {
                        contentHistory =
                            $"username vừa luyện tập và trả lời sai câu hỏi: {_currentQuestion.CauHoi}";
                        sqlCommand =
                            $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        _mang--;
                        HienThiMang();
                    }
                    else
                    {
                        //Update lịch sử
                        contentHistory =
                            $"username vừa luyện tập và trả lời đúng câu hỏi: {_currentQuestion.CauHoi}";
                        sqlCommand =
                            $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();

                        sqlCommand =
                            $"delete from VOCABULARY where TAIKHOAN = '{Player.UserName}' and QUESTION_ID =  '{_currentQuestion.Id}'";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();

                        sqlCommand =
                            $"insert into VOCABULARY (TAIKHOAN, QUESTION_ID) values ('{Player.UserName}', '{_currentQuestion.Id}')";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                    }

                    UpdateNewQuestion();
                    _isClickingButton = false;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                _isConfirmExit = true;
                this.Close();
            }
        }

        #endregion

        private void LuyenTapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isConfirmExit)
            {
                if (ConfirmOutGame()) { }
                else
                {
                    e.Cancel = true;
                }
            }
            else { }
        }
    }
}
