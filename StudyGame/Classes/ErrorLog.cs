using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGame.Classes
{
    public class ErrorLog
    {
        private static List<String> _errorLogList = new List<String>();
        private static List<Object[]> _errorLogExceptions = new List<Object[]>();
        private static Int32 _errorID = 1;

        public static Int32 ErrorID
        {
            get { return _errorID; }
            private set { _errorID = value; }
        }

        public static List<String> ErrorLogList
        {
            get { return _errorLogList; }
            private set { _errorLogList = value; }
        }

        public static List<Object[]> ErrorLogExceptions
        {
            get { return _errorLogExceptions; }
            private set { _errorLogExceptions = value; }
        }

        public static void AddErrorMessage(Exception ex, String methodName)
        {
            List<String> tempErrorList = ErrorLogList;
            tempErrorList.Add($"Error ID: {ErrorID}\nMessage: {ex.Message}\nHResult: {ex.HResult} at {DateTime.Now}\nOriginating from method:{methodName}");
            ErrorLogList = tempErrorList;

            List<Object[]> tempExceptionList = ErrorLogExceptions;
            Object[] arrayOfExceptionAndSourceMethod = { ex, methodName };
            tempExceptionList.Add(arrayOfExceptionAndSourceMethod);
            ErrorLogExceptions = tempExceptionList;

            Int32 tempErrorID = ErrorID;
            tempErrorID++;
            ErrorID = tempErrorID;
        }
    }
}
