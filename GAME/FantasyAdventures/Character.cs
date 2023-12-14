using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FantasyAdventures
{
    internal class Character
    {
        int _id;
        string _ten = "";
        string _moTa = "";
        int _coin = 0;
        int _tocDo = 15;
        int _mayMan = 0;
        int _luotTraLoiThem = 0;

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
        public int TocDo
        {
            get { return _tocDo; }
        }
        public int MayMan
        {
            get { return _mayMan; }
        }
        public int LuotTraLoiThem
        {
            get { return _luotTraLoiThem; }
        }
        public int Coin
        {
            get { return _coin; }
        }

        public Character(
            int id,
            string ten,
            string moTa,
            int coin,
            int tocDo,
            int mayMan,
            int luotTraLoiThem
        )
        {
            _id = id;
            _ten = ten;
            _moTa = moTa;
            _coin = coin;
            _tocDo = tocDo;
            _mayMan = mayMan;
            _luotTraLoiThem = luotTraLoiThem;
        }

        // Hàm lấy ảnh nhân vật đang đứng yên
        public static Image GetImageCharacter(int Id)
        {
            Image image = null;
            if (Id == 1)
            {
                image = Properties.Resources.dog;
            }
            else if (Id == 2)
            {
                image = Properties.Resources.dog1;
            }
            else if (Id == 3)
            {
                image = Properties.Resources.cat;
            }
            else if (Id == 4)
            {
                image = Properties.Resources.cat1;
            }
            else if (Id == 5)
            {
                image = Properties.Resources.bird;
            }
            else if (Id == 6)
            {
                image = Properties.Resources.bird1;
            }
            else if (Id == 7)
            {
                image = Properties.Resources.rat;
            }
            else if (Id == 8)
            {
                image = Properties.Resources.rat1;
            }

            return image;
        }

        // Hàm lấy ảnh nhân vật đang chạy
        public static Image GetImageCharacterWalk(int Id)
        {
            Image image = null;
            if (Id == 1)
            {
                image = Properties.Resources.dog_walk;
            }
            else if (Id == 2)
            {
                image = Properties.Resources.dog1_walk;
            }
            else if (Id == 3)
            {
                image = Properties.Resources.cat_walk;
            }
            else if (Id == 4)
            {
                image = Properties.Resources.cat1_walk;
            }
            else if (Id == 5)
            {
                image = Properties.Resources.bird_walk;
            }
            else if (Id == 6)
            {
                image = Properties.Resources.bird1_walk;
            }
            else if (Id == 7)
            {
                image = Properties.Resources.rat_walk;
            }
            else if (Id == 8)
            {
                image = Properties.Resources.rat1_walk;
            }

            return image;
        }

        // Hàm lấy ảnh avatar của nhân vật
        public static Image GetImageCharacterAvatar(int Id)
        {
            Image image = null;
            if (Id == 1)
            {
                image = Properties.Resources.dog_avatar;
            }
            else if (Id == 2)
            {
                image = Properties.Resources.dog1_avatar;
            }
            else if (Id == 3)
            {
                image = Properties.Resources.cat_avatar;
            }
            else if (Id == 4)
            {
                image = Properties.Resources.cat1_avatar;
            }
            else if (Id == 5)
            {
                image = Properties.Resources.bird_avatar;
            }
            else if (Id == 6)
            {
                image = Properties.Resources.bird1_avatar;
            }
            else if (Id == 7)
            {
                image = Properties.Resources.rat_avatar;
            }
            else if (Id == 8)
            {
                image = Properties.Resources.rat1_avatar;
            }
            return image;
        }
    }
}
