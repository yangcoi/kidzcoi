using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FantasyAdventures
{
    internal class Map
    {
        int _id;
        string _ten = "";
        string _moTa = "";
        int _capDo = 0;
        int _coinRandom1 = 0;
        int _coinRandom2 = 0;
        int _coinTarget = 0;

        public int Id
        {
            get { return _id; }
        }
        public string Ten
        {
            get { return _ten; }
        }
        public string MoTa
        {
            get { return _moTa; }
        }
        public int CapDo
        {
            get { return _capDo; }
        }
        public int CoinRandom1
        {
            get { return _coinRandom1; }
        }

        public int CoinRandom2
        {
            get { return _coinRandom2; }
        }
        public int CoinTarget
        {
            get { return _coinTarget; }
        }

        public Map(
            int id,
            string ten,
            string moTa,
            int capDo,
            int coinRandom1,
            int coinRandom2,
            int coinTarget
        )
        {
            _id = id;
            _ten = ten;
            _moTa = moTa;
            _capDo = capDo;
            _coinRandom1 = coinRandom1;
            _coinRandom2 = coinRandom2;
            _coinTarget = coinTarget;
        }

        public static Image GetImageMap(int Id)
        {
            Image image = null;
            if (Id == 1)
            {
                image = Properties.Resources.map1;
            }
            else if (Id == 2)
            {
                image = Properties.Resources.map2;
            }
            else if (Id == 3)
            {
                image = Properties.Resources.map3;
            }

            return image;
        }
    }
}
