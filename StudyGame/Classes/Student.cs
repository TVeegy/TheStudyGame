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

        public Student(String username, String password, String firstname="", String lastName="", DateTime dateOfBirth = default(DateTime))
        {
            _username = username;
            _password = password;
            _firstName = firstname;
            _lastName = lastName;
            //_gender = gender;
            _dateOfBirth = dateOfBirth;

            CheckProfileCompletion();
            SendToDB();
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
        /*public String Gender
        {
            get { return _gender; }
            set { _gender = value; CheckProfileCompletion(); }
        }*/
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth.Date; }
            set { _dateOfBirth = value; CheckProfileCompletion();}
        }
        private String[] StudentInfo
        {
            get { return new String[] { _username, _password, _firstName, _lastName, _dateOfBirth.ToString() }; }
        }

        /*---------------------------------------------------------// Class Methods //---------------------------------------------------------*/
        /// <summary>
        /// Checks if the student profile is 'complete'.
        /// </summary>
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
        }

        /// <summary>
        /// Solidifies a new student by sending it's info to the database methods.
        /// </summary>
        private void SendToDB()
        {
            Database.CreateRecords("tblStudent", StudentInfo);
        }

        /// <summary>
        /// Populates the database with some pre-made students.
        /// </summary>
        static public void PopulateStudents()
        {
            Database.ResetDataBase();
            Student oliver = new Student("TheGreenArrow", "X", "Oliver", "Queen", new DateTime(1998, 06, 15));
            Student felicity = new Student("Overwatch", "X", "Felicity", "Smoak", new DateTime(1998, 06, 15));
            Student john = new Student("Spartan", "X", "John", "Diggle", new DateTime(1998, 06, 15));
            Student adrian = new Student("Prometheus", "X", "Adrian", "Chase", new DateTime(1998, 06, 15));
            Student thea = new Student("Speedy", "X", "Thea", "Queen", new DateTime(1998, 06, 15));
            Student ray = new Student("TheAtom", "X", "Ray", "Palmer", new DateTime(1998, 06, 15));
            Student laurel = new Student("BlackCanary", "X", "Laurel", "Lance", new DateTime(1998, 06, 15));
            Student damien = new Student("DamienDarkh", "X", "Damien", "Darkh", new DateTime(1998, 06, 15));
            Student laurelX = new Student("BlackSiren", "X", "Laurel", "Lance", new DateTime(1998, 06, 15));
            Student curtis = new Student("MisterTerrific", "X", "Curtis", "Holt", new DateTime(1998, 06, 15));
            Student rene = new Student("WildDog", "X", "Rene", "Ramirez", new DateTime(1998, 06, 15));
        }
    }
}
