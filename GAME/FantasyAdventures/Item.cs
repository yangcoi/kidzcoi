using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FantasyAdventures
{
    internal class Item
    {
        int _positionLeft = 200;
        int _positionTop = 354;
        PictureBox _item = new PictureBox();

        public Item(int positionLeft = 200, int positionTop = 360)
        {
            _positionLeft = positionLeft;
            _positionTop = positionTop;
        }

        public PictureBox CreateItem()
        {
            _item.Size = new Size(65, 60);
            _item.SizeMode = PictureBoxSizeMode.StretchImage;
            _item.Image = Properties.Resources.item;
            _item.BackColor = Color.Transparent;
            _item.Tag = "item";
            _item.Left = _positionLeft;
            _item.Top = _positionTop;
            _item.BringToFront();
            return _item;
        }
    }
}
