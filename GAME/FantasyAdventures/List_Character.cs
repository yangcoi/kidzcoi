using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyAdventures
{
    internal class List_Character
    {
        static List<Character> _listCharacter = new List<Character>();
        public static List<Character> ListCharacter
        {
            get { return _listCharacter; }
        }

        public static void Add(Character character)
        {
            _listCharacter.Add(character);
        }

        public static int Count()
        {
            return _listCharacter.Count;
        }

        public static Character GetCharacter(int ID)
        {
            Character character = null;
            foreach (Character item in _listCharacter)
            {
                if (ID == item.Id)
                {
                    character = item;
                    break;
                }
            }
            return character;
        }
    }
}
