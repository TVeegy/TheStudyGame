using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudyGame.Classes
{
    public class HelperMethods
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static String GetMethodName()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }
    }
}
