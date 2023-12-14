using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyAdventures
{
    internal class Question
    {
        int _loai = 0;
        int _id;
        string _cauHoi = null;
        int _coin = 0;
        int _capDo = 1;
        List<Answer> _danhSachDapAn = null;

        public int Id
        {
            get { return _id; }
        }
        public int Loai
        {
            get { return _loai; }
        }
        public int Coin
        {
            get { return _coin; }
        }
        public int CapDo
        {
            get { return _capDo; }
        }
        public string CauHoi
        {
            get { return _cauHoi; }
        }
        public int SoLuongDapAn
        {
            get { return _danhSachDapAn.Count; }
        }

        public Question(int loai, int id, string cauHoi, int coin, int capDo, int soLuongDapAn)
        {
            _loai = loai;
            _id = id;
            _cauHoi = cauHoi;
            _danhSachDapAn = new List<Answer>(soLuongDapAn);
            _coin = coin;
            _capDo = capDo;
        }

        public void AddAnswer(Answer item)
        {
            _danhSachDapAn.Add(item);
        }

        public Answer GetRandomAnswer()
        {
            Random random = new Random();
            int indexRandom = random.Next(0, _danhSachDapAn.Count);
            Answer randomAnswer = _danhSachDapAn.ElementAt(indexRandom);
            return randomAnswer;
        }
    }
}
