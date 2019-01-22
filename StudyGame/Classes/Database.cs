using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudyGame.Classes
{
    public class Database
    {
        static SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=dbStudyGame;Integrated Security=True");
        static SqlCommand command;
        static SqlDataAdapter adapter;
        static SqlCommandBuilder builder;

        static DataTable dt;
        static DataSet ds;

        public static void wipeDataBase(String tableName)
        {
            connection.Open();
            // Collect all table names and truncate them
            DataTable tableSchema  = connection.GetSchema("Tables");
            foreach (DataRow item in tableSchema.Rows)
            {
                command = new SqlCommand("TRUNCATE TABLE " + item[2].ToString(), connection);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }

        static public void PopulateTables()
        {
            DataTable dt = new DataTable();

            // Filling the datatable with stored commands
            SqlDataAdapter adapter = LoadAdapter(new SqlCommand("select * from tblStudent", connection));
            adapter.Fill(dt);

            String[,] entries = new String[,] 
            { {"Oliver Queen", "PWD"}, {"Quentin Lance", "PWD"}, { "Thea Merlyn", "PWD" }, { "Felicity Smoak", "PWD" } };

            for (int i = 0; i < entries.GetLength(0); i++)
            {
                DataRow row = dt.NewRow();
                row[1] = entries[i,0];
                row[2] = entries[i, 1];
                dt.Rows.Add(row);
            }
            UpdateAdapter(adapter, dt);
        }

        static SqlDataAdapter LoadAdapter(SqlCommand command)
        {
            connection.Open();
            adapter = new SqlDataAdapter(command);
            builder = new SqlCommandBuilder(adapter);
            connection.Close();
            return adapter;
        }

        static void UpdateAdapter(SqlDataAdapter adapter, DataTable dt)
        {
            // Calling commands in given dataset for operation against database connection
            connection.Open();
            adapter.Update(dt);
            connection.Close();
        }

        private void TestDatabase()
        {
            // Initialising a connection
            String connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=dbStudyGame;Integrated Security=True"; // @ neglects backslashes
            SqlConnection connection = new SqlConnection(connectionString);

            // SQL Command
            SqlCommand command;
            // Represents a set of data commands and a database connection that are used to fill the DataSet and update a SQL Server database.
            SqlDataAdapter adapter;
            // SQL generator object
            SqlCommandBuilder builder;

            // One table with rows and columns
            DataTable tblStudent = new DataTable();
            // Dataset van databases
            DataSet ds = new DataSet();

            connection.Open();

            // Remove everything
            string sqlTrunc = "TRUNCATE TABLE " + "tblStudent";
            SqlCommand cmd = new SqlCommand(sqlTrunc, connection);
            cmd.ExecuteNonQuery();

            // Initialising command into adapter to feed builder.
            command = new SqlCommand("select * from tblStudent", connection);
            adapter = new SqlDataAdapter(command);
            builder = new SqlCommandBuilder(adapter);

            //MessageBox.Show(adapter.SelectCommand.CommandText);
            //MessageBox.Show(builder.GetUpdateCommand().CommandText);
            connection.Close();

            // Filling the datatable with stored commands
            adapter.Fill(tblStudent);

            // Create a first record
            DataRow firstRow = tblStudent.NewRow();
            firstRow[1] = "Matt Murdock";
            firstRow[2] = "Password";
            tblStudent.Rows.Add(firstRow);

            // Create a second record
            DataRow secondRow = tblStudent.NewRow();
            secondRow[1] = "Foggy Nelson";
            secondRow[2] = "Password";
            tblStudent.Rows.Add(secondRow);

            // Create a third record
            DataRow thirdRow = tblStudent.NewRow();
            thirdRow[1] = "Karen Page";
            thirdRow[2] = "Password";
            tblStudent.Rows.Add(thirdRow);

            // Remove the first and last record
            for (int index = tblStudent.Rows.Count - 1; index > 1; index--)
            {
                tblStudent.Rows.RemoveAt(index - 1);
            }

            // Calling commands in given dataset for operation against database connection
            connection.Open();
            adapter.Update(tblStudent);
            connection.Close();

            // Filling dataset with command in adapter.
            adapter.Fill(ds, "tblStudent");
            //adapter.Fill(tblStudent);

            // Showing rows in dataset table tblStudent
            foreach (DataRow row in ds.Tables["tblStudent"].Rows)
            {
                //MessageBox.Show($"Username: {row[1]} -  Password: {row[2]}");
            }

            Connected();
        }

        private static void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            MessageBox.Show($"{e.OriginalState} -> {e.CurrentState}");
        }

        static void Connected()
        {
            String connectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=dbStudyGame;Integrated Security=True"; // @ neglects backslashes
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command;
            SqlDataReader reader;
            int resultaat;

            connection.StateChange += Connection_StateChange;
            connection.Open();

            // Adding the entry "Daredevil"
            command = new SqlCommand("insert into tblStudent (Username, Password) values ('Daredevil', 'Password')", connection);
            //MessageBox.Show($"Rows affected: {resultaat = command.ExecuteNonQuery()}");

            // Performing a query and returning the first result
            command = new SqlCommand("select count(*) from tblStudent", connection);
            //MessageBox.Show($"Number of entries: {resultaat = (int)command.ExecuteScalar()}");
            command = new SqlCommand("SELECT Username FROM tblStudent WHERE StudentID = (SELECT MAX(StudentID) FROM tblStudent)", connection);
            //MessageBox.Show($"Last entry Username: {command.ExecuteScalar()}");

            // NOTE: DOesn't work with multiple exceptions (for logging)
            try
            {
                // Reading all entries in database using the command's connection
                command = new SqlCommand("select *ERROR from tblStudent", connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //MessageBox.Show($"{reader.GetInt32(0)}: {reader.GetValue(1)}"); //nummer vd kolom vanaf 0
                }
            }

            catch (Exception ex) // Default foutmelding
            {
                switch (ex.HResult) //HResult = getal ivm een specifieke fout
                {
                    case -2146233079:
                        ErrorLog.AddErrorMessage(ex, HelperMethods.GetMethodName());
                        break;
                    case -2146232060:
                        ErrorLog.AddErrorMessage(ex, HelperMethods.GetMethodName());
                        break;
                    default:
                        ErrorLog.AddErrorMessage(ex, HelperMethods.GetMethodName());
                        break;
                }
            }

            // command = new SqlCommand("delete from tblStudent", connection);
            // MessageBox.Show($"Execute: {command.ExecuteNonQuery()}");
            // Resultaat vd query = ExecuteScalar (object)
            // NonQuery = hoeveelheid betrokken records (integer)
            connection.Close();

            String errors = "";
            foreach (String error in ErrorLog.ErrorLogList)
            {
                errors += error + "\n";
            }
            MessageBox.Show(errors);
        }
    }
}
