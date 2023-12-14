using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyAdventures
{
    internal class Answer
    {
        int _id;
        string _dapAn = null;
        int _cauHoiID;
        int _loai;
        public string DapAn
        {
            get { return _dapAn; }
        }
        public int CauHoiID
        {
            get { return _cauHoiID; }
        }
        public int Id
        {
            get { return _id; }
        }
        public int Loai
        {
            get { return _loai; }
        }

        public Answer(int id, int cauHoiID, string dapAn, int loai)
        {
            _id = id;
            _cauHoiID = cauHoiID;
            _dapAn = dapAn;
            _loai = loai;
        }
    }
}
