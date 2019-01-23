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
        public static DataRow activeUser = null;


        /*---------------------------------------------------------// Populate the Database //---------------------------------------------------------*/
        /// <summary>
        /// Populates the DB with examples.
        /// </summary>
        public static void PopulateDB()
        {
            #region Adding Users to the Database
            String tableName = "tblStudent";
            String[,] records =
                { { "TheGreenArrow", "X", "Oliver", "Queen", "15/06/1998" }, { "Speedy", "X", "Thea", "Queen", "15/06/1998" }, 
                { "BratvaCaptain", "X", "Anatoly", "Knyazev", "15/06/1998" }, { "Spartan", "X", "John", "Diggle", "15/06/1998" },
                { "Overwatch", "X", "Felicity", "Smoak", "15/06/1998" }, { "BlackCanary", "X", "Laurel", "Lance", "15/06/1998" } };
            
            CreateRecords(tableName, null, records);
            #endregion
        }

        /*---------------------------------------------------------// Retrieve Data from the Database //---------------------------------------------------------*/

        /// <summary>
        /// Retrieves a single Datarow from a Datatable when given a tableName and/or rowIndex.
        /// </summary>
        /// <param name="tableName">Name of the table to be used.</param>
        public static DataRow RetrieveRow(String tableName, Int32 rowIndex = 0)
        {
            return FillDataTable(new SqlCommand($"select * from {tableName}", connection)).Rows[rowIndex];
        }

        /// <summary>
        /// Retrieves the column index when given a tableName and a columnName.
        /// </summary>
        public static Int32 RetrieveColumnIndex(String tableName, String columnName)
        {
            DataTable newDataTable = FillDataTable(new SqlCommand($"select * from {tableName}", connection));

            Int32 columnIndex = 0;
            foreach (DataColumn dc in newDataTable.Columns)
            {
                if (dc.ColumnName == columnName)
                    columnIndex = dc.Ordinal;
            }
            return columnIndex;
        }

        /// <summary>
        /// Retrieves a String value from a Datarow when given a columnName.
        /// </summary>
        public static String RetrieveColumnName(DataRow containingRow, String columnName)
        {
            String tableName = containingRow.Table.TableName;
            DataTable dt = FillDataTable(new SqlCommand($"select * from {tableName}", connection));

            return containingRow[RetrieveColumnIndex(tableName, columnName)].ToString();
        }


        /*---------------------------------------------------------// Retrieve Login-Data from the Database //---------------------------------------------------------*/
        
        /// <summary>
        /// Retrieves username and password from the database and returns succes or the correct value.
        /// </summary>
        public static String RetrieveAndCheckLogin(String username, String password)
        {
            DataTable dt = FillDataTable(new SqlCommand($"select * from tblStudent", connection));
            String output = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                if (row[RetrieveColumnIndex("tblStudent", "Username")].ToString() == username && row[RetrieveColumnIndex("tblStudent", "Password")].ToString() == password)
                {
                    activeUser = row;
                    return "success";
                }

                if (row[RetrieveColumnIndex("tblStudent", "Username")].ToString() == username)
                    output = "username";

                if (row[RetrieveColumnIndex("tblStudent", "Password")].ToString() == password)
                    output = "password";
                
            }
            return output;
        }


        /*---------------------------------------------------------// Searching the Database //---------------------------------------------------------*/

        /// <summary>
        /// Returns a datarow when given keys to search with in the database.
        /// </summary>
        public static DataRow RetrieveRowWithKeys(String tableName, String firstKey="", String secondKey = "", String thirdKey = "")
        {
            DataTable dt = FillDataTable(new SqlCommand($"select * from {tableName}", connection));
            DataRow outputRow = null;

            Int32 columnRange = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                columnRange++;
            }

            Int32 givenKeys = 0;
            foreach (String key in new String[] { firstKey, secondKey, thirdKey})
            {
                if (key != "")
                    givenKeys++;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Int32 dataMatches = 0;
                DataRow row = dt.Rows[i];
                for (int y = 0; y < columnRange; y++)
                {
                    if (row[y].ToString() == firstKey || row[y].ToString() == secondKey || row[y].ToString() == thirdKey)
                        dataMatches++;
                }
                if (dataMatches == givenKeys)
                    outputRow = row;
            }
            return outputRow;
        }

        /*---------------------------------------------------------// Creating records in the Database //---------------------------------------------------------*/

        /// <summary>
        /// Creates records by input of an 2D or 1D array when given a tableName.
        /// </summary>
        public static void CreateRecords(String tableName, String[] recordInfo, String[,] multipleRecordInfo=null)
        {
            DataTable newDataTable = FillDataTable(new SqlCommand($"select * from {tableName}", connection));
            
            if (multipleRecordInfo == null)
            {
                DataRow newRow = newDataTable.NewRow();
                for (int i = 0; i < recordInfo.Length; i++)
                {
                    newRow[i + 1] = recordInfo[i];
                }
                newDataTable.Rows.Add(newRow);
            }
            
            else
            {
                Int32 columnCounter = 0;
                foreach (DataColumn dc in newDataTable.Columns)
                {
                    columnCounter++;
                }

                for (int i = 0; i < multipleRecordInfo.GetLength(0); i++)
                {
                    DataRow newRow = newDataTable.NewRow();
                    for (int y = 0; y < multipleRecordInfo.GetLength(1); y++)
                    {
                        newRow[y + 1] = multipleRecordInfo[i, y];
                    }
                    newDataTable.Rows.Add(newRow);
                }
            }

            UpdateDataTable(newDataTable, tableName);
        }


        /*---------------------------------------------------------// Core database communication //---------------------------------------------------------*/

        private static void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
        {
            //MessageBox.Show($"{e.OriginalState} -> {e.CurrentState}");
        }

        /// <summary>
        /// Fills the datatable using the sqlAdapter when given an SQL command.
        /// </summary>
        public static DataTable FillDataTable(SqlCommand fillCommand)
        {
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(fillCommand);
            connection.Close();

            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Updates the datatable using the sqlAdapter when given a datatable
        /// <summary>
        public static void UpdateDataTable(DataTable dt, String tableName)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"select * from {tableName}", connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

            connection.Open();
            adapter.Update(dt);
            connection.Close();
        }
        
        /// <summary>
        /// Resetting all values to nothing in the database.
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


        /*---------------------------------------------------------// Showcasing the database //---------------------------------------------------------*/

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
    }
}
