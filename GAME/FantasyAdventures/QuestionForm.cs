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
    public partial class QuestionForm : LostForm
    {
        Random _randomQuestion = new Random();

        // Câu trả lời chính xác
        Answer _correctAnswer = null;
        Timer _timerThoiGianTraLoi;

        // Thời gian trả lời câu hỏi, tính bằng giây (s)
        int _counter = 30;
        bool _isConfirmExit = false;

        public QuestionForm()
        {
            InitializeComponent();
            this.btnA.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
            this.btnB.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
            this.btnC.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
            this.btnD.Click += new System.EventHandler(this.buttonSelectAnswer_Click);
        }

        private void QuestionForm_Load(object sender, EventArgs e)
        {
            try
            {
                ControlCharacter.IsCompletedQA = false;
                ControlCharacter.CurrentQuestion = null;
                ControlCharacter.IsSelectedRightAnswer = false;

                lblLuotChonLai.Text = $"Bạn có {ControlCharacter.LuotChonLai} lượt để chọn lại";
                lblThoiGian.Text = _counter.ToString() + "s";
                _timerThoiGianTraLoi = new Timer();
                _timerThoiGianTraLoi.Tick += timerThoiGianTraLoi_Tick;
                _timerThoiGianTraLoi.Interval = 1000;
                _timerThoiGianTraLoi.Enabled = false;
                int questionRandomIndex = _randomQuestion.Next(0, ListQuestion.ListItem.Count);
                Question question = ListQuestion.ListItem.ElementAt(questionRandomIndex);
                if (question != null)
                {
                    ControlCharacter.CurrentQuestion = question;
                    HienThiThongTinCauHoi();
                    HienThiThongTinDapAn();
                    _timerThoiGianTraLoi.Start();
                }
            }
            catch (Exception err)
            {
                _timerThoiGianTraLoi.Stop();
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }

        #region Các hàm Timer
        private void timerThoiGianTraLoi_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_counter > 0)
                {
                    _counter--;
                }
                else
                {
                    Database.CreateConnection();
                    string sqlCommand;
                    SqlCommand command;
                    string contentHistory;
                    _timerThoiGianTraLoi.Stop();
                    contentHistory =
                        $"username vừa trả lời sai câu hỏi: {ControlCharacter.CurrentQuestion.CauHoi}";
                    sqlCommand =
                        $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    _isConfirmExit = true;

                    this.Close();
                }
                lblThoiGian.Text = $"{_counter}s";
            }
            catch (Exception err)
            {
                _timerThoiGianTraLoi.Stop();
                GameDialog fGameDialog = new GameDialog();
                fGameDialog.SetState(1, err.Message);
                fGameDialog.ShowDialog();
            }
        }
        #endregion
        #region Các hàm Click

        private void buttonSelectAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                Database.CreateConnection();
                string sqlCommand;
                SqlCommand command;
                string contentHistory;
                Control ctr = (Control)sender;
                _timerThoiGianTraLoi.Stop();

                if (ctr.Text != _correctAnswer.DapAn)
                {
                    contentHistory =
                        $"username vừa trả lời sai câu hỏi: {ControlCharacter.CurrentQuestion.CauHoi}";
                    sqlCommand =
                        $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    if (ControlCharacter.LuotChonLai == 0)
                    {
                        MessageBox.Show(
                            $"Bạn đã trả lời sai, bạn còn {ControlCharacter.LuotChonLai} lượt chọn lại"
                        );
                        _isConfirmExit = true;
                        this.Close();
                        return;
                    }

                    ControlCharacter.LuotChonLai--;
                    lblLuotChonLai.Text = $"Bạn có {ControlCharacter.LuotChonLai} lượt để chọn lại";
                    MessageBox.Show(
                        $"Bạn đã trả lời sai, bạn còn {ControlCharacter.LuotChonLai} lượt chọn lại"
                    );
                    if (ControlCharacter.LuotChonLai == 0)
                    {
                        _isConfirmExit = true;
                        this.Close();
                        return;
                    }
                    ControlCharacter.IsSelectedRightAnswer = false;
                    _timerThoiGianTraLoi.Start();
                }
                else
                {
                    //Update lịch sử
                    contentHistory =
                        $"username vừa trả lời đúng câu hỏi: {ControlCharacter.CurrentQuestion.CauHoi}";
                    sqlCommand =
                        $"insert into HISTORY (TAIKHOAN, NOIDUNG) values ('{Player.UserName}', N'{contentHistory}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();

                    sqlCommand =
                        $"delete from VOCABULARY where TAIKHOAN = '{Player.UserName}' and QUESTION_ID =  '{ControlCharacter.CurrentQuestion.Id}'";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();

                    sqlCommand =
                        $"insert into VOCABULARY (TAIKHOAN, QUESTION_ID) values ('{Player.UserName}', '{ControlCharacter.CurrentQuestion.Id}')";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();

                    sqlCommand =
                        $"update PLAYER set COIN = COIN + '{ControlCharacter.CurrentQuestion.Coin}' where TAIKHOAN = '{Player.UserName}'";
                    command = Database.CreateCommand(sqlCommand);
                    command.ExecuteNonQuery();
                    Player.Coin += ControlCharacter.CurrentQuestion.Coin;
                    ControlCharacter.Coin += ControlCharacter.CurrentQuestion.Coin;

                    MessageBox.Show("Bạn đã trả lời đúng");
                    ControlCharacter.IsSelectedRightAnswer = true;
                    Random random = new Random();
                    // Random độ may mắn để nhận thêm Coin
                    int randomNum = random.Next(0, 10 - Player.SelectedCharacter.MayMan);
                    if (randomNum == 0)
                    {
                        ControlCharacter.Coin += ControlCharacter.CurrentQuestion.Coin;
                        int coinRandom = random.Next(
                            Player.SelectedMap.CoinRandom1,
                            Player.SelectedMap.CoinRandom2
                        );
                        MessageBox.Show($"Bạn đã may mắn nhận được thêm {coinRandom} Coin");
                        sqlCommand =
                            $"update PLAYER set COIN = COIN + '{coinRandom}' where TAIKHOAN = '{Player.UserName}'";
                        command = Database.CreateCommand(sqlCommand);
                        command.ExecuteNonQuery();
                        Player.Coin += coinRandom;
                    }
                    ControlCharacter.FormMainGame.HienThiNhanVatLuaChon();
                    _isConfirmExit = true;
                    this.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                _isConfirmExit = true;
                this.Close();
            }
        }

        // Sự kiện click vào nút 50/50
        private void btn5050_Click(object sender, EventArgs e)
        {
            try
            {
                if (Player.Coin < 50)
                {
                    throw new Exception("Bạn cần đủ 50 Coin để dùng công cụ này!");
                }
                int dem = 0;

                foreach (Control x in this.Controls)
                {
                    if (dem == 2)
                    {
                        break;
                    }
                    if ((string)x.Tag == "answer")
                    {
                        if (x.Text != _correctAnswer.DapAn)
                        {
                            this.Controls.Remove(x);
                            dem++;
                        }
                    }
                }
                Database.CreateConnection();
                string sqlCommand;
                SqlCommand command;
                sqlCommand =
                    $"update PLAYER set COIN = COIN - '50' where TAIKHOAN = '{Player.UserName}'";
                command = Database.CreateCommand(sqlCommand);
                command.ExecuteNonQuery();
                Player.Coin -= 50;
                ControlCharacter.FormMainGame.HienThiNhanVatLuaChon();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        #endregion
        #region Các hàm phục vụ

        // Phân bố vị trí câu trả lời chính xác
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
        }

        // Phân bố vị trí các câu trả lời còn lại
        void PhanBoViTriCauTraLoiConLai(int indexCauTraLoiDung)
        {
            // chọn ngẫu nhiên các câu trả lời khác trong database
            Random random = new Random();
            List<Answer> answerList = new List<Answer>();
            List<Answer> answerListStore = ListAnswer.GetAnswersMapByType(
                ControlCharacter.CurrentQuestion.Loai
            );

            int i = 0;
            while (i < 4)
            {
                int index = random.Next(0, answerListStore.Count);
                Answer answer = answerListStore.ElementAt(index);
                // Nếu random trùng câu trả lời đúng thì random lại
                while (answer.CauHoiID == ControlCharacter.CurrentQuestion.Id)
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

        // Hiển thị thông tin câu hỏi
        void HienThiThongTinCauHoi()
        {
            if (ControlCharacter.CurrentQuestion != null)
            {
                if (ControlCharacter.CurrentQuestion.Loai == 0)
                {
                    lblLoaiCauHoi.Text = "Hãy chọn nghĩa tiếng Việt của từ này:";
                }
                else
                {
                    lblLoaiCauHoi.Text = "Hãy chọn nghĩa tiếng Anh của từ này:";
                }
                lblCauHoi.Text = ControlCharacter.CurrentQuestion.CauHoi;
                lblPhanThuong.Text = $"Phần thưởng: {ControlCharacter.CurrentQuestion.Coin} Coin";
            }
        }

        // Hiển thị thông tin đáp án
        void HienThiThongTinDapAn()
        {
            if (ControlCharacter.CurrentQuestion != null)
            {
                Answer dapAn = ControlCharacter.CurrentQuestion.GetRandomAnswer();
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

        bool ConfirmOutGame()
        {
            DialogResult r;
            r = MessageBox.Show(
                $"Bạn có muốn thoát hay không? Bạn sẽ bị tính là trả lời sai nếu thoát",
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

        private void QuestionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_isConfirmExit)
                {
                    if (ConfirmOutGame())
                    {
                        _timerThoiGianTraLoi.Stop();
                        ControlCharacter.IsCompletedQA = true;
                        ControlCharacter.FormStartGame.UpdateGame();

                        ControlCharacter.LuotChonLai = Player.SelectedCharacter.LuotTraLoiThem;

                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    _timerThoiGianTraLoi.Stop();
                    ControlCharacter.LuotChonLai = Player.SelectedCharacter.LuotTraLoiThem;

                    ControlCharacter.IsCompletedQA = true;
                    if (ControlCharacter.FormStartGame != null)
                    {
                        ControlCharacter.FormStartGame.UpdateGame();
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
