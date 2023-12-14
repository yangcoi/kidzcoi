using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyAdventures
{
    internal class ListAnswer
    {
        static List<Answer> _listItem = new List<Answer>();
        static List<Answer> _listItemLuyenTap = new List<Answer>();
        static List<Answer> _listAllAnswer = new List<Answer>();
        public static List<Answer> ListItem
        {
            get { return _listItem; }
        }
        public static List<Answer> ListItemLuyenTap
        {
            get { return _listItemLuyenTap; }
        }
        public static List<Answer> ListAllAnswer
        {
            get { return _listAllAnswer; }
        }

        public static void AddAnswer(Answer item)
        {
            _listItem.Add(item);
        }

        public static void AddAnswerAllQuestion(Answer item)
        {
            _listAllAnswer.Add(item);
        }

        public static List<Answer> GetAnswersByQuestionID(int questionID)
        {
            List<Answer> data = new List<Answer>();
            foreach (Answer item in _listAllAnswer)
            {
                if (item.CauHoiID == questionID)
                {
                    data.Add(item);
                }
            }
            return data;
        }

        public static List<Answer> GetAnswersLuyenTapByType(int type)
        {
            List<Answer> data = new List<Answer>();
            foreach (Answer item in _listAllAnswer)
            {
                if (item.Loai == type)
                {
                    data.Add(item);
                }
            }
            return data;
        }

        public static List<Answer> GetAnswersMapByType(int type)
        {
            List<Answer> data = new List<Answer>();
            foreach (Answer item in _listItem)
            {
                if (item.Loai == type)
                {
                    data.Add(item);
                }
            }
            return data;
        }
    }
}
