using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGame.Classes
{
    public class Student
    {
        private String _username="", _password="";
        private Boolean _profileCompleted = false;
        private String _firstName="", _lastName="";
        private DateTime _dateOfBirth = default(DateTime);

        public Student(String username, String password, String firstname="", String lastName="", DateTime dateOfBirth = default(DateTime))
        {
            _username = username;
            _password = password;
            _firstName = firstname;
            _lastName = lastName;
            _dateOfBirth = dateOfBirth;

            CheckProfileCompletion();
            addNewStudent(createStudentInfo());
        }

        public bool ProfileComplete { get; private set; }

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
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth.Date; }
            set { _dateOfBirth = value; CheckProfileCompletion();}
        }

        private Boolean addNewStudent(List<String> credentials)
        {
            Boolean addSuccesful = false;

            return addSuccesful;
        }

        private Boolean editRecords(List<String> credentials)
        {
            Boolean editSuccesful = false;

            return editSuccesful;
        }

        private List<String> createStudentInfo()
        {
            // List up all filled-in profile information for database entry.
            List<String> credentials = new List<String> { _username, _password };
            String[] profile = { _firstName, _lastName, _dateOfBirth.ToString() };
            foreach (String item in profile)
            {
                if (item != default(String) || item != default(DateTime).ToString())
                    credentials.Add(item);
            }
            return credentials;
        }

        private void CheckProfileCompletion()
        {
            // Shortcut when unnecessary
            if (_profileCompleted != true)
            {
                // Counting what hasn't been filled in
                Byte uncompleted = 0;
                String[] profile = { _firstName, _lastName, _dateOfBirth.ToString() };
                foreach (String item in profile)
                {
                    if (item == "" || item == default(DateTime).ToString())
                        uncompleted++;
                }
                if (uncompleted == 0)
                    ProfileComplete = true;
            }
        }
    }
}
