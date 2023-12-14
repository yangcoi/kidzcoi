using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyAdventures
{
    // Lớp này dùng để điều khiển mọi thứ khi tham gia game
    internal class ControlCharacter
    {
        // Vị trí left và top ban đầu của nhân vật
        static int _positionLeft = 100;
        static int _positionTop = 364;

        // Kiểm tra xem người chơi có nhấn nút di chuyển phải hay không
        static bool _isRight = false;

        // Mạng của người chơi
        static int _mang = 3;

        // Độ may mắn của người chơi
        static int _mayMan = 0;

        // Lượt chọn lại khi tham gia Quizz
        static int _luotChonLai = 0;

        // Coin của người chơi
        static int _coin = 0;

        // Tốc độ di chuyển
        static int _speedWalker = 15;

        // Kiểm tra xem người chơi có chọn đúng đáp án hay không
        static bool _isSelectedRightAnswer = false;

        // Kiểm tra xem người chơi có click nút về trang chủ
        static bool _isGoHome = false;

        // Kiểm tra xem người chơi có click nút chơi lại
        static bool _isPlayAgain = false;

        // Câu hỏi hiện tại
        static Question _currentQuestion = null;

        // Kiểm tra xem người chơi đã hoàn thành Quizz chưa
        static bool _isCompletedQA = false;

        // Các form hiện hành
        static StartGame _formStartGame = null;
        static InitialGame _formInitialGame = null;
        static MainGame _formMainGame = null;
        public static StartGame FormStartGame
        {
            get { return _formStartGame; }
            set { _formStartGame = value; }
        }
        public static MainGame FormMainGame
        {
            get { return _formMainGame; }
            set { _formMainGame = value; }
        }
        public static InitialGame FormInitialGame
        {
            get { return _formInitialGame; }
            set { _formInitialGame = value; }
        }
        public static int PositionLeft
        {
            get { return _positionLeft; }
            set { _positionLeft = value; }
        }
        public static int PositionTop
        {
            get { return _positionTop; }
            set { _positionTop = value; }
        }
        public static bool IsRight
        {
            get { return _isRight; }
            set { _isRight = value; }
        }
        public static bool IsGoHome
        {
            get { return _isGoHome; }
            set { _isGoHome = value; }
        }
        public static bool IsPlayAgain
        {
            get { return _isPlayAgain; }
            set { _isPlayAgain = value; }
        }
        public static bool IsCompletedQA
        {
            get { return _isCompletedQA; }
            set { _isCompletedQA = value; }
        }
        public static bool IsSelectedRightAnswer
        {
            get { return _isSelectedRightAnswer; }
            set { _isSelectedRightAnswer = value; }
        }

        public static int Mang
        {
            get { return _mang; }
            set { _mang = value; }
        }
        public static int LuotChonLai
        {
            get { return _luotChonLai; }
            set { _luotChonLai = value; }
        }
        public static int Coin
        {
            get { return _coin; }
            set { _coin = value; }
        }
        public static int SpeedWalker
        {
            get { return _speedWalker; }
            set { _speedWalker = value; }
        }
        public static int MayMan
        {
            get { return _mayMan; }
            set { _mayMan = value; }
        }
        public static Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set { _currentQuestion = value; }
        }
        // Reset lại Control Character
        public static void ResetGame()
        {
            _positionLeft = 100;
            _positionTop = 364;
            _isRight = false;
            _mang = 3;
            _luotChonLai = 0;
            _coin = 0;
            _isCompletedQA = false;
            _isSelectedRightAnswer = false;
            _currentQuestion = null;
            _isGoHome = false;
            _isPlayAgain = false;
        }
    }
}
