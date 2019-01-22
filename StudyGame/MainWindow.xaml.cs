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
        SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=dbStudyGame;Integrated Security=True");
        
        public MainWindow()
        {
            InitializeComponent();
            Database.ResetDataBase();
            Database.PopulateDB();
            //Database.TestDBFunctions();
            Database.ShowCaseExceptionHandling();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            String keyword = "Oliver";

            DataRow row = Database.RetrieveRowWithKeys("tblStudent", keyword);
            MessageBox.Show($"Using keyword '{keyword}' resulted in '{row[1]}'");

            String name = Database.RetrieveColValueFromRow("tblStudent", row, "FirstName") + " " + Database.RetrieveColValueFromRow("tblStudent", row, "LastName");
            MessageBox.Show($"Search based on column name resulted in '{name}'");
        }
    }
}
