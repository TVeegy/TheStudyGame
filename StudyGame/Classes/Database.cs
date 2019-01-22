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

        /// <summary>
        /// Populates the DB with examples.
        /// </summary>
        public static void PopulateDB()
        {
            #region Adding Users to the Database
            String tableName = "tblStudent";
            String[,] records =
                { { "The Green Arrow", "X", "Oliver", "Queen", "15/06/1998" }, { "Speedy", "X", "Thea", "Queen", "15/06/1998" }, 
                { "Bratva Captain", "X", "Anatoly", "Knyazev", "15/06/1998" }, { "Spartan", "X", "John", "Diggle", "15/06/1998" },
                { "Overwatch", "X", "Felicity", "Smoak", "15/06/1998" }, { "Black Canary", "X", "Laurel", "Lance", "15/06/1998" } };
            
            CreateRecords(tableName, records);
            #endregion
        }
        
        /// <summary>
        /// Returns by default the first row of 'tblStudent'.
        /// </summary>
        /// <param name="tableName">Name of the table to be used.</param>
        /// <param name="rowIndex">Index of the record to be retrieved.</param>
        public static DataRow RetrieveRow(String tableName = "tblStudent", Int32 rowIndex = 0)
        {
            return DBToDataTable(new SqlCommand($"select * from {tableName}", connection)).Rows[rowIndex];
        }

        public static String RetrieveColValueFromRow(String tableName, DataRow containingRow, String columnName)
        {
            Int32 colIndex = 0;
            DataTable newDataTable = DBToDataTable(new SqlCommand($"select * from {tableName}", connection));
            foreach (DataColumn dc in newDataTable.Columns)
            {
                if (dc.ColumnName == columnName)
                    colIndex = dc.Ordinal;
            }
            return containingRow[colIndex].ToString();
        }
        

        // Amount of results
        public static DataRow RetrieveRowWithKeys(String tableName = "tblStudent", String firstKey="", String secondKey = "", String thirdKey = "")
        {
            DataTable newDataTable = DBToDataTable(new SqlCommand($"select * from {tableName}", connection));
            DataRow outputRow = null;

            Int32 colRange = 0;
            foreach (DataColumn dc in newDataTable.Columns)
            {
                colRange++;
            }

            Int32 keys = 0;
            foreach (String key in new String[] { firstKey, secondKey, thirdKey})
            {
                if (key != "")
                    keys++;
            }

            for (int i = 0; i < newDataTable.Rows.Count; i++)
            {
                Int32 dataMatches = 0;
                DataRow row = newDataTable.Rows[i];
                for (int y = 0; y < colRange; y++)
                {
                    if (row[y].ToString() == firstKey || row[y].ToString() == secondKey || row[y].ToString() == thirdKey)
                        dataMatches++;
                }
                if (dataMatches == keys)
                    outputRow = row;
            }
            return outputRow;
        }

        /// <summary>
        /// Returns by default the first row of 'tblStudent'.
        /// </summary>
        public static void CreateRecords(String tableName, String[,] recordInfo)
        {
            DataTable newDataTable = DBToDataTable(new SqlCommand($"select * from {tableName}", connection));
            
            Int32 columnCounter = 0;
            foreach (DataColumn dc in newDataTable.Columns)
            {
                columnCounter++;
            }
            
            for (int i = 0; i < recordInfo.GetLength(0); i++)
            {
                DataRow newRow = newDataTable.NewRow();
                for (int y = 0; y < recordInfo.GetLength(1); y++)
                {
                    newRow[y+1] = recordInfo[i, y];
                }
                newDataTable.Rows.Add(newRow);
            }

            DataTableToDB(newDataTable, tableName);
        }

        public static DataTable DBToDataTable(SqlCommand command)
        {
            DataTable tblTable = new DataTable();
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            connection.Close();

            adapter.Fill(tblTable);
            return tblTable;
        }

        // Work away tablename, its a function!
        public static void DataTableToDB(DataTable dt, String tableName)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"select * from {tableName}", connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

            connection.Open();
            adapter.Update(dt);
            connection.Close();
        }

        private static void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            MessageBox.Show($"{e.OriginalState} -> {e.CurrentState}");
        }

        public static void ShowCaseDBFunctions()
        {
            connection.StateChange += Connection_StateChange;
            
            SqlCommand cmdDaredevil = new SqlCommand("insert into tblStudent (Username, Password, FirstName, LastName) values ('Daredevil', 'Password', 'Matt', 'Murdock')", connection);
            SqlCommand cmdLookupA = new SqlCommand("select Firstname from tblStudent where FirstName like '%a%'", connection);

            connection.Open();
            cmdDaredevil.ExecuteNonQuery();
            MessageBox.Show($"Adding Matt to the database, Displaying first name with an 'a': {cmdLookupA.ExecuteScalar()}");
            connection.Close();

            connection.Open();
            SqlCommand cmdLastAdded = new SqlCommand("SELECT Username FROM tblStudent WHERE StudentID = (SELECT MAX(StudentID) FROM tblStudent)", connection);
            SqlCommand cmdBeforeLastAdded = new SqlCommand("SELECT Username FROM tblStudent WHERE StudentID = (SELECT (MAX(StudentID)-1) FROM tblStudent)", connection);
            MessageBox.Show($"Last added: {cmdLastAdded.ExecuteScalar()} and under that: {cmdBeforeLastAdded.ExecuteScalar()}");
            connection.Close();

            SqlCommand cmdFindA = new SqlCommand("select Username from tblStudent where FirstName like '%a%'", connection);

            connection.Open();
            SqlDataReader reader = cmdFindA.ExecuteReader();
            while (reader.Read())
            {
                MessageBox.Show($"Usernames of students with an 'a' in the name: {reader.GetValue(0)}");
            }
            connection.Close();
        }

        public static void ShowCaseExceptionHandling()
        {
            try
            {
                // Foute conversie;
                object objectString = "tets";
                Int32 testInt = Convert.ToInt32(objectString);

                // Foute SQL
                SqlCommand command = new SqlCommand("select NOTAQUERY from tblStudent", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception ex)
            {
                switch (ex.HResult)
                {
                    default:
                        ErrorLog.AddErrorMessage(ex, HelperMethods.GetMethodName());
                        break;
                }

                MessageBox.Show("Message stored");
                String output = "";
                foreach (String item in ErrorLog.ErrorLogList)
                {
                    output += "\n" + item;
                }
                MessageBox.Show(output);
            }
        }

        /// <summary>
        /// Wiping out any entries in the database.
        /// </summary>
        /// <param tableName="dbStudent">Used to wipe a specified table.</param>
        public static void ResetDataBase(String tableName = default(String))
        {
            SqlCommand command;
            if (tableName == default(String))
            {
                connection.Open();
                DataTable tableSchema = connection.GetSchema("Tables"); // Collect all table names and truncate them
                foreach (DataRow item in tableSchema.Rows)
                {
                    command = new SqlCommand("TRUNCATE TABLE " + item[2].ToString(), connection);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

            else
            {
                connection.Open();
                command = new SqlCommand("TRUNCATE TABLE " + tableName, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
