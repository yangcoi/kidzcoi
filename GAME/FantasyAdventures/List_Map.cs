using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyAdventures
{
    internal class List_Map
    {
        static List<Map> _listMap = new List<Map>();
        public static List<Map> ListMap
        {
            get
            {
                return _listMap;
            }
        }

        public static void Add(Map item)
        {
            _listMap.Add(item);
        }

        public static int Count()
        {
            return _listMap.Count;
        }

        public static Map GetMap(int ID)
        {
            Map map = null;
            foreach (var item in _listMap)
            {
                if (ID == item.Id)
                {
                    map = item;
                    break;
                }
            }
            return map;
        }
    }
}
