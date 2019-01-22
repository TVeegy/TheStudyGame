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
        private static Student oliver = new Student("GreenArrow", "XXXXX", "Oliver", "Queen", "man", new DateTime(1998,06,15));
        private static Student felicity = new Student("Overwatch", "XXXXX", "Felicity", "Smoak", "woman", new DateTime(1998, 06, 15));
        private static Student john = new Student("Spartan", "XXXXX", "John", "Diggle", "man", default(DateTime));
        private static Student adrian = new Student("Prometheus", "XXXXX", "Adrian", "Chase", "", new DateTime(1998, 06, 15));
        private static Student thea = new Student("Al Ghul", "XXXXX", "Thea", "Queen", "woman", new DateTime(1998, 06, 15));
        private static Student ray = new Student("The Atom", "XXXXX", "Ray", "", "man", new DateTime(1998, 06, 15));
        private static Student laurel = new Student("Black Canary", "XXXXX", "", "Lance", "woman", new DateTime(1998, 06, 15));


        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            
        }


    }
}
