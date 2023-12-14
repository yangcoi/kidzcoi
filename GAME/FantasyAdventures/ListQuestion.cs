using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace FantasyAdventures
{
    internal class ListQuestion
    {
        static List<Question> _listItem = new List<Question>();
        static List<Question> _listItemLuyenTap = new List<Question>();
        static List<Question> _listAllQuestion = new List<Question>();

        public static List<Question> ListItem
        {
            get { return _listItem; }
        }
        public static List<Question> ListItemLuyenTap
        {
            get { return _listItemLuyenTap; }
        }
        public static List<Question> ListAllQuestion
        {
            get { return _listAllQuestion; }
        }

        static Question GetQuestionByID(int ID)
        {
            Question data = null;
            foreach (Question item in _listAllQuestion)
            {
                if (item.Id == ID)
                {
                    data = item;
                    return data;
                }
            }
            return data;
        }

        public static void InitialListAllQuestion()
        {
            _listAllQuestion.Clear();
            Database.CreateConnection();

            string sqlCommand =
                $"select count(*)  as SOLUONG , CAUHOI, QUESTION_ID, LOAI, CAPDO, COIN from QUESTION A, ANSWER B where A.ID = B.QUESTION_ID group by CAUHOI, QUESTION_ID, LOAI, CAPDO, COIN";
            DataTable dt = Database.SelectQuery(sqlCommand);
            foreach (DataRow dr in dt.Rows)
            {
                int loai = (int)dr["LOAI"];
                int soLuongDapAn = (int)dr["SOLUONG"];
                int capDo = (int)dr["CAPDO"];
                int coin = (int)dr["COIN"];
                int questionID = (int)dr["QUESTION_ID"];
                string cauHoi = (string)dr["CAUHOI"];
                Question newQuestion = new Question(
                    loai,
                    questionID,
                    cauHoi,
                    coin,
                    capDo,
                    soLuongDapAn
                );
                _listAllQuestion.Add(newQuestion);
            }
            UpdateAnswerAllQuestion();
        }

        public static void UpdateAnswerAllQuestion()
        {
            Database.CreateConnection();
            string sqlCommand = "";
            SqlCommand cmd;

            foreach (Question item in _listAllQuestion)
            {
                sqlCommand = $"select * from ANSWER where QUESTION_ID = '{item.Id}'";
                DataTable dt = Database.SelectQuery(sqlCommand);
                foreach (DataRow dr in dt.Rows)
                {
                    int id = (int)dr["ID"];
                    int questionID = (int)dr["QUESTION_ID"];
                    string dapAn = (string)dr["DAPAN"];
                    int loai = item.Loai;
                    Answer newAnswer = new Answer(id, questionID, dapAn, loai);
                    item.AddAnswer(newAnswer);
                    ListAnswer.AddAnswerAllQuestion(newAnswer);
                }
            }
        }

        public static void InitialListQuestionMap()
        {
            _listItem.Clear();
            foreach (Question item in _listAllQuestion)
            {
                if (item.CapDo <= Player.SelectedMap.CapDo)
                {
                    _listItem.Add(item);
                }
            }
            UpdateAnswerQuestionMap();
        }

        public static void UpdateAnswerQuestionMap()
        {
            foreach (Question item in _listItem)
            {
                List<Answer> listAnswer = ListAnswer.GetAnswersByQuestionID(item.Id);
                foreach (Answer x in listAnswer)
                {
                    item.AddAnswer(x);
                    ListAnswer.ListItem.Add(x);
                }
            }
        }

        public static void InitialListQuestionLuyenTap()
        {
            _listItemLuyenTap.Clear();
            Database.CreateConnection();
            string sqlCommand =
                $"select top 90 * from VOCABULARY A, QUESTION B where A.TAIKHOAN = '{Player.UserName}' and A.QUESTION_ID = B.ID order by THOIGIAN asc";
            DataTable dt = Database.SelectQuery(sqlCommand);
            foreach (DataRow dr in dt.Rows)
            {
                int questionID = (int)dr["QUESTION_ID"];

                Question newQuestion = GetQuestionByID(questionID);
                _listItemLuyenTap.Add(newQuestion);
            }
            UpdateAnswerLuyenTap();
        }

        public static void UpdateAnswerLuyenTap()
        {
            foreach (Question item in _listItemLuyenTap)
            {
                List<Answer> listAnswer = ListAnswer.GetAnswersByQuestionID(item.Id);
                foreach (Answer x in listAnswer)
                {
                    ListAnswer.ListItemLuyenTap.Add(x);
                }
            }
        }
    }
}
