using System;
using System.Collections;
using System.Text;

namespace Sramek.FX
{
    public static class Messages
    {
        public static string BuildErrorMessage(string aMessagePrefix, Exception aEx, bool aIncludeInnerExceptions = true)
        {
            return aMessagePrefix + BuildErrorMessage(aEx, aIncludeInnerExceptions);
        }

        public static string BuildErrorMessage(Exception aEx, bool aIncludeInnerExceptions = true)
        {
            if (aEx == null)
            {
                return "Can't print exception to string ! Exception object is null !";
            }

            var lMessageOnly = false;


            var lSb = new StringBuilder();
            lSb.Append((aEx.Message) + "\r\n");

            if (!lMessageOnly && aEx.Data != null && aEx.Data.Count > 0)
            {
                lSb.Append("\r\nParameters:\r\n");
                try
                {
                    foreach (DictionaryEntry nItem in aEx.Data)
                    {
                        lSb.Append(nItem.Key + ": ");
                        lSb.Append(nItem.Value + "\r\n");
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }

            if (!lMessageOnly && !string.IsNullOrEmpty(aEx.StackTrace))
            {
                lSb.AppendLine("\r\nStackTrace:" + aEx.StackTrace + "\r\n");
            }

            if (aIncludeInnerExceptions && aEx.InnerException != null)
            {
                lSb.Append("\r\nInnerException:\r\n");
                lSb.Append(BuildErrorMessage(aEx.InnerException, true));
            }

            return lSb.ToString();
        }


    }
}