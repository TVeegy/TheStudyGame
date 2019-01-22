using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudyGame.Classes
{
    public class Student
    {
        private String _username="", _password="";
        private Boolean _profileCompleted = false;
        private String _firstName="", _lastName="", _gender="";
        private DateTime _dateOfBirth = default(DateTime);

        public Student(String username, String password, String firstname="", String lastName="", String gender="", DateTime dateOfBirth = default(DateTime))
        {
            _username = username;
            _password = password;
            _firstName = firstname;
            _lastName = lastName;
            _gender = gender;
            _dateOfBirth = dateOfBirth;

            CheckProfileCompletion();
        }

        public bool profileComplete { get; private set; }

        public String Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public String FirstName
        {
            get { return _firstName; }
            set { _firstName = value; CheckProfileCompletion();}
        }
        public String LastName
        {
            get { return _lastName; }
            set { _lastName = value; CheckProfileCompletion();}
        }
        public String Gender
        {
            get { return _gender; }
            set { _gender = value; CheckProfileCompletion(); }
        }
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth.Date; }
            set { _dateOfBirth = value; CheckProfileCompletion();}
        }

        private List<String> createStudentInfo()
        {
            List<String> credentials = new List<String> { _username, _password };
            String[] profile = { _firstName, _lastName, _dateOfBirth.ToString() };
            foreach (String item in profile)
            {
                if (item != default(String) || item != default(DateTime).ToString())
                    credentials.Add(item);
            }
            return credentials;
        }

        public void CheckProfileCompletion()
        {
            Byte completed = 0;
            String[] profileInfo = { _firstName, _lastName, _gender, _dateOfBirth.ToString() };
            foreach (String item in profileInfo)
            {
                if (item != "" && item != default(DateTime).ToString())
                    completed++;
            }

            profileComplete = (completed == profileInfo.Length) ? true : false;
            MessageBox.Show($"Completed? {profileComplete} in het geval: {this._username}");
        }
    }
}
