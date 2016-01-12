using System;
using System.Windows.Forms;
//using System.Data.SQLite;
//using System.IO;

namespace Dipl{
    internal class Login
    {
        public static string Username = "";
        public static string Type = "";

        private static string _userLogin;
        private static string _userPass;
        private static string _username;
        private static string _type;


        //типо данные в бд, когда будет БД, удалить.
        // { логин, пароль, имя, группа }
        private static string[,] _userArray = new string[5, 4]{{"admin", "admin", "Админ", "admin"}, {"user", "user", "Юзер", "user"}, {"guest", "", "Гость", "guest"}, {"Oleg", "1234", "Олег", "admin"}, {"Ghost", "1234", "Влад", "user"}};

        public static bool Authorization(string login, string password)
        {
            //тут запрос к бд. Это всё нужно заменить на бд, тут бред, потому что приходится эмулировать бд.

            if (String.IsNullOrEmpty(login))
            {
                MessageBox.Show("Поле логин не должно быть пустым", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for (int i = 0; _userArray.GetLength(0) > i; i++){
                    _userLogin = _userArray[i, 0];
                    _userPass = _userArray[i, 1];
                    _username = _userArray[i, 2];
                    _type = _userArray[i, 3];

                    if (login.ToLower() == _userLogin.ToLower()){
                        break;
                    }
                }
            }

            //конец запроса

            if (login.ToLower() == _userLogin.ToLower() &&
                password == _userPass){
                Username =_username;
                Type = _type;
                return true;
            }

            return false;
        }

        public static void LogOut(){
            Username = "";
            Type = "";
            new LoginForm2().ShowDialog();
        }
    }
}