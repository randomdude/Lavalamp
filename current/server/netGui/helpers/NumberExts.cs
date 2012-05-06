namespace netGui.helpers
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class numberExts
    {
        public static bool isNumber(this int x , object check)
        {
            if (check == null) 
                return false;
            if (check is Int32) 
                return true;
            int num;
            if (check is string)
                return int.TryParse(check as string, out num);
            
            return int.TryParse(check.ToString(), out num);
        }
   
        public static char[] toCharArray(this string[] x)
        {
            char[] toRet = new char[x.Length];
            for (int i = 0; i < x.Length;i++ )
            {
                if (x[i].Length > 1 || x[i].Length < 1)
                    throw new InvalidCastException();
                toRet[i] = x[i][0];
            }
            return toRet;
        }
    }

   

}
