using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FantasyAdventures
{
    internal class Player
    {
        static string _userName = null;
        static int _capDo = 1;
        static int _coin = 0;
        static int _quyenHan = 0;
        static Character _selectedCharacter = null;
        static Map _selectedMap = null;
        static List<Character> _listCharacter = new List<Character>();
        static List<Map> _listMap = new List<Map>();

        public static string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public static int CapDo
        {
            get { return _capDo; }
            set { _capDo = value; }
        }
        public static int QuyenHan
        {
            get { return _quyenHan; }
            set { _quyenHan = value; }
        }
        public static int Coin
        {
            get { return _coin; }
            set { _coin = value; }
        }
        public static Character SelectedCharacter
        {
            get { return _selectedCharacter; }
            set { _selectedCharacter = value; }
        }
        public static Map SelectedMap
        {
            get { return _selectedMap; }
            set { _selectedMap = value; }
        }
        public static List<Character> ListCharacter
        {
            get { return _listCharacter; }
        }
        public static List<Map> ListMap
        {
            get { return _listMap; }
            set { _listMap = value; }
        }

        public Player()
        {
            _userName = "";
            _capDo = 1;
            _coin = 0;
        }

        public static void SetPlayer(string userName, int capDo, int coin, Character character)
        {
            _userName = userName;
            _capDo = capDo;
            _coin = coin;
            _selectedCharacter = character;
        }

        public static bool IsLogin()
        {
            return !string.IsNullOrEmpty(_userName);
        }

        public static void Destroy()
        {
            _userName = null;
            _capDo = 1;
            _coin = 0;
            _selectedCharacter = null;
        }

        public static void ResetNewGame()
        {
            _selectedMap = null;
        }

        public static void AddCharacter(Character item)
        {
            _listCharacter.Add(item);
        }

        public static void AddMap(Map item)
        {
            _listMap.Add(item);
        }

        public static void SetSelectedCharacter(Character character)
        {
            _selectedCharacter = character;
            Database.CreateConnection();
            string sqlCommand =
                $"update PLAYER set NHANVAT = '{character.Id}' where TAIKHOAN = '{_userName}'";
            SqlCommand command = Database.CreateCommand(sqlCommand);
            command.ExecuteNonQuery();
        }

        public static void SetSelectedMap(Map map)
        {
            _selectedMap = map;
        }
    }
}
