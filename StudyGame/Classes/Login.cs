using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGame.Classes
{
    public class Login
    {
        public static String username;

        public static String HandleLogin(String username, String password)
        {
            return Database.RetrieveAndCheckLogin(username, password).ToString();
        }
    }
}
