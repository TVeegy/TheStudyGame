using StudyGame.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            Student.PopulateStudents();

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            String message = "";
            switch (Login.HandleLogin(txtUsername.Text, pwbPassword.Password))
            {
                case "success":
                    message = "Login successful!";
                    break;
                case "username":
                    message = "wrong password!";
                    break;
                case "password":
                    message = "Wrong username!";
                    break;
                case "":
                    message = "Neither of both was right";
                    break;
            }
            MessageBox.Show(message);
        }
    }
}
